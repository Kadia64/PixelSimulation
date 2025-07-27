using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Pixel_Simulation;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Menu.MenuComponents;

namespace Pixel_Simulation.Menu {
    
    public static class MenuManager {

        public static AddPixels _addPixels;

        public static int MenuOutlineThickness = 2;
        public static Rectangle MenuDimensions = new Rectangle(Screen.ScreenWidth - Screen.MenuWidth, 0, Screen.MenuWidth - MenuOutlineThickness, Screen.ScreenHeight - MenuOutlineThickness);        

        public static void DrawMenuContent() {
            Globals._shapes.DrawBoxOutline(MenuDimensions, Color.Blue, MenuOutlineThickness);
            Globals._shapes.FillOutline(MenuDimensions, Color.BlueViolet, MenuOutlineThickness);            

            float textScale = 1.2f;
            Globals._spriteBatch.DrawString(Globals._debugFont, "Elements:", new Vector2(MenuDimensions.X + 20, 10), Color.White, 0f, new Vector2(0, 0), textScale, SpriteEffects.None, 0f);
            Globals._spriteBatch.DrawString(Globals._debugFont, "Brush Sizes:", new Vector2(MenuDimensions.X + 20, 315), Color.White, 0f, new Vector2(0, 0), textScale, SpriteEffects.None, 0f);
        }
        public static void DrawGrid(bool enabled) {
            if (enabled) {
                int columns = Screen.GridWidth;
                int rows = Screen.GridHeight;

                int lengthX = Screen.GridWidth * Screen.PixelWidth;
                int lengthY = Screen.GridHeight * Screen.PixelHeight;
                int startX = Screen.StartMapPositionX;
                int startY = Screen.StartMapPositionY;

                int countX = 0;
                for (int x = 0; x < rows + 1; ++x) {
                    Globals._shapes.DrawHorizontalLine(new Point(startX, startY + countX), new Point(startX + lengthX, startY + countX), Color.Black);
                    countX += Screen.PixelHeight;
                }

                for (int y = 0; y < columns + 1; ++y) {
                    Globals._shapes.DrawVerticalLine(new Point(startX, startY), new Point(startX, startY + lengthY), Color.Black);
                    startX += Screen.PixelWidth;
                }
            }
        }
    }
   
    public enum PaintScale {
        Tiny,
        Small,
        Medium,
        Large
    }
    public enum MenuSwitch {
        Forward, 
        Back
    }
}
