namespace Shared.Chess

open System
open System.Diagnostics
open System.Runtime.InteropServices
open System.Xml

module BlackWhite =
    type Color =
        | Black
        | White
        | Blank
    let opposite_color c =
        match c with
        | Black -> White
        | White -> Black
        | Blank -> Blank
    type ChessBoard =
        { height: int
          width: int
          board: Color [,] }

    type Action = { x: int; y: int; color: Color }

    type Change = { x: int; y: int; color: Color }

    type Turn =
        { turn_number: int
          action: Action
          changes: Change list }

    type Records = Turn list

    type Game =
        {
            board: ChessBoard
            records: Records
            next_turn: Color
            current_turn_number: int
        }

    type GameError =
        | NotValidColor
        | PosOutRange
        | PosHasChess
        | InvalidPos
    let StartGame(): Game =
        let game = {
            board = {
                height = 8
                width = 8
                board = Array2D.create 8 8 Blank
            }

            records = []
            next_turn = Black
            current_turn_number = 1
        }
        game.board.board.[3,3] <- Black
        game.board.board.[4,4] <- Black
        game.board.board.[3,4] <- White
        game.board.board.[4,3] <- White
        game

    let ValidPos board x y =
        x >= 0 && y >= 0 && x < board.width && y < board.height

    let CheckOrChangeBoard(board: ChessBoard, color, x, y): Option<ChessBoard> =
        let folder2 state x1 x2 =
            let folder state (x,y) =
                match state with
                |(0, l) -> if ValidPos board x y && board.board.[y, x] = Blank then (1, l) else (-1, l)
                |(-1, l) -> (-1, l)
                |(-2, l) -> (-2, l)
                |(1, l) -> if ValidPos board x y && board.board.[y, x] = opposite_color(color) then (2, (x, y):: l) else (-1, l)
                |(2, l) -> if ValidPos board x y && board.board.[y, x] = opposite_color(color)
                                then (2, (x, y):: l)
                                elif ValidPos board x y && board.board.[y, x] = color
                                then (3, l)
                                else (-2, l)
                |_ -> state

            let r =
                if x1 = 0 && x2 = 0
                then (-1, [])
                else List.fold folder (0, []) [for i in 0..7 -> (x1+i, x2+i)]

            match state, r with
            |(_, l1), (3, l2) -> (3, l1@l2)
            |_, _ -> state

        let r = List.fold2 folder2 (-1, []) [-1; 0; 1] [-1; 0; 1]
        match r with
        |(3, l) ->
            let b = Array2D.copy board.board
            let f = fun (x, y) -> (board.board.[y, x] <- color) in
            List.iter f l

            Some({
                board with board = b
            })
        |_ -> None

    let NextTurn (game:Game,color:Color, x: int, y: int): Result<Game, GameError> =
        if game.next_turn <> color
        then Error NotValidColor
        elif not (ValidPos game.board x y)
        then Error PosOutRange
        elif game.board.board.[y, x] <> Blank
        then Error PosHasChess
        else
            match CheckOrChangeBoard(game.board, color, x, y) with
            |None -> Error InvalidPos
            |Some(board) -> Ok {
                game with board = board
            }

