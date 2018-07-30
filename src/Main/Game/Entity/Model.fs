module Entity.Model
open Microsoft.Xna.Framework.Content
open MonoGame.Extended.TextureAtlases
open System.Collections.Generic
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Animated = Dictionary<string,Rectangle>*Texture2D

let rec FrameNumStr x =
    if String.length x < 3 then FrameNumStr("0"+x) else x

let FrameNum x = x.ToString() |> FrameNumStr

let ConstructAnimated (contentmanager:ContentManager) name :Animated =
    contentmanager.Load(sprintf "Sprites\\%sData" name), contentmanager.Load(sprintf "Sprites\\%s" name)