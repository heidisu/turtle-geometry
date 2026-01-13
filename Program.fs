namespace TurtleGeometry

module Program = 
    open System
    open Microsoft.AspNetCore.Builder

    let builder = WebApplication.CreateBuilder()
    let app = builder.Build()
    app.MapGet("/", Func<string>(fun () -> $"Welcome to workshop")) |> ignore
    app.Run()

