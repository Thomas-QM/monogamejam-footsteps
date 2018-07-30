module Entity.Player

open Game.Object
open Utility.Animation
open Model
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework

type PlayerAnimation = Run of int*WASDDir | Idle of WASDDir
type Player = {Mouth: Item option; Animated:Animated<PlayerAnimation>; Position: Vector2}

let UpdateAnimation f = function
    | Run (x,y) -> Run (x+f%3, y)
    | x -> x

let UpdatePlayer f player =
    let {Animated=x,y,z} = player
    let newanimation = UpdateAnimation f x
    {player with Animated=newanimation,y,z}

let DrawPlayer (sprites:SpriteBatch) {Mouth=mouth; Animated=(anim,tex,sheet); Position=position} =
    let dir = match anim with | Run (_, dir) | Idle dir -> dir
    let frame = match anim with | Run (x, _) -> x | Idle _ -> 0

    let str = "dog" + dir.ToString() + FrameNum(frame)

    sprites.Draw (texture=sheet, position=position, color=Color.White, sourceRectangle=System.Nullable(tex.Item(str)))

let defaultPlayer contentmanager = {Mouth=None; Animated=ConstructAnimated contentmanager "Dog" (Idle Forward); Position=new Vector2(float32 0)}