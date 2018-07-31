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

let mutable state = MainMenu (UI.Menu.menuui None, None)

let rec update msg =
    let newstate, dispatch =
        match state, msg with
            | MainMenu (_,None), IntoGame {Graphics=graphics; Content=content} ->
                ActiveGame {Entities=defaultEntities content; Environment=defaultenv;}, []
            | MainMenu (_,Some x), IntoGame {Graphics=graphics; Content=content} -> ActiveGame x,[]

            | ActiveGame x, Draw {TileSet=tileset; Sprite=sprite; Graphics=graphics; Camera=cam} ->
                let {Environment=env; Entities=entities} = x
                sprite.Begin(cam, SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp)

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

            | ActiveGame x, ToMainMenu ->
                let game = Some x
                MainMenu (UI.Menu.menuui game, game), []
            | MainMenu (x, game), Update (time, depends) ->
                let {Input=input; Camera=cam; Graphics=graphics} = depends
                let newui, uimsg = UpdateUI x update input graphics cam
                MainMenu (newui, game), (uimsg |> List.map (fun x -> x depends))
            | MainMenu (x, _), Draw {Graphics=graphics; Camera=cam; Sprite=sprite; UIDependencies=uideps} ->
                cam.Position <- Vector2(float32 0) + Vector2(cam.Width/float32 2, cam.Height/float32 2) //TODO: make util function for this
                sprite.Begin(cam, SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp)
                DrawUI x sprite uideps
                sprite.End()
                state, []
            | _, Quit {Game=game} ->
                game.Exit()
                Exit, []
            | _ -> state, []

    state <- newstate
    dispatch |> List.iter update