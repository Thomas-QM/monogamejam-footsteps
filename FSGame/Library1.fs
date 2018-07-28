namespace FSGame

namespace FSGame

open Microsoft.Xna.Framework

module Game =
    open Microsoft.Xna.Framework.Graphics

    type MainGame () as this =
        inherit Game ()
        let graphics = new GraphicsDeviceManager(this)
        do this.Initialize()

        override x.LoadContent() =
            //spriteBatch = new SpriteBatch(GraphicsDevice)
            ()

        override x.UnloadContent() = ()

        override x.Update(gameTime:GameTime) =
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

        override x.Draw(gameTime:GameTime) =
            
            graphics.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);