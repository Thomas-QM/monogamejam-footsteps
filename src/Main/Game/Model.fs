module Game.Model

open System.IO
open Microsoft.Xna.Framework
open Environment.Model
open Newtonsoft.Json
open Entity
open MonoGame.Extended
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open InputStateManager
open MonoGame.Extended.ViewportAdapters
open UI.Model
open Comora

type GameDependencies = {Viewport:BoxingViewportAdapter; UIDependencies:UIDependencies; Content:ContentManager; Graphics:GraphicsDeviceManager; Camera:ICamera; TileSet:TileSet; Sprite: SpriteBatch; Input:InputManager}

type Message =
    | ToMainMenu
    | IntoGame of GameDependencies
    | PlayerMove of Vector2
    | PlayerGrab
    | Draw of GameDependencies
    | Update of GameTime*GameDependencies
    | Resize of int*int

type Game = {Entities:Entity list; Environment: Environment}
type GameState = MainMenu of UI<GameDependencies, Message> | ActiveGame of Game

let defaultstate = MainMenu

let SaveState state =
    let x = JsonConvert.SerializeObject state

    ()