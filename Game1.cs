using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Win8ShooterGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        //Represents the player
        Player player;

        //Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        //Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        //Mouse states used to track Mouse button press
        MouseState currentMouseState;
        MouseState previousMouseState;

        //A movement speed for the player
        float playerMoveSpeed;

        // Image used to display the static background
        Texture2D mainBackground;
        Rectangle rectBackground;

        float scale = 1f;
        private ParallaxingBackground bgLayer1;
        private ParallaxingBackground bgLayer2;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initialize the player class
            player = new Player();

            //Background
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            //Set a constant player move speed
            playerMoveSpeed = 8.0f;

            //Enable the FreeDrag gesture
            TouchPanel.EnabledGestures = GestureType.FreeDrag;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Load the player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("Graphics\\shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            player.Initialize(playerAnimation, playerPosition);

            // Load the parallaxing background
            bgLayer1.Initialize(Content, "Graphics/bgLayer1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);
            bgLayer2.Initialize(Content, "Graphics/bgLayer2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);

            mainBackground = Content.Load<Texture2D>("Graphics/mainbackground");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Save the previous state of the keyboard and gamepad so we can determine single key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            //Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentMouseState = Mouse.GetState();

            //Update the player
            UpdatePlayer(gameTime);

            // Update the parallaxing background
            bgLayer1.Update(gameTime);
            bgLayer2.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {

            player.Update(gameTime);

            // Windows 8 Touch Gestures for MonoGame
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.FreeDrag)
                {
                    player.Position += gesture.Delta;
                }
            }

            //Get Mouse State then Capture the Button type and Respond Button Press
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 posDelta = mousePosition - player.Position;
                posDelta.Normalize();
                posDelta = posDelta * playerMoveSpeed;
                player.Position = player.Position + posDelta;
            }

            //Get Thumbstick Controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            //Use the keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) || currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) || currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMoveSpeed;
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Start drawing
            _spriteBatch.Begin();

            //Draw the Main Background Texture
            _spriteBatch.Draw(mainBackground, rectBackground, Color.White);

            // Draw the moving background
            bgLayer1.Draw(_spriteBatch);
            bgLayer2.Draw(_spriteBatch);

            // Draw the Player
            player.Draw(_spriteBatch);

            // Stop drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
