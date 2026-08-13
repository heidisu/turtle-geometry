module TurtleGeometry.Program

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Giraffe
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe.ViewEngine
open TurtleGeometry.Core
open Microsoft.AspNetCore.Http
open System

let turtleHandler: HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        let showArrow =
            match ctx.TryGetQueryStringValue "arrow" with
            | None -> false
            | Some arrow ->
                match Boolean.TryParse arrow with
                | true, value -> value
                | false, _ -> false

        htmlString (RenderView.AsString.htmlDocument (WebPage.htmlPage webPagePath showArrow)) next ctx

let webApp = choose [ route "/" >=> turtleHandler ]

let configureApp (app: IApplicationBuilder) = app.UseGiraffe webApp

let configureServices (services: IServiceCollection) = services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder.Configure(configureApp).ConfigureServices(configureServices).UseUrls("http://*:3030")
            |> ignore)
        .Build()
        .Run()

    0
