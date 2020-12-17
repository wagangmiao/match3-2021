module Index

open System
open Browser.Types
open Elmish
open Fable.Remoting.Client
open Shared
open Browser
open Fable.Core
open Chess

type Model =
    { MouseX: float
      MouseY: float
      Game: BlackWhite.Game }

type Msg =
    | MouseDownEvent of x: float * y: float
    | Game of BlackWhite.Game

let matchApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IBlackWhiteApi>

let init (): Model * Cmd<Msg> =
    let model =
        { MouseX = 0.
          MouseY = 0.
          Game = BlackWhite.StartGame() }

    let cmd =
        Cmd.OfAsync.perform matchApi.game () Game

    model, cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | MouseDownEvent (x, y) -> { model with MouseX = x; MouseY = y }, Cmd.none
    | Game game -> { model with Game = game }, Cmd.none

open Fulma


let canvasInit (model: Model) (dispatch: Msg -> unit) =
    let canvas =
        document.querySelector (".view") :?> HTMLCanvasElement

    canvas.onmousedown <- fun e -> dispatch (MouseDownEvent(x = e.pageX, y = e.pageY))
    canvas.onmousemove <- fun e -> dispatch (MouseDownEvent(x = e.pageX, y = e.pageY))
    let ctx = canvas.getContext_2d ()
    let style1 = U3.Case1 "rgb(200,200,0)"
    let gridWidth = 50.
    let gap = 3.
    ctx.fillRect (0., 0., 500., 500.)
    ctx.fillStyle <- style1
    let white = U3.Case1 "rgb(255,255,255)"
    let black = U3.Case1 "rgb(0, 0, 0)"

    for i in 0 .. model.Game.board.width - 1 do
        for j in 0 .. model.Game.board.height - 1 do
            let x = (float) i * gridWidth + gap
            let y = (float) j * gridWidth + gap
            let w = gridWidth - 2. * gap
            let h = w

            match BlackWhite.GetPos model.Game.board i j with
            | BlackWhite.Black ->
                ctx.fillStyle <- black
                ctx.fillRect (x, y, w, h)
            | BlackWhite.White ->
                ctx.fillStyle <- white
                ctx.fillRect (x, y, w, h)
            | _ -> ignore (0)

    let style2 = U3.Case1 "rgb(0,200,0)"
    ctx.fillStyle <- style2
    ctx.fillRect (model.MouseX, model.MouseY, 10., 10.)

open Fable.React

let view (model: Model) (dispatch: Msg -> unit) =
    canvasInit model dispatch
    Container.container [] []
