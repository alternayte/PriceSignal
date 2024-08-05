package main

import (
	"context"
	"encoding/json"
	"log"
	"os"
	"os/signal"
	"syscall"

	tgbotapi "github.com/go-telegram-bot-api/telegram-bot-api/v5"
	"github.com/joho/godotenv"
	"github.com/nats-io/nats.go"
	"github.com/nats-io/nats.go/jetstream"
)

type ChatIDMessage struct {
	ChatID   int64  `json:"chat_id"`
	Username string `json:"username"`
	UserId   string `json:"user_id"`
}

type Notification struct {
	ChatID  int64  `json:"chat_id"`
	Message string `json:"message"`
}

func main() {
	_ = godotenv.Load()

	// Get the Telegram Bot API token from the environment
	botToken := os.Getenv("TELEGRAM_BOT_TOKEN")
	if botToken == "" {
		log.Fatal("TELEGRAM_BOT_TOKEN must be set")
	}

	// Get the NATS server URL from the environment
	natsURL := os.Getenv("NATS_URL")
	if natsURL == "" {
		log.Fatal("NATS_URL must be set")
	}

	// Connect to the NATS server
	nc, err := nats.Connect(natsURL)
	if err != nil {
		log.Fatalf("Error connecting to NATS server: %v", err)
	}
	defer nc.Close()

	js, err := jetstream.New(nc)
	if err != nil {
		log.Fatal(err)
	}
	ctx := context.Background()
	s, err := js.CreateOrUpdateStream(ctx, jetstream.StreamConfig{
		Name:     "notifications",
		Subjects: []string{"notifications.>"},
	})
	if err != nil {
		log.Fatal(err)
	}

	// Create a new Telegram bot
	bot, err := tgbotapi.NewBotAPI(botToken)
	if err != nil {
		log.Fatalf("Error creating new bot: %v", err)
	}

	// Log the bot name and ID
	log.Printf("Authorized on account %s", bot.Self.UserName)

	c, _ := s.CreateOrUpdateConsumer(ctx, jetstream.ConsumerConfig{
		Durable:       "telegram",
		FilterSubject: "notifications.telegram",
		AckPolicy:     jetstream.AckExplicitPolicy,
	})

	c.Consume(func(msg jetstream.Msg) {
		var notification Notification
		err := json.Unmarshal(msg.Data(), &notification)
		if err != nil {
			log.Printf("Error unmarshaling message: %v", err)
			return
		}

		telegramMsg := tgbotapi.NewMessage(notification.ChatID, notification.Message)
		_, err = bot.Send(telegramMsg)
		if err != nil {
			log.Printf("Error sending message: %v", err)
		}
		msg.Ack()
	})

	// Handle incoming Telegram messages
	u := tgbotapi.NewUpdate(0)
	u.Timeout = 60
	updates := bot.GetUpdatesChan(u)

	for update := range updates {
		if update.Message == nil { // ignore any non-Message updates
			continue
		}

		chatID := update.Message.Chat.ID
		username := update.Message.From.UserName
		userId := update.Message.CommandArguments()

		// Send the chat ID and username to NATS for saving in the main database
		chatIDMessage := ChatIDMessage{
			ChatID:   chatID,
			Username: username,
			UserId:   userId,
		}
		data, err := json.Marshal(chatIDMessage)
		if err != nil {
			log.Printf("Error marshaling message: %v", err)
			continue
		}

		_, err = js.PublishAsync("notifications.init.telegram", data)
		if err != nil {
			log.Printf("Error publishing message: %v", err)
		}

		// Respond to the user
		msg := tgbotapi.NewMessage(chatID, "Hello, your chat ID has been recorded!")
		bot.Send(msg)
	}

	// Wait for exit signal
	sig := make(chan os.Signal, 1)
	signal.Notify(sig, syscall.SIGINT, syscall.SIGTERM)
	<-sig
}
