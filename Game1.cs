using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.View;
using Pixel_Simulation.Climate;
using Pixel_Simulation.Menu;
using Pixel_Simulation.Menu.MenuComponents;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Input;
using Pixel_Structure.Performance;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _debugFont;
        private ColorSchemeImporter _colorSchemeManager;

        private Pixel[,] grid;
        private PixelUpdater _pixelUpdater;                
        private Color BackGroundColor = Color.Black;

        private bool _PHYSICS_ENABLED_ = true;
        private FPSMeter _fpsCounter;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("Fonts/DebugFont");

            _graphics.IsFullScreen = false;
            Screen.ScreenWidth += Screen.MenuWidth;
            _graphics.PreferredBackBufferWidth = Screen.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Screen.ScreenHeight;
            _graphics.ApplyChanges();

            Globals._spriteBatch = _spriteBatch;
            Globals._debugFont = _debugFont;
            Globals._graphicsDevice = GraphicsDevice;
            Globals._mouse = new MouseUserInput();
            Globals._shapes = new Shapes();
            MenuManager._addPixels = new AddPixels();
            _pixelUpdater = new PixelUpdater();  
            _colorSchemeManager = new ColorSchemeImporter("Config\\color-schemes.json");

            // Initialize camera (viewport size excludes the menu area)
            int cameraViewportWidth = Screen.ScreenWidth - Screen.MenuWidth;
            int cameraViewportHeight = Screen.ScreenHeight;
            Globals._camera = new Camera(cameraViewportWidth, cameraViewportHeight);
            
            // No world bounds - unlimited camera movement for testing
            Globals._camera.WorldBounds = null;

            // Initialize previous keyboard state
            Globals._previousKeyboardState = Keyboard.GetState();

            Globals.TemperatureMap = new float[Screen.GridWidth, Screen.GridHeight];
            Globals.InitialTemperatureMap = new float[Screen.GridWidth, Screen.GridHeight];
            grid = new Pixel[Screen.GridWidth, Screen.GridHeight];
            Globals.PhysicsEnabled = _PHYSICS_ENABLED_;
            
            // Initialize FPS counter
            _fpsCounter = new FPSMeter();
            
            //_debugTimer = new Timer(1.0f); // 1 second timer

            base.Initialize();
        }

        protected override void LoadContent() {
            Globals._reusablePixel = new Texture2D(Globals._graphicsDevice, 1, 1);
            Globals._reusablePixel.SetData<Color>(new Color[] { Color.White });
            Globals._worldPixel = new Texture2D(Globals._graphicsDevice, 1, 1);
            Globals._worldPixel.SetData<Color>(new Color[] { Color.White });

            _colorSchemeManager.ImportDustColorSchemes();
            _colorSchemeManager.ImportLiquidColorSchemes();
            _colorSchemeManager.ImportGasColorSchemes();
            _colorSchemeManager.ImportSolidColorSchemes();

            TempColorsMapper.BuildTable();
            TempColorsMapper.PreCalculateCircleOffsets(TempColorsMapper.Radius);
            PixelSelectionMenu.CreateButtons();

            for (int x = 0; x < Screen.GridWidth; x++) {
                for (int y = 0; y < Screen.GridHeight; y++) {
                    grid[x, y] = null;                    

                    float temperature = TempColorsMapper.FluctuateTemperature();
                    Globals.InitialTemperatureMap[x, y] = temperature;
                    Globals.TemperatureMap[x, y] = temperature;

                }
            }
            PixelActions.DrawInitialPixels(grid);

            //Exit();
        }

        protected override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape)) Exit();
            
            // Update timers and FPS counter
            //_timer.Update(gameTime);
            _fpsCounter.Update(gameTime);
            
            // Check if event timer reached its target
            //if (_eventTimer.HasReachedTarget) {
            //    _eventTimer.Reset(); // Reset for next use
            //}

            // Update camera with WASD controls
            Globals._camera.Update(gameTime, currentKeyboardState, Globals._previousKeyboardState);

            Globals._mouse.Update();
            Globals._mouse.InitializeMouseStates();
            MenuManager._addPixels.UpdateMousePosition();

            // Update Pixel Positions
            _pixelUpdater.PixelUpdateDirections(grid, deltaTime);

            MenuManager._addPixels.UpdateHighlightedTile();
            PixelSelectionMenu.CheckButtonBoundaries(grid);
            Globals._mouse.UpdateOld();

            // Store current keyboard state for next frame
            Globals._previousKeyboardState = currentKeyboardState;

            ++Globals._FRAME_COUNTER_;
            if (Globals._FRAME_COUNTER_ >= 3000) Globals._FRAME_COUNTER_ = 0;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(BackGroundColor);
            
            // Set up viewport to exclude menu area for camera
            var viewport = GraphicsDevice.Viewport;
            var gameViewport = new Viewport(0, 0, Screen.ScreenWidth - Screen.MenuWidth, Screen.ScreenHeight);
            GraphicsDevice.Viewport = gameViewport;
            
            // Begin sprite batch with camera transformation for world objects
            _spriteBatch.Begin(transformMatrix: Globals._camera.GetTransformMatrix());
            var visibleArea = Screen.GetVisibleGridArea();
            
            //MenuManager._addPixels.DrawHighlightedTile();
            MenuManager._addPixels.DrawPixelAtPosition(_spriteBatch, grid);
            
            // Example 1: World space - Grid boundary that moves with camera
            int gridWorldX = Screen.StartMapPositionX - Screen.PixelWidth;
            int gridWorldY = Screen.StartMapPositionY - Screen.PixelHeight;
            int gridWidth = (Screen.GridWidth * Screen.PixelWidth) + Screen.PixelWidth;
            int gridHeight = (Screen.GridHeight * Screen.PixelHeight) + Screen.PixelHeight;
            Globals._shapes.DrawBoxOutline(gridWorldX, gridWorldY, gridWidth, gridHeight, Color.White, CoordinateSystem.World, thickness: 2);

            // Only draw pixels that are visible
            
            for (int x = visibleArea.X; x < visibleArea.X + visibleArea.Width; ++x) {
                for (int y = visibleArea.Y; y < visibleArea.Y + visibleArea.Height; ++y) {
                    if (x >= 0 && x < Screen.GridWidth && y >= 0 && y < Screen.GridHeight && grid[x, y] != null) {
                        grid[x, y].Draw();
                    }
                }
            }
            //TempColorsMapper.DrawTemperatureMap(grid);

            _spriteBatch.End();
            
            // Restore full viewport for UI elements
            GraphicsDevice.Viewport = viewport;
            
            // Begin sprite batch without camera transformation for UI (menu stays in place)
            _spriteBatch.Begin();

            MenuManager.DrawMenuContent();
            PixelSelectionMenu.DrawButtons();
            MenuManager.DrawGrid(false);

            // Display FPS counter
            //_fpsCounter.Draw(_spriteBatch, _debugFont, new Vector2(10, 100), Color.White);
            _fpsCounter.DrawFps(_spriteBatch, _debugFont, new Vector2(10, 100), Color.White);

            //Debug.Write($"{PixelSelectionMenu.ElementMenuIndex}", 10, 10);

            // Display timer information
            //Debug.Write($"Performance Timer: {_performanceTimer.GetFormattedTime()}", 10, 30);
            //Debug.Write($"Event Timer: {_eventTimer.GetFormattedTime()}/{_eventTimer.TargetTime:F1}s", 10, 50);
            //Debug.Write($"Timer Controls: T=Start, R=Stop, E=Event", 10, 70);

            TempColorsMapper.DrawThermometer();


            // Example 2: Screen space - Static UI elements that don't move with camera
            // These should stay in fixed screen positions
            //Globals._shapes.DrawBoxOutline(new Rectangle(10, 10, 200, 100), Color.Red, thickness: 3);
            //Globals._shapes.DrawBoxOutline(new Rectangle(10, 120, 150, 80), Color.Green, thickness: 2);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
