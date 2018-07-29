module Game.Update

open Game
open System
open Model
open Environment.TileSelector
open Environment.Model
open MonoGame.Extended
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let mutable state = MainMenu

let update msg =
    let newstate =
        match state, msg with
            | MainMenu, IntoGame {Graphics=graphics} ->
                ActiveGame {Entities=[]; Environment=defaultenv; Camera=new Camera2D(graphics.GraphicsDevice)}
            | ActiveGame _, ToMainMenu -> state
            | ActiveGame x, Draw {TileSet=tileset; Sprite=sprite; Graphics=graphics} ->
                let {Camera=cam; Environment=env;} = x
                sprite.Begin(transformMatrix=Nullable(cam.GetViewMatrix()), sortMode=SpriteSortMode.FrontToBack, samplerState=SamplerState.PointClamp)

                let newenv = RenderTiles graphics cam env tileset sprite
                sprite.End()
                ActiveGame {x with Environment=newenv}
            | ActiveGame {Camera=cam}, Update (time, x) -> cam.Move(new Vector2(time.TotalSeconds*15.0 |> float32)); state

    state <- newstate