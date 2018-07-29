﻿module Game.Model

open System.IO
open Microsoft.Xna.Framework
open Environment.Model
open Newtonsoft.Json
open Entity
open MonoGame.Extended
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Graphics

type GameDependencies = {Content:ContentManager; Graphics:GraphicsDeviceManager; TileSet:TileSet; Sprite: SpriteBatch}

type Message =
    | ToMainMenu
    | IntoGame of GameDependencies
    | PlayerMove of Vector2
    | PlayerGrab
    | Draw of GameDependencies
    | Update of System.TimeSpan*GameDependencies

type Game = {Entities:Entity list; Environment: Environment; Camera:Camera2D}
type GameState = MainMenu | ActiveGame of Game

let defaultstate = MainMenu

let SaveState state =
    let x = JsonConvert.SerializeObject state

    ()