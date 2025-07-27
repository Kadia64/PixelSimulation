using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Climate;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Behavior.Reactions.Particles;

namespace Pixel_Simulation {
    public class PixelUpdater {


        public PixelUpdater() { }
    
        public void PixelUpdateDirections(Pixel[,] grid, float deltaTime) {
            if (!Globals.PhysicsEnabled) return;

            bool flip = RandomEngine.Flip();

            // render from bottom up, pixels that fall
            if (flip) {
                for (int x = 0; x < Screen.GridWidth; ++x) {
                    for (int y = Screen.GridHeight - 1; y >= 0; --y) {
                        UpdateFallingPixels(x, y, grid, deltaTime);
                    }
                }
            } else {
                for (int x = Screen.GridWidth - 1; x >= 0; --x) {
                    for (int y = Screen.GridHeight - 1; y >= 0; --y) {
                        UpdateFallingPixels(x, y, grid, deltaTime);
                    }
                }
            }

            if (Globals._FRAME_COUNTER_ % Globals.GasUpdateFrequency == 0) {
                for (int x = 0; x < Screen.GridWidth; ++x) {
                    for (int y = 0; y < Screen.GridHeight - 1; ++y) {

                        if (Globals._FRAME_COUNTER_ % 10 == 0) {
                            float temperatureLevel = Globals.TemperatureMap[x, y];
                            int mappedMultiple = TempColorsMapper.PossibleTemperatures[(int)temperatureLevel];
                            TempColorsMapper.ColorMap[x, y] = TempColorsMapper.LookupTable[mappedMultiple];
                            TempColorsMapper.AdjustTemperature(x, y);
                        }                        

                        UpdateFloatingPixels(x, y, grid, deltaTime);
                    }
                }
            }
        }
        private void UpdateFallingPixels(int x, int y, Pixel[,] grid, float deltaTime) {

            if (grid[x, y] != null) {
                var pixel = grid[x, y];

                if (pixel is Dust dustType) {
                    if (dustType.State == PixelState.Static && dustType.EnableWallBreakup) {
                        pixel.BreakLeftTallWall(grid);
                        pixel.BreakRightTallWall(grid);
                        pixel.BreakSingleTallWall(grid);
                    }
                }

                if (pixel is Dust && grid[x, y].State != PixelState.Static) {
                    pixel.SimulateFallingPhysics(grid, deltaTime);
                } else if (pixel is Liquid) {
                    pixel.SimulateLiquidPhysics(grid, deltaTime);
                } else if (pixel is Solid) {
                    pixel.SolidReactions(x, y, grid);
                }
            }
        }
        private void UpdateFloatingPixels(int x, int y, Pixel[,] grid, float deltaTime) {
            if (grid[x, y] != null) {
                var pixel = grid[x, y];

                if (pixel is Gas || pixel is Fire) {
                    pixel.SimulateFloatingPhysics(grid, deltaTime);
                }
            }
        }
    }
}
