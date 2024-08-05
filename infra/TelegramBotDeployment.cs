using System;
using System.Collections.Generic;
using System.IO;
using PriceSignal.IaC;
using Pulumi;
using Pulumi.Kubernetes.Core.V1;
using Pulumi.Kubernetes.Networking.V1;
using Pulumi.Kubernetes.Types.Inputs.Apps.V1;
using Pulumi.Kubernetes.Types.Inputs.Core.V1;
using Pulumi.Kubernetes.Types.Inputs.Meta.V1;
using Pulumi.Kubernetes.Types.Inputs.Networking.V1;
using Kubernetes = Pulumi.Kubernetes;

namespace PriceSignal.IaC;

public class TelegramBotDeployment
{
    public TelegramBotDeployment(Namespace webserverNs)
    {
        const string appName = "telegram-bot";
        
        var telegramBotLabels = new InputMap<string>
        {
            { "app", appName },
        };
        
        var config = new Config();
        var env = new[]
        {
            new EnvVarArgs()  
            {
                Name = "TELEGRAM_BOT_TOKEN",
                Value = config.RequireSecret("telegramBotToken"),
            },
            new EnvVarArgs()
            {
                Name = "NATS_URL",
                Value = config.RequireSecret("natsUrl"),
            },
        };
        
        var webserverdeployment = new Kubernetes.Apps.V1.Deployment($"{appName}-deployment", new()
        {
            Metadata = new ObjectMetaArgs
            {
                Name = "telegram-bot",
                Namespace = webserverNs.Metadata.Apply(m => m.Name),
                Labels = telegramBotLabels,
            },
            Spec = new DeploymentSpecArgs
            {
                Replicas = 1,
                Selector = new LabelSelectorArgs
                {
                    MatchLabels = telegramBotLabels,
                },
                Template = new PodTemplateSpecArgs
                {
                    Metadata = new ObjectMetaArgs
                    {
                        Labels = telegramBotLabels,
                    },
                    Spec = new PodSpecArgs
                    {
                        ImagePullSecrets = new InputList<LocalObjectReferenceArgs>
                        {
                            new LocalObjectReferenceArgs
                            {
                                Name = "dockerhub-f4806cff"
                            }  
                        },
                        Containers = 
                        {
                            new ContainerArgs
                            {
                                Image = "nayth/price-signal-telegram-bot:latest",
                                ImagePullPolicy = "Always",
                                Name = appName,
                                Ports = 
                                {
                                    new ContainerPortArgs
                                    {
                                        ContainerPortValue = 80,
                                    },
                                },
                                Env = env
                            },
                        },
                    },
                },
            },
        });
    }
}