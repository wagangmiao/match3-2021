module Index

open Browser.Types
open Elmish
open Fable.Remoting.Client
open Shared
open Browser
open Fable.Core
type Model =
    { Todos: Todo list
      Input: string }

type Msg =
    | GotTodos of Todo list
    | SetInput of string
    | AddTodo
    | AddedTodo of Todo

let todosApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let init(): Model * Cmd<Msg> =
    let model =
        { Todos = []
          Input = "" }
    let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos
    model, cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | GotTodos todos ->
        { model with Todos = todos }, Cmd.none
    | SetInput value ->
        { model with Input = value }, Cmd.none
    | AddTodo ->
        let todo = Todo.create model.Input
        let cmd = Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo
        { model with Input = "" }, cmd
    | AddedTodo todo ->
        { model with Todos = model.Todos @ [ todo ] }, Cmd.none

open Fable.React
open Fable.React.Props
open Fulma

let navBrand =
    Navbar.Brand.div [ ] [
        Navbar.Item.a [
            Navbar.Item.Props [ Href "https://safe-stack.github.io/" ]
            Navbar.Item.IsActive true
        ] [
            img [
                Src "/favicon.png"
                Alt "Logo"
            ]
        ]
    ]

let containerBox (model : Model) (dispatch : Msg -> unit) =
    Box.box' [ ] [
        Content.content [ ] [
            Content.Ol.ol [ ] [
                for todo in model.Todos do
                    li [ ] [ str todo.Description ]
            ]
        ]
        Field.div [ Field.IsGrouped ] [
            Control.p [ Control.IsExpanded ] [
                Input.text [
                  Input.Value model.Input
                  Input.Placeholder "What needs to be done?"
                  Input.OnChange (fun x -> SetInput x.Value |> dispatch) ]
            ]
            Control.p [ ] [
                Button.a [
                    Button.Color IsPrimary
                    Button.Disabled (Todo.isValid model.Input |> not)
                    Button.OnClick (fun _ -> dispatch AddTodo)
                ] [
                    str "Add"
                ]
            ]
        ]
    ]
let canvasInit() =
    let canvas = document.querySelector(".view") :?> HTMLCanvasElement
    let ctx = canvas.getContext_2d()
    ctx.fillStyle <- U3.Case1 "rgb(200,0,0)"
    ctx.fillRect (0., 0., 500., 500.)
    ctx.fillStyle <- U3.Case1 "rgba(0, 0, 200, 0.5)"
    ctx.fillRect (30., 30., 55., 50.)

let view (model : Model) (dispatch : Msg -> unit) =
    canvasInit()
    Hero.hero [
        Hero.Color IsPrimary
        Hero.IsFullHeight
        Hero.Props [
            Style [
                Background """linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://unsplash.it/1200/900?random") no-repeat center center fixed"""
                BackgroundSize "cover"
            ]
        ]
    ] [
        Hero.head [ ] [
            Navbar.navbar [ ] [
                Container.container [ ] [ navBrand ]
            ]
        ]

        Hero.body [ ] [
            Container.container [ ] [
                Column.column [
                    Column.Width (Screen.All, Column.Is6)
                    Column.Offset (Screen.All, Column.Is3)
                ] [
                    Heading.p [ Heading.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [ str "match3_2021" ]
                    containerBox model dispatch
                ]
            ]
        ]
    ]


