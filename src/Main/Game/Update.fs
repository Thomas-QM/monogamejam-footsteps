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
open MonoGame.Extended.ViewportAdapters
open UI.Model
open Comora

let mutable state = MainMenu UI.Menu.menuui

let rec update msg =
    let newstate, dispatch =
        match state, msg with
            | MainMenu _, IntoGame {Graphics=graphics; Content=content; Viewport=view} ->
                ActiveGame {Entities=defaultEntities content; Environment=defaultenv;}, []
            | ActiveGame _, ToMainMenu -> state, []
            | ActiveGame x, Draw {TileSet=tileset; Sprite=sprite; Graphics=graphics; Camera=cam} ->
                let {Environment=env; Entities=entities} = x
                sprite.Begin(cam)

                RenderTiles graphics env tileset sprite
                DrawEntities sprite entities

                sprite.End()
                state, []
            | ActiveGame x, Update (time, {Input=i; Graphics=graphics; Camera=cam}) ->
                let {Entities=entities; Environment=env} = x
                let player = GetPlayer entities
                cam.Position <- player.Position

                let newenv = UpdateTiles graphics cam env
                let newentities = entities |> UpdateEntities (time.TotalGameTime.TotalSeconds*6.0 |> int)
                ActiveGame {x with Entities=newentities; Environment=newenv}, Input.InputMsgs i time.ElapsedGameTime
            | ActiveGame x, PlayerMove y ->
                let {Entities=entities} = x
                let newentities = MapPlayer (Player.MovePlayer y) entities
                ActiveGame {x with Entities=newentities}, []

            | MainMenu x, Update (time, depends) ->
                let {Input=input;} = depends
                let newui, uimsg = UpdateUI x update input
                MainMenu newui, (uimsg |> List.map (fun x -> x depends))
            | MainMenu x, Draw {Graphics=graphics; Camera=cam; Sprite=sprite; UIDependencies=uideps} ->
                cam.Position <- new Vector2(float32 0)
                sprite.Begin(cam)
                DrawUI x sprite uideps
                sprite.End()
                state, []
            | _ ->
                state, []

    state <- newstate
    dispatch |> List.iter update