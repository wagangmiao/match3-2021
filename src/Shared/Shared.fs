namespace Shared

open System
type ActiveBoard<'CellType> =
    {
        height: int
        width: int
        cells: 'CellType[,]
    }

type Cell<'T> =
    {
        x: int
        y: int
        value: 'T
    }

module Match3 =
    let make_board height width =
        let b = {
            height = height
            width = width
            cells = Array2D.zeroCreateBased 0 0 height width
        }
        b

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IMatchApi =
    {
      board: unit -> Async<ActiveBoard<int>>
    }