module Entity.Player

open Game.Object
open Microsoft.Xna.Framework.Graphics
open MonoGame.Spritesheet
open Utility.Animation
open Microsoft.Xna.Framework
open System

type PlayerAnimation = Run of int*WASDDir | Idle of int
type Player = {Mouth: Item option}

let updateAnimation f = function
    | Run (x,y) -> Run (x+f%4, y)
    | Idle x -> Idle (0)

let drawPlayer (sprites:SpriteBatch) (sheet:GridSheet) (position:Vector2) (anim:PlayerAnimation) =
    let (y,x) = match anim with
                    | Run (x,Forward) -> 3,x
                    | Run (x,Backward) -> 1,x
                    | Run (x,Right) -> 2,x
                    | Run (x,Left) -> 4,x

    sprites.Draw (texture=sheet.Texture, position=position, color=Color.White, sourceRectangle=Nullable(sheet.[x,y]))