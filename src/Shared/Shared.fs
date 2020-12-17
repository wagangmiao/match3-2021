namespace Shared
open Chess

module Route =
    let builder typeName methodName = sprintf "/api/%s/%s" typeName methodName

type IBlackWhiteApi = { game: unit -> Async<BlackWhite.Game> }
