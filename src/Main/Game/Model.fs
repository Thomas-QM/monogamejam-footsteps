module Game.Model

open System.IO
open Microsoft.Xna.Framework
open Environment.Model
open Newtonsoft.Json

type Message =
    | ToMainMenu
    | IntoGame
    | PlayerMove of Vector2
    | PlayerGrab

type Item = Ball | Trash

type PlayerState = {Position:Vector2; Mouth:Item option;}
type Game = {PlayerState:PlayerState; Environment: Environment;}
type GameState = MainMenu | ActiveGame of Game

let SaveState state =
    let x = JsonConvert.SerializeObject state
    
    ()