namespace Shared

open System
type ActiveBoard<'CellType> =
    {
        height: int
        width: int
        cells: array<'CellType>
    }

type Cell<'T> =
    {
        x: int
        y: int
        value: 'T
    }

module Match3 =
    let make_board =
        let b = {
            height = 3
            width = 3
            cells = [|1;2;3;4;5;6;7;8;9|]
        }
        b

type Todo =
    { Id : Guid
      Description : string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    { getTodos : unit -> Async<Todo list>
      addTodo : Todo -> Async<Todo>
      board: unit -> Async<ActiveBoard<int>> }