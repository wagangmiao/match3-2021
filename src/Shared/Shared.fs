namespace Shared

open System.Xml

type Color =
    | Black = 0
    | White = 1
    | Red = 2
    | Green = 3
    | Blue = 4

type Tile =
    | Blank = 0
    | Wall = 1


type Cell =
    | Color of color: Color
    | Tile of tile: Tile

open System

type ActiveBoard =
    { height: int
      width: int
      cells: Map<int * int, Cell> }



module Match3 =
    let make_board height width =
        let b =
            { height = height
              width = width
              cells =
                  Map
                      ([| (0, 0), Color(color = Color.Red)
                          (0, 0), Color(color = Color.Red)
                          (0, 0), Color(color = Color.Red) |]) }

        b

module Route =
    let builder typeName methodName = sprintf "/api/%s/%s" typeName methodName

type IMatchApi = { board: unit -> Async<ActiveBoard> }
