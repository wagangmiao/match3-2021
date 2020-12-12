namespace Shared

type Cell<'T> =
    {
        value: 'T
    }
open System
type ActiveBoard =
    {
        height: int
        width: int
        cells: Cell<int>[,]
    }



module Match3 =
    let make_board height width =
        let b = {
            height = height
            width = width
            cells = Array2D.create height width {value = 0}
        }
        b

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IMatchApi =
    {
      board: unit -> Async<ActiveBoard>
    }