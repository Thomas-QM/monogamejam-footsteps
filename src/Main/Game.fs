module FSGame

open Game.Model
open Microsoft.Xna.Framework
open Game.Update
open Microsoft.Xna.Framework.Graphics
open Environment.Model

type MainGame () as x =
    inherit Game()

    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable (tileSet:TileSet) = Map.empty
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    let toDependencies () = {Content=x.Content; Graphics=graphics; TileSet=tileSet; Sprite=spriteBatch}

    override x.Initialize() =
        do spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        do base.Initialize()
        x.IsMouseVisible <- true
        x.Window.AllowUserResizing <- true
        x.Window.ClientSizeChanged.Add (fun _ -> graphics.PreferredBackBufferWidth <- x.Window.ClientBounds.Width;
                                                    graphics.PreferredBackBufferHeight <- x.Window.ClientBounds.Height;
                                                    printfn "NEW RES: %A" x.Window.ClientBounds;
                                                    graphics.ApplyChanges())

         // TODO: Add your initialization logic here

        ()

    override x.LoadContent() =

        tileSet <- tiles |> List.map (fun y -> (y, getTile x.Content y)) |> Map.ofList
        do toDependencies() |> Game.Model.IntoGame |> update
         // TODO: use this.Content to load your game content here

        ()

    override x.Update (gameTime) =
        (gameTime.ElapsedGameTime, toDependencies()) |> Update |> update

        ()

    override x.Draw (gameTime) =
        do x.GraphicsDevice.Clear Color.Black
        toDependencies() |> Draw |> update

        ()