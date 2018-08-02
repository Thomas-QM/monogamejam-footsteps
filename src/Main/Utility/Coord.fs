module Utility.Coord
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework
open Comora

let ConvertWindow (graphics:GraphicsDeviceManager) (cam:ICamera) (coord:Vector2) =
    let lerpvec = coord/Vector2(float32 graphics.PreferredBackBufferWidth, float32 graphics.PreferredBackBufferHeight)
    lerpvec*Vector2(cam.Width, cam.Height)

let CartesianToIsometric(cart:Vector2) = Vector2(cart.X - cart.Y, (cart.X + cart.Y) / float32 2)

let IsometricToCartesian(iso:Vector2) = Vector2((float32 2 * iso.Y + iso.X) / float32 2, (float32 2 * iso.Y - iso.Y) / float32 2)
let CamPos (cam:ICamera) =
    cam.Position - Vector2(cam.Width/float32 2, cam.Height/float32 2)

let GetTileCoordinates (pos:Vector2) (relative:Vector2) tilewidth =
    let newvec = pos- relative |> CartesianToIsometric
    let offx = relative.X
    let offy = relative.Y
    let ftw = float32 2
    Vector2((newvec.X)/ftw |> floor |> (+) offx,
                (newvec.Y)/ftw |> floor |> (+) offy)
