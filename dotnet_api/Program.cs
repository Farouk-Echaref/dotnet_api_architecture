using dotnet_api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "GOW",
        "Fantasy",
        19.99M,
        new DateOnly(2008, 02, 01)),
    new (
        2,
        "RDR",
        "RPG",
        69.69M,
        new DateOnly(1999, 12, 25)),
    new (
        3,
        "Witcher",
        "Magic",
        69.420M,
        new DateOnly(2000, 01, 01)),
    
];

app.MapGet("/", () => "Hello World!");
app.MapGet("/fechcha", () => "This is fechcha route");

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("/games/{id}", (int id) => games.Find(games => games.Id == id))
    .WithName(GetGameEndpointName);

// POST /games
// some validation of the data is required to match the dto 
app.MapPost("/games", (CreateGameDto incomingGame) => {
    GameDto newGame =  new GameDto(
        games.Count + 1,
        incomingGame.Name,
        incomingGame.Genre,
        incomingGame.Price,
        incomingGame.ReleaseDate
    );

    games.Add(newGame);

    return Results.CreatedAtRoute(GetGameEndpointName, new {id = newGame.Id}, newGame);
});

app.Run();

