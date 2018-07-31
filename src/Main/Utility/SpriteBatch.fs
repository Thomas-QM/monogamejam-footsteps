module Utility.SpriteBatch
open MonoGame.Extended.BitmapFonts
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Utility

let DrawStringRect (spriteBatch:SpriteBatch) (font:BitmapFont) (str:string) (boundaries:Rectangle) (color:Color) =
    //500x300
    let size = font.MeasureString(str) //100x20
    let xscale = float32 boundaries.Width / size.Width //5
    let yscale = float32 boundaries.Height / size.Height //15
    let scale = min xscale yscale //5
    let strwidth = size.Width * scale |> int
    let strheight = size.Height * scale |> int
    let pos = Vector2(((boundaries.Width-strwidth)/2)+boundaries.X |> float32, ((boundaries.Height-strheight)/2)+boundaries.Y |> float32)

    let rotation = 0.0 |> float32
    let origin = Vector2(float32 0, float32 0)
    let layer = 0.0 |> float32
    let effects = new SpriteEffects();

    spriteBatch.DrawString (font, str, pos, color, rotation, origin, scale, effects, layer)
    // float xScale = (boundaries.Width / size.X);
    // float yScale = (boundaries.Height / size.Y);

    // // Taking the smaller scaling value will result in the text always fitting in the boundaires.
    // float scale = Math.Min(xScale, yScale);

    // // Figure out the location to absolutely-center it in the boundaries rectangle.
    // int strWidth = (int)Math.Round(size.X * scale);
    // int strHeight = (int)Math.Round(size.Y * scale);
    // Vector2 position = Vector2();
    // position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
    // position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);

    // // A bunch of settings where we just want to use reasonable defaults.
    // float rotation = 0.0f;
    // Vector2 spriteOrigin = Vector2(0, 0);
    // float spriteLayer = 0.0f; // all the way in the front
    // SpriteEffects spriteEffects = new SpriteEffects();

    // // Draw the string to the sprite batch!
    // spriteBatch.DrawString(font, strToDraw, position, Color.White, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);