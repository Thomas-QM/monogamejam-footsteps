module Game.Input
open Utility.Animation

open Model
open InputStateManager
open System
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework

let vecfrombool w a s d =
    let tonum x = function true -> x | false -> 0
    Vector2 ([tonum -1 a; tonum 1 d] |> List.sum |> float32, [tonum -1 w; tonum 1 s] |> List.sum |> float32)

let InputMsgs (input:InputManager) (delta:TimeSpan) =
    let ms = Vector2(float32 (delta.TotalMilliseconds/2.0))
    let keydown x = input.Key.Is.Down([|x|])
    [vecfrombool (keydown Keys.W) (keydown Keys.A) (keydown Keys.S) (keydown Keys.D) |> (fun x -> x*ms) |> PlayerMove]
        @if keydown Keys.Escape then [ToMainMenu] else []