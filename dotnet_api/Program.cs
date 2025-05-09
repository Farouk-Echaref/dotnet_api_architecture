using dotnet_api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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
app.Map("/games", () => games);

// GET /games/{id}
app.Map("/games/{id}", (int id) => games.Find(games => games.Id == id));

app.Run();

