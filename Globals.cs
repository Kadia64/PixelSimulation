using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.View;
using Pixel_Simulation.Input;

namespace Pixel_Simulation {
    public static class Globals {

        public static SpriteBatch _spriteBatch;
        public static SpriteFont _debugFont;
        public static GraphicsDevice _graphicsDevice;
        public static MouseUserInput _mouse;
        public static Shapes _shapes;
        public static Camera _camera;

        public static Texture2D _reusablePixel;
        public static Texture2D _worldPixel;
        public static Texture2D _tempColorPixel;

        public static float DebugFontSize = 8.0f;

        public static int _FRAME_COUNTER_ = 0;

        public static bool PhysicsEnabled = true;

        public static float[,] TemperatureMap;
        public static float[,] InitialTemperatureMap;
        public static float GlobalTemperature = 65.0f;
        public static float TemperatureFluctuation = 6.0f;

        public static int GasUpdateFrequency = 2;  // update gas particles every 2 frames

        // Previous keyboard state for camera input
        public static KeyboardState _previousKeyboardState;
    }
    public static class Screen {
        // wdith of the menu to the right
        public static int MenuWidth = 250;

        public static int ScreenWidth = 2100;
        public static int ScreenHeight = 950;

        public static int PixelWidth = 2;
        public static int PixelHeight = 2;

        public static int StartMapPositionX = 0;
        public static int StartMapPositionY = 0;

        public static int GridWidth = 700;
        public static int GridHeight = 400;

        public static int PixelIDCounter = 0;


        // converts a 2d array position to a position on the screen (world coordinates)
        public static Vector2i ConvertGridToScreenPosition(int x, int y) {
            return new Vector2i(StartMapPositionX + x * PixelWidth, StartMapPositionY + y * PixelHeight);
        }

        // converts the screen position to a xy coordinate on the 2d array
        public static Vector2i ConvertScreenToGridPosition(int x, int y) {
            return new Vector2i((x - StartMapPositionX) / PixelWidth, (y - StartMapPositionY) / PixelHeight);
        }

        // converts the mouse position on the screen to a xy coordinate on the 2d array (camera-aware)
        public static Vector2 MousePositionToPixelPosition() {
            var mousePosition = Globals._mouse.GetMousePosition();
            
            // If camera exists, transform mouse position to world coordinates
            if (Globals._camera != null) {
                var worldPosition = Globals._camera.ScreenToWorld(mousePosition);
                return new Vector2(
                    (int)((worldPosition.X - StartMapPositionX) / PixelWidth),
                    (int)((worldPosition.Y - StartMapPositionY) / PixelHeight)
                );
            }
            
            // Fallback to old behavior if no camera
            return new Vector2(
                (int)((mousePosition.X - StartMapPositionX) / PixelWidth),
                (int)((mousePosition.Y - StartMapPositionY) / PixelHeight)
            );
        }
        
        // Get the visible area of the grid based on camera position (for culling)
        public static Rectangle GetVisibleGridArea() {
            if (Globals._camera == null) {
                return new Rectangle(0, 0, GridWidth, GridHeight);
            }
            
            var viewRect = Globals._camera.GetViewRectangle();
            
            // Convert world coordinates to grid coordinates
            int startX = Math.Max(0, (viewRect.X - StartMapPositionX) / PixelWidth - 1);
            int startY = Math.Max(0, (viewRect.Y - StartMapPositionY) / PixelHeight - 1);
            int endX = Math.Min(GridWidth - 1, (viewRect.Right - StartMapPositionX) / PixelWidth + 1);
            int endY = Math.Min(GridHeight - 1, (viewRect.Bottom - StartMapPositionY) / PixelHeight + 1);
            
            return new Rectangle(startX, startY, endX - startX + 1, endY - startY + 1);
        }
    }
}
