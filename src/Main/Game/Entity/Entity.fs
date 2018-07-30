module Game.Entity

open Microsoft.Xna.Framework.Graphics
open Entity.Player
open MonoGame.Spritesheet
open Microsoft.Xna.Framework
open MonoGame.Spritesheet

type Entity = Player of Player

let InitializeEntities c = [defaultPlayer c]

let UpdateEntities frame =
    List.map (function | Player y -> UpdatePlayer frame y |> Player)