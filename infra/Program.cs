using System.Collections.Generic;
using System.IO;
using Pulumi;
using Pulumi.Kubernetes.Core.V1;
using Pulumi.Kubernetes.Networking.V1;
using Pulumi.Kubernetes.Types.Inputs.Apps.V1;
using Pulumi.Kubernetes.Types.Inputs.Core.V1;
using Pulumi.Kubernetes.Types.Inputs.Meta.V1;
using Pulumi.Kubernetes.Types.Inputs.Networking.V1;
using Kubernetes = Pulumi.Kubernetes;

return await Deployment.RunAsync(() => 
{
    var config = new Config();
    var k8sNamespace = config.Get("namespace") ?? "default";
    var numReplicas = config.GetInt32("replicas") ?? 1;
    var appLabels = new InputMap<string>
    {
        { "app", "price-signal-graph" },
    };

    var webserverNs = new Namespace("webserverNs", new()
    {
        Metadata = new ObjectMetaArgs
        {
            Name = k8sNamespace,
        },
    });

    var webserverconfig = new ConfigMap("appsettings", new()
    {
        Metadata = new ObjectMetaArgs
        {
            Namespace = webserverNs.Metadata.Apply(m => m.Name),
        },
        Data = 
        {
            { "appsettings.json", File.ReadAllText("../src/PriceSignal/appsettings.json") },
        },
    });

    var webserverdeployment = new Kubernetes.Apps.V1.Deployment("price-signal-graph-deployment", new()
    {
        Metadata = new ObjectMetaArgs
        {
            Name = "price-signal-graph",
            Namespace = webserverNs.Metadata.Apply(m => m.Name),
        },
        Spec = new DeploymentSpecArgs
        {
            Selector = new LabelSelectorArgs
            {
                MatchLabels = appLabels,
            },
            Replicas = numReplicas,
            Template = new PodTemplateSpecArgs
            {
                Metadata = new ObjectMetaArgs
                {
                    Labels = appLabels,
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
                    Containers = new[]
                    {
                        new ContainerArgs
                        {
                            Image = "nayth/price-signal-graph:latest",
                            ImagePullPolicy = "Always",
                            Name = "price-signal-graph",
                            Ports = new []
                            {
                                new ContainerPortArgs
                                {
                                    ContainerPortValue = 8080
                                },
                            },
                            VolumeMounts = new[]
                            {
                                new VolumeMountArgs
                                {
                                    MountPath = "/app/appsettings.json",
                                    Name = "appsettings-volume",
                                    ReadOnly = true,
                                    SubPath = "appsettings.json",
                                },
                                new VolumeMountArgs
                                {
                                    MountPath = "/app/secrets",
                                    Name = "price-signal-secret-volume",
                                    ReadOnly = true,
                                },
                            },
                        },
                    },
                    Volumes = new[]
                    {
                        new VolumeArgs
                        {
                            ConfigMap = new ConfigMapVolumeSourceArgs
                            {
                                Items = new[]
                                {
                                    new KeyToPathArgs
                                    {
                                        Key = "appsettings.json",
                                        Path = "appsettings.json",
                                    },
                                },
                                Name = webserverconfig.Metadata.Apply(m => m.Name),
                            },
                            Name = "appsettings-volume",
                        },
                        new VolumeArgs
                        {
                            Name = "price-signal-secret-volume",
                            Secret = new SecretVolumeSourceArgs()
                            {
                                SecretName = "timescale-cluster-app"
                            }
                        }
                    },
                },
            },
        },
    });

    var webserverservice = new Service("price-signal-graph-service", new()
    {
        Metadata = new ObjectMetaArgs
        {
            Namespace = webserverNs.Metadata.Apply(m => m.Name),
        },
        Spec = new ServiceSpecArgs
        {
            Ports = new[]
            {
                new ServicePortArgs
                {
                    Port = 80,
                    TargetPort = 8080,
                },
            },
            Selector = appLabels,
        },
    });
    
    var webserveringress = new Ingress("price-signal-graph-ingress", new()
    {
        Metadata = new ObjectMetaArgs
        {
            Namespace = webserverNs.Metadata.Apply(m => m.Name),
            Annotations = new InputMap<string>
            {
                { "kubernetes.io/ingress.class", "nginx" },
                {"cert-manager.io/cluster-issuer", "letsencrypt-prod"}
            },
            Name = "price-signal-graph-ingress",
        },
        Spec = new IngressSpecArgs
        {
            Tls = new InputList<IngressTLSArgs>()
            {
                new IngressTLSArgs
                {
                    Hosts = new[] { "price-signal-graph.nxtspec.com" },
                    SecretName = "price-signal-graph-tls",
                },
            },
            Rules = new[]
            {
                new IngressRuleArgs
                {
                    Host = "price-signal-graph.nxtspec.com",
                    Http = new HTTPIngressRuleValueArgs
                    {
                        Paths = new[]
                        {
                            new HTTPIngressPathArgs
                            {
                                Path = "/",
                                PathType = "Prefix",
                                Backend = new IngressBackendArgs
                                {
                                    Service = new IngressServiceBackendArgs
                                    {
                                        Name = webserverservice.Metadata.Apply(m=>m.Name),
                                        Port = new ServiceBackendPortArgs
                                        {
                                            Number = webserverservice.Spec.Apply(s => s.Ports[0].Port),
                                        }
                                    }
                                    },
                            },
                        },
                    },
                },
            },
        },
    });

    return new Dictionary<string, object?>
    {
        ["deploymentName"] = webserverdeployment.Metadata.Apply(metadata => metadata?.Name),
        ["serviceName"] = webserverservice.Metadata.Apply(metadata => metadata?.Name),
    };
});
