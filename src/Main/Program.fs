module Program

[<EntryPoint>]
let main argv =
    let game = new FSGame.MainGame()
    game.Run ()

    0