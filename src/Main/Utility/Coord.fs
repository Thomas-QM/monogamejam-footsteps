module Utility.Coord
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework
open Comora

let ConvertWindow (graphics:GraphicsDeviceManager) (cam:ICamera) (coord:Vector2) =
    let lerpvec = coord/Vector2(float32 graphics.PreferredBackBufferWidth, float32 graphics.PreferredBackBufferHeight)
    lerpvec*Vector2(cam.Width, cam.Height)