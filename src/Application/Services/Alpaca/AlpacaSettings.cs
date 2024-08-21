namespace Application.Services.Alpaca;

public class AlpacaSettings
{
    public const string Alpaca = "Alpaca";

    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}