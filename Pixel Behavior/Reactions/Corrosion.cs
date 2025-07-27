using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Behavior.Gases;
using Pixel_Simulation.Pixel_Properties;
using System;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {

    public class Corrosion : Reaction {
        private int CorrosionRate;
        private ElementType? EvaporatedElement;
        public Corrosion(int corrosionRate, ElementType? evaporatedElement) {
            this.CorrosionRate = corrosionRate;
            this.EvaporatedElement = evaporatedElement;
        }

        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {
            if (TryCorrodeSolid(x, y, grid)) {
                return true;
            }
            if (TryCorrodeDust(x, y, grid)) {
                return true;
            }
            return false;
        }

        public bool TryCorrodeSolid(int x, int y, Pixel[,] grid) {
            int chooseToCorrode = RandomEngine.GetNumber(3);

            if (grid[x, y + 1] is Solid downPixel) {

                if (grid[x - 1, y] is Solid leftPixel && grid[x - 1, y].CanCorrode && chooseToCorrode == 1) {
                    if (Corrode(leftPixel, grid, x - 1, y)) {
                        return true;
                    }
                }

                if (grid[x + 1, y] is Solid rightPixel && grid[x + 1, y].CanCorrode && chooseToCorrode == 2) {
                    if (Corrode(rightPixel, grid, x + 1, y)) {
                        return true;
                    }
                }

                if (chooseToCorrode == 0 && grid[x, y + 1].CanCorrode) {
                    if (Corrode(downPixel, grid, x, y + 1)) {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool TryCorrodeDust(int x, int y, Pixel[,] grid) {
            int chooseToCorrode = RandomEngine.GetNumber(3);

            if (grid[x, y + 1] is Dust downPixel) {

                if (grid[x - 1, y] is Dust leftPixel && grid[x - 1, y].CanCorrode && chooseToCorrode == 1) {
                    if (Corrode(leftPixel, grid, x - 1, y)) {
                        return true;
                    }
                }

                if (grid[x + 1, y] is Dust rightPixel && grid[x + 1, y].CanCorrode && chooseToCorrode == 2) {
                    if (Corrode(rightPixel, grid, x + 1, y)) {
                        return true;
                    }
                }

                if (chooseToCorrode == 0 && grid[x, y + 1].CanCorrode) {
                    if (Corrode(downPixel, grid, x, y + 1)) {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool Corrode(Pixel pixel, Pixel[,] grid, int x, int y) {
            var currentHealth = pixel.Health;
            float density = pixel.Density;
            density = pixel.Density;
            int corrosionRate = RandomEngine.GetNumber(CorrosionRate, CorrosionRate * 2);
            float overallDamage = density * corrosionRate;

            grid[x, y].Health -= overallDamage;
            if (grid[x, y].Health < 0) {
                grid[x, y] = null;

                Pixel pixelObj = PixelFactory.CreatePixel((ElementType)EvaporatedElement, x, y);
                PixelFactory.AddPixelToGrid(x, y, grid, (ElementType)EvaporatedElement);
                return true;
            } else {
                return false;
            }
        }
    }
}
