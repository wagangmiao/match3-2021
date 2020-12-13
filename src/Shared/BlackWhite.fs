namespace Shared.Chess

open System.Runtime.InteropServices
open System.Xml

module BlackWhite =
    type Color =
        | Black
        | White

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

    type Record = { turns: Turn list }
