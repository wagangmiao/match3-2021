module Index

open System
open Browser.Types
open Elmish
open Fable.Remoting.Client
open Shared
open Browser
open Fable.Core
type Model =
    {
      MouseX: float
      MouseY: float
      Board: ActiveBoard
    }

type Msg =
    | MouseDownEvent of x:float * y:float
    | Board of ActiveBoard
let matchApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IMatchApi>

let init(): Model * Cmd<Msg> =
    let model =
        {
          MouseX = 0.
          MouseY = 0.
          Board = Match3.make_board 2 2
        }
    let cmd = Cmd.OfAsync.perform matchApi.board () Board
    model, cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | MouseDownEvent (x, y) ->
        {
            model with MouseX = x; MouseY = y
        }, Cmd.none
    | Board board ->
        {
            model with Board = board
        }, Cmd.none
open Fulma


let canvasInit (model : Model) (dispatch : Msg -> unit) =
    let canvas = document.querySelector(".view") :?> HTMLCanvasElement
    canvas.onmousedown <- fun (e) -> dispatch(MouseDownEvent(x = e.pageX, y = e.pageY))
    canvas.onmousemove <- fun (e) -> dispatch(MouseDownEvent(x = e.pageX, y = e.pageY))
    let ctx = canvas.getContext_2d()
    let style1 = U3.Case1 "rgb(200,200,0)"
    let gridWidth = 50.
    let gridSize = 10
    let gap = 3.
    ctx.fillStyle <-  U3.Case1 "rgb(255,255,255)"
    ctx.fillRect (0., 0., 500., 500.)
    ctx.fillStyle <- style1

    for i in 0 .. model.Board.height - 1 do
        for j in 0 .. model.Board.height - 1 do
            let x = (float)i * gridWidth + gap
            let y = (float)j * gridWidth + gap
            let w = gridWidth - 2. * gap
            let h = w
            ctx.fillRect (x, y, w, h)

    let style2 = U3.Case1 "rgb(0,200,0)"
    ctx.fillStyle <- style2
    ctx.fillRect(model.MouseX, model.MouseY, 10., 10.)

open Fable.React
let view (model : Model) (dispatch : Msg -> unit) =
    canvasInit model dispatch
    Container.container [ ] [ ]