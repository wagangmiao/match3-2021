namespace Shared.Chess

open System.Runtime.InteropServices
open System.Xml

module BlackWhite =
    type Color =
        | Black
        | White
        | Blank

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

    let StartGame(): Game =
        {
            board = {
                height = 8
                width = 8
                board = Array2D.create 8 8 Blank
            }

            records = []
            next_turn = Black
            current_turn_number = 1
        }