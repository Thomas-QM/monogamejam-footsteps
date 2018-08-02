module FSGame

open Game.Model
open Microsoft.Xna.Framework
open Game.Update
open Microsoft.Xna.Framework.Graphics
open GameEnvironment.Model
open InputStateManager
open MonoGame.Extended.ViewportAdapters
open UI.Model
open MonoGame.Extended.BitmapFonts
open Comora

type MainGame () as x =
    inherit Game()

    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable (tileSet:TileSet) = Map.empty
    let mutable input = new InputManager()
    let mutable camera = Unchecked.defaultof<ICamera>
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable emptytex = Unchecked.defaultof<Texture2D>
    let mutable uideps = {Fonts=Map.empty; ButtonTextures=Map.empty}

    let toDependencies () = {Camera=camera; Game=x; UIDependencies=uideps; Content=x.Content; Graphics=graphics; TileSet=tileSet; Sprite=spriteBatch; Input=input}

    override x.Initialize() =
        camera <- new Camera(x.GraphicsDevice)
        camera.Width <- float32 800
        camera.Height <- float32 480
        camera.ResizeMode <- AspectMode.FillUniform

        do spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        do base.Initialize()
        x.IsMouseVisible <- true
        x.Window.AllowUserResizing <- true
        x.Window.ClientSizeChanged.Add (fun _ ->
                                            graphics.PreferredBackBufferWidth <- x.Window.ClientBounds.Width
                                            graphics.PreferredBackBufferHeight <- x.Window.ClientBounds.Height
                                            Resize (graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight) |> update
                                            graphics.ApplyChanges())

        x.Exiting.Add (fun _ -> state <- Exit)


        // TODO: Add your initialization logic here

        ()

    override x.LoadContent() =
        camera.LoadContent()
        uideps <- {
            Fonts=[Typograph, x.Content.Load<BitmapFont> ("Fonts/Typograph")] |> Map.ofList
            ButtonTextures=ButtonTextures |> List.map (fun y -> (y, y.ToString() |> sprintf "ButtonTextures/%s" |> x.Content.Load)) |> Map.ofList
        }

        tileSet <- tiles |> List.map (fun (_,y) -> (y, getTile x.Content y)) |> Map.ofList
        //do toDependencies() |> Game.Model.IntoGame |> update
        // TODO: use this.Content to load your game content here

        ()

    override x.Update (gameTime) =
        match state with
            | Exit -> x.Exit()
            | _ ->
                camera.Update (gameTime)
                input.Update ()
                (gameTime, toDependencies()) |> Update |> update


        ()

    override x.Draw (gameTime) =
        do x.GraphicsDevice.Clear Color.DeepSkyBlue
        toDependencies() |> Draw |> update

        ()