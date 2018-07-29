module Game.Entity

open Microsoft.Xna.Framework.Graphics
open Entity.Player
open MonoGame.Spritesheet
open Microsoft.Xna.Framework
open MonoGame.Spritesheet

type EntityData = Player of Player*PlayerAnimation
type Entity = {Entity: EntityData; Position: Vector2}


let UpdateEntity (sprites:SpriteBatch) elapsed deps x =
    let {sheets}