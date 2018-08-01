module Game.Update

open Game
open System
open Model
open Environment.TileSelector
open GameEnvironment.Model
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
                ActiveGame {Entities=defaultEntities content; GameEnvironment=defaultenv;}, []
            | MainMenu (_,Some x), IntoGame {Graphics=graphics; Content=content} -> ActiveGame x,[]

            | ActiveGame x, Draw {TileSet=tileset; Sprite=sprite; Graphics=graphics; Camera=cam} ->
                let {GameEnvironment=env; Entities=entities} = x
                sprite.Begin(cam, SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp)

                RenderTiles graphics env tileset sprite
                DrawEntities sprite entities

                sprite.End()
                state, []
            | ActiveGame x, Update (time, {Input=i; Graphics=graphics; Camera=cam}) ->
                let {Entities=entities; GameEnvironment=env} = x
                let player = GetPlayer entities
                cam.Position <- player.Position

                let newenv = UpdateTiles graphics cam env
                let newentities = entities |> UpdateEntities (time.TotalGameTime.TotalSeconds*6.0 |> int)
                ActiveGame {x with Entities=newentities; GameEnvironment=newenv}, Input.InputMsgs i time.ElapsedGameTime
            | ActiveGame x, PlayerMove y ->
                let {Entities=entities; GameEnvironment=gameenv} = x
                let newentities = MapPlayer (Player.MovePlayer y gameenv) entities
                ActiveGame {x with Entities=newentities}, []

            | ActiveGame x, ToMainMenu ->
                let game = Some x
                MainMenu (UI.Menu.menuui game, game), []
            | MainMenu (_, game), ToMainMenu ->
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

            | MainMenu (_, game), ToCredits _ ->
                MainMenu (UI.Menu.credits, game), []
            | _, Quit {Game=game} ->
                Exit, []
            | _ -> state, []

    state <- newstate
    dispatch |> List.iter update