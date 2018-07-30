module Game.Entity

open Microsoft.Xna.Framework.Graphics
open Entity.Player
open MonoGame.Spritesheet
open Microsoft.Xna.Framework
open MonoGame.Spritesheet

type Entity = Player of Player

let defaultEntities c = [defaultPlayer c |> Player]

let UpdateEntities frame =
    List.map (function | Player y -> UpdatePlayer frame y |> Player)

let DrawEntities spriteBatch =
    List.iter (function | Player y -> DrawPlayer spriteBatch y)

let MapPlayer x = List.map (function | Player pl -> x pl |> Player | x -> x)
let GetPlayer = List.choose (function | Player pl -> Some(pl) | _ -> None) >> List.head