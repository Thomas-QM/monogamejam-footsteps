module UI.Menu

open UI.Model
open Game.Model
open Microsoft.Xna.Framework
open Utility

type ModelControl = PositionedControl<GameDependencies,Message>

let text1 = MakeControl (400, 100) 200 200 0 (Text ("MONOGAMEJAM - FOOTSTEPS", Color.White))
let playbutton game = MakeButton (if Option.isSome game then "Continue" else "Play") (IntoGame) |> MakeControl (400, 230) 200 50 10
let quitbutton = MakeButton "Quit" (Quit) |> MakeControl (400, 280) 200 50 10
let creditsbutton = MakeButton "Credits" (ToCredits) |> MakeControl (400, 330) 200 50 10
let menuui game = [text1; playbutton game; creditsbutton; quitbutton]

let backbutton:ModelControl = MakeButton "Back" (fun x -> ToMainMenu) |> MakeControl (400, 350) 200 50 5
let credittexts:ModelControl list = Credit.Credits.Split([|'\n'|]) |> Array.toList |> List.mapi (fun i x -> Text (x, Color.White) |> MakeControl (400, i*60+100) 600 50 0)
let credits = [backbutton]@credittexts