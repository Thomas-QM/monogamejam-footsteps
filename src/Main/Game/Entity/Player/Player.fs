module Entity.Player

open Game.Object
open Utility.Animation
open Model
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework
open GameEnvironment
open GameEnvironment.Model

type PlayerAnimation = Run of int*WASDDir | Idle of WASDDir
type Player = {Mouth: Item option; Animated:Animated; Animation:PlayerAnimation; Position: Vector2}

let UpdateAnimation f = function
    | Run (_,y) -> Run (f%3, y)
    | x -> x

let UpdatePlayer f player =
    let {Animation=x} = player
    let newanimation = UpdateAnimation f x
    {player with Animation=newanimation}

let DrawPlayer (sprites:SpriteBatch) {Mouth=mouth; Animation=anim; Animated=(tex,sheet); Position=position} =
    let dir = match anim with | Run (_, dir) | Idle dir -> dir
    let frame = match anim with | Run (x, _) -> x | Idle _ -> 0

    let str = "dog" + dir.ToString() + FrameNum(frame)

    let rect = new Rectangle(int position.X, int position.Y, 50, 50)
    sprites.Draw (texture=sheet, color=Color.White, sourceRectangle=System.Nullable(tex.Item(str)), destinationRectangle=rect)

let MovePlayer (dir:Vector2) ({RenderTiles=tiles}) player =
    let spritedir =
        match (float dir.X, float dir.Y) with
            | 0.0,0.0 -> None
            | x,y when abs(x) >= abs(y) -> (if x < 0.0 then Left else Right) |> Some
            | x,y when abs(y) >= abs(x) -> (if y > 0.0 then Backward else Forward) |> Some
            | _ -> None

    let olddir = match player.Animation with | Run (_, dir) | Idle dir -> dir
    let frame = match player.Animation with | Run (f, _) -> f | Idle _ -> 0

    let newanim = match spritedir with | Some dir -> Run (frame, dir) | _ -> Idle olddir

    let newpos = player.Position+dir
    let colliding = tiles |> List.filter (function | {Collision=Block} -> true | _ -> false) |> List.exists (fun {Region=x} -> x.Contains (newpos))
    let newerpos = if colliding then player.Position else newpos //smart naming
    { player with Position = newerpos; Animation=newanim }

let defaultPlayer contentmanager = {Mouth=None; Animated=ConstructAnimated contentmanager "Dog"; Position=Vector2(float32 0); Animation=Idle Forward}