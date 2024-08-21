namespace Application.Services.Alpaca.Models;

public record NewsResponse(
    News[] news,
    string next_page_token
);

public record News(
    string author,
    string content,
    string created_at,
    string headline,
    int id,
    Images[] images,
    string source,
    string summary,
    string[] symbols,
    string updated_at,
    string url
);

public record Images(
    string size,
    string url
);

