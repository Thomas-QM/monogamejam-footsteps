module Game.Update

open Game
open System
open Model
open Environment.TileSelector
open Environment.Model
open MonoGame.Extended
open Microsoft.Xna.Framework

let mutable state = MainMenu


let update msg =
    let newstate =
        match state, msg with
            | MainMenu, IntoGame {Graphics=graphics} ->
                ActiveGame {Entities=[]; Environment=defaultenv; Camera=new Camera2D(graphics.GraphicsDevice)}
            | ActiveGame _, ToMainMenu -> state
            | ActiveGame x, Draw {TileSet=tileset; Sprite=sprite} ->
                let {Camera=cam; Environment=env} = x
                cam.Move(new Vector2(float32 0.5))
                sprite.Begin(transformMatrix=Nullable(cam.GetViewMatrix()))

                let newenv = RenderTiles cam env tileset sprite
                sprite.End()
                ActiveGame {x with Environment=newenv}

    state <- newstate