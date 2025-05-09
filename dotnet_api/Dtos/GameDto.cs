namespace dotnet_api.Dtos;

public record class GameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    // string Description,
    DateOnly ReleaseDate
);