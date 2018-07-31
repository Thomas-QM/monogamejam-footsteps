module UI.Menu

open UI.Model
open Game.Model
open Microsoft.Xna.Framework

let text1 = MakeControl (400, 100) 200 200 0 (Text ("MONOGAMEJAM - FOOTSTEPS", Color.White))
let playbutton game = MakeButton (if Option.isSome game then "Continue" else "Play") (IntoGame) |> MakeControl (400, 240) 200 50 10
let quitbutton = MakeButton "Quit" (Quit) |> MakeControl (400, 350) 200 50 10
let menuui game = [text1; playbutton game; quitbutton]