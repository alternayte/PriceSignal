namespace Application.Services.Binance;

public class BinanceSettings
{
    public const string Binance = "Binance";
    public string WebsocketUrl { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
    public bool Enabled { get; set; }
}