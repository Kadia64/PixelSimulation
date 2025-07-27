using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Behavior.Dusts;
using Pixel_Simulation.Pixel_Behavior.Solids;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Properties;


namespace Pixel_Simulation.Menu.Pixel_Selection {
    public static class PixelActions {
        public static Color ChoosePixelColor(ElementType element) {
            Color[] colors = (Color[])PixelFactory.MapColorSchemes(element, $"{element}Colors");
            return colors[RandomEngine.GetNumber(colors.Length)];
        }
        public static void CreatePixel(int x, int y, Type pixelType, Pixel[,] grid) {
            var coords = Screen.ConvertGridToScreenPosition(x, y);
            grid[x, y] = (Pixel)Activator.CreateInstance(pixelType, coords.X, coords.Y);
        }
        public static void ClearAllPixels(Pixel[,] pixels) {
            for (int y = 0; y < Screen.GridHeight; y++) {
                for (int x = 0; x < Screen.GridWidth; x++) {
                    pixels[x, y] = null;
                }
            }
            Screen.PixelIDCounter = 0;
        }
        public static void ApplyPhysics() {
            Globals.PhysicsEnabled = true;
        }
        public static void DrawRandomBox(Pixel[,] pixels) {
            int sizeX = 150;
            int sizeY = 200;
            int startX = 150;
            int startY = 40;

            for (int x = 0; x < Screen.GridWidth; x++) {
                for (int y = 0; y < Screen.GridHeight; y++) {

                    if (x > startX && x < startX + sizeX && y > startY && y < startY + sizeY) {
                        var newCoords = Screen.ConvertGridToScreenPosition(x, y);
                        pixels[x, y] = new Water(newCoords.X, newCoords.Y);
                    }
                }
            }
        }
        public static void DrawInitialPixels(Pixel[,] grid) {
            for (int x = 0; x < Screen.GridWidth; x++) {
                for (int y = 0; y < Screen.GridHeight; y++) {
                    if (y == 0 || y == 1 || y == 2) {
                        PixelFactory.AddPixelToGrid(x, y, grid, ElementType.Stone);
                    }
                }
            }
        }
    }
}
