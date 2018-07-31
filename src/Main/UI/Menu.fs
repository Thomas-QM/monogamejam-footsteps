module UI.Menu

open UI.Model
open Game.Model
open Microsoft.Xna.Framework

let text1 = MakeControl (10, 10) 50 20 0 (Text ("Hello world!", Color.Gray))
let playbutton = MakeButton "Play!" (IntoGame) |> MakeControl (200, 200) 500 100 0
let (menuui:UI<GameDependencies, Message>) = [text1; playbutton]