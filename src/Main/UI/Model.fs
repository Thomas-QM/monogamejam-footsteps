module UI.Model
open Microsoft.Xna.Framework
open InputStateManager
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework
open MonoGame.Extended.BitmapFonts
open Utility.SpriteBatch
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Input

type Button<'C, 'T> = {Text:string; Hover:bool; OnClick: 'C -> 'T;}
type Control<'C, 'T> = Button of Button<'C, 'T> | Text of string*Color
type PositionedControl<'C, 'T> = {Bounds:Rectangle; Padding:int; Control:Control<'C, 'T>}
type UI<'C, 'T> = PositionedControl<'C, 'T> list

type Font = Typograph

type ButtonTexture = Normal | Hover
type UIDependencies = {Fonts:Map<Font, BitmapFont>; ButtonTextures:Map<ButtonTexture, Texture2D>}

let ButtonTextures = [Normal; Hover]

let MakeControl (x,y) width height pad control =
    let bounds = new Rectangle (x, y, width, height)
    {Bounds=bounds; Padding=pad; Control=control}

let MakeButton text onclick = {Text=text; OnClick=onclick; Hover=false} |> Button

let MouseInBounds (input:InputManager) (bounds:Rectangle) =
    bounds.Contains(input.Mouse.Is.Position.X, input.Mouse.Is.Position.Y)

let UpdateUI (ui:UI<_,_>) (dispatch:'T -> unit) (input:InputManager) =
    let clicking = input.Mouse.Was.Down(Inputs.Mouse.Button.LEFT)
    ui |> List.fold (fun (controls, msgs) x ->
                        let {Bounds=bounds; Control=control} = x
                        let newcontrol, newmsgs =
                            match control with
                                | Button {OnClick=y} when clicking && MouseInBounds input bounds ->
                                    control, [y]
                                | Button x when MouseInBounds input bounds -> {x with Hover=true} |> Button, []
                                | Button x -> {x with Hover=false} |> Button, []
                                | x -> x, []
                        {x with Control=newcontrol}::controls, newmsgs@msgs
                        ) ([], [])

let DrawUI (ui:UI<_,_>) (sprite:SpriteBatch) {Fonts=fonts; ButtonTextures=buttextures;} =
    let typograph = Map.find Typograph fonts
    ui |> List.iter (function
        | {Bounds=bounds; Padding=pad; Control=Button {Hover=hov; Text=text}} ->
            let foreground = if hov then Color.White else Color.Black
            let background = Map.find (if hov then Hover else Normal) buttextures
            sprite.Draw (background, bounds, Color.White)
            let unpadded = new Rectangle (bounds.X-pad, bounds.Y-pad, bounds.Width-pad, bounds.Height-pad)
            DrawStringRect sprite typograph text unpadded foreground
        | {Bounds=bounds; Control=Text (x,col)} ->
            DrawStringRect sprite typograph x bounds col
    )