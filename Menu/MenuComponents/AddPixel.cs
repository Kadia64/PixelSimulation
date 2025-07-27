using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation;
using Pixel_Simulation.Menu;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Properties;

namespace Pixel_Simulation.Menu.MenuComponents {
    public class AddPixels {

        private Texture2D highlightPixel;
        private Rectangle highlightRect;
        private Vector2 MousePosition;
        private int DrawingScaleFactor = 2;

        public AddPixels() {
            highlightPixel = new Texture2D(Globals._graphicsDevice, 1, 1);
            highlightPixel.SetData(new Color[] { Color.White });
            highlightRect = new Rectangle(0, 0, Screen.PixelWidth, Screen.PixelHeight);
        }


        public void UpdateHighlightedTile() {
            switch (PixelSelectionMenu.SelectedPaintScale) {
                case PaintScale.Tiny:
                    DrawingScaleFactor = 1;
                    break;
                case PaintScale.Small:
                    DrawingScaleFactor = 2;
                    break;
                case PaintScale.Medium:
                    DrawingScaleFactor = 4;
                    break;
                case PaintScale.Large:
                    DrawingScaleFactor = 6;
                    break;
            }

            int posX = (int)MousePosition.X - DrawingScaleFactor * Screen.PixelWidth + 2;
            int posY = (int)MousePosition.Y - DrawingScaleFactor * Screen.PixelHeight + 2;
            int width = DrawingScaleFactor * Screen.PixelWidth;
            int height = DrawingScaleFactor * Screen.PixelHeight;
            highlightRect = new Rectangle(posX, posY, width, height);
        }
        public void UpdateMousePosition() {
            MousePosition = Globals._mouse.GetMousePosition();
        }
        public bool WithinArrayBorders() {
            var gridPosition = Screen.MousePositionToPixelPosition();
            return gridPosition.X > 0 && gridPosition.Y > 0 && gridPosition.X < Screen.GridWidth && gridPosition.Y < Screen.GridHeight;
        }
        public void DrawPixelAtPosition(SpriteBatch _spriteBatch, Pixel[,] grid) {
            var gridPosition = Screen.MousePositionToPixelPosition();
            var currentElement = PixelSelectionMenu.SelectedElement;
            int xCoord = (int)gridPosition.X;
            int yCoord = (int)gridPosition.Y;

            if (WithinArrayBorders() && grid[xCoord, yCoord] == null && Globals._mouse.LeftClickHold()) {
                var arrayCoords = new Rectangle((int)gridPosition.X - DrawingScaleFactor / 2, (int)gridPosition.Y - DrawingScaleFactor / 2, DrawingScaleFactor, DrawingScaleFactor);
                int lengthX = arrayCoords.Width + arrayCoords.X;
                int lengthY = arrayCoords.Height + arrayCoords.Y;

                if (DrawingScaleFactor == 1) {
                    PixelFactory.AddPixelToGrid(arrayCoords.X, arrayCoords.Y, grid, currentElement);
                } else {
                    for (int x = arrayCoords.X; x < lengthX; ++x) {
                        for (int y = arrayCoords.Y; y < lengthY; ++y) {
                            if (x > 0 && y > 0 && x < Screen.GridWidth && y < Screen.GridHeight) {
                                PixelFactory.AddPixelToGrid(x, y, grid, currentElement);
                            }
                        }
                    }
                }
            }
        }
        public void DrawHighlightedTile() {
            if (WithinArrayBorders()) {
                Color highlightColor = Color.White;

                highlightColor = (Color)PixelFactory.MapColorSchemes(PixelSelectionMenu.SelectedElement, "DisplayColor");

                highlightRect.X += DrawingScaleFactor / 2 * Screen.PixelWidth / 2;
                highlightRect.Y += DrawingScaleFactor / 2 * Screen.PixelHeight / 2;
                Globals._spriteBatch.Draw(highlightPixel, highlightRect, highlightColor);
            }
        }
    }
}