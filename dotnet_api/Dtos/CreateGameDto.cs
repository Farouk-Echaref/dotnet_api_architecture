namespace dotnet_api.Dtos;

public record class CreateGameDto(
    string Name,
    string Genre,
    decimal Price,
    // string Description,
    DateOnly ReleaseDate
);