using System;
using Pixel_Simulation.Pixel_Behavior.Solids;


namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public static class Collisions {
        public static bool CheckCollideWithSolid(Pixel pixel, Pixel[,] grid) {
            var position = pixel.GridPosition;

            if (position.Y + 1 >= Screen.GridHeight) return false;

            if ((grid[position.X, position.Y + 1] is Solid solidPixel)) {
                return true;
            } else {
                return false;
            }
        }
    }

}
