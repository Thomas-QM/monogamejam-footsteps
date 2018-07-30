module Game.Update

open Game
open System
open Model
open Environment.TileSelector
open Environment.Model
open MonoGame.Extended
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Game.Entity
open Entity

let mutable state = MainMenu

let rec update msg =
    let newstate, dispatch =
        match state, msg with
            | MainMenu, IntoGame {Graphics=graphics; Content=content} ->
                ActiveGame {Entities=defaultEntities content; Environment=defaultenv; Camera=new Camera2D(graphics.GraphicsDevice)}, []
            | ActiveGame _, ToMainMenu -> state, []
            | ActiveGame x, Draw {TileSet=tileset; Sprite=sprite; Graphics=graphics} ->
                let {Camera=cam; Environment=env; Entities=entities} = x
                sprite.Begin(transformMatrix=Nullable(cam.GetViewMatrix()), sortMode=SpriteSortMode.FrontToBack, samplerState=SamplerState.PointClamp)

                RenderTiles graphics env tileset sprite
                DrawEntities sprite entities

                sprite.End()
                state, []
            | ActiveGame x, Update (time, {Input=i; Graphics=graphics}) ->
                let {Camera=cam; Entities=entities; Environment=env} = x
                let player = GetPlayer entities
                cam.LookAt player.Position

                let newenv = UpdateTiles graphics cam env
                let newentities = entities |> UpdateEntities (time.TotalGameTime.TotalSeconds*6.0 |> int)
                ActiveGame {x with Entities=newentities; Environment=newenv}, Input.InputMsgs i time.ElapsedGameTime
            | ActiveGame x, PlayerMove y ->
                let {Entities=entities} = x
                let newentities = MapPlayer (Player.MovePlayer y) entities
                ActiveGame {x with Entities=newentities}, []

    state <- newstate
    dispatch |> List.iter update