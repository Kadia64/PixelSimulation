using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.Pixel_Behavior.Gases;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior {
    public abstract class Gas : Pixel {
        public int WispRate = 30;
        public int AbsorbtionRate = 10;
        public int GasDensity = 0;
        protected Gas(int posX, int posY) : base(posX, posY) {

        }
        public override void SimulateFloatingPhysics(Pixel[,] grid, float deltaTime) {
            int x = GridPosition.X;
            int y = GridPosition.Y;

            if (RemoveParticlesAtBorder(grid, false)) return;

            StoreOldPosition();
            CalculateFloatingPhysics(grid, deltaTime);

            if (!WithinArrayBoundaries(x, y)) {
                State = PixelState.Static;
                Velocity = Vector2.Zero;
                TargetGridPosition = GridPosition;
                return;
            }

            // try the wisp effect
            if (x + 1 < Screen.GridWidth && x - 1 > 0 && y + 1 < Screen.GridHeight && y - 1 > 0) {
                if (GasAbsorbtionRate(Element, grid)) {
                    // pixel has been deleted
                    return;
                }
            }

            #region try physics movement
            int targetX = TargetGridPosition.X;
            int targetY = TargetGridPosition.Y;
            targetX = Math.Max(0, Math.Min(Screen.GridWidth - 1, targetX));
            targetY = Math.Max(0, Math.Min(Screen.GridHeight - 1, targetY));
            if (targetX != x || targetY != y) {
                if (CanMoveTo(targetX, targetY, grid)) {
                    TargetGridPosition = new Vector2i(targetX, targetY);
                    State = PixelState.Falling;
                    UpdateGridPosition(grid); // This will handle both Fall and Swap motions
                    return;
                }
            }
            #endregion

            if (CanMoveTo(x, y - 1, grid)) {
                TargetGridPosition = new Vector2i(x, y - 1);
                State = PixelState.Falling;
                UpdateGridPosition(grid);
                return;
            }

            if (CanMoveTo(x + 1, y - 1, grid)) {
                TargetGridPosition = new Vector2i(x + 1, y - 1);
                State = PixelState.Falling;
                UpdateGridPosition(grid);
                return;
            }

            if (CanMoveTo(x - 1, y - 1, grid)) {
                TargetGridPosition = new Vector2i(x - 1, y - 1);
                State = PixelState.Falling;
                UpdateGridPosition(grid);
                return;
            }

            // completly stuck
            State = PixelState.Static;
            Velocity *= 0.8f; // Slightly less damping to maintain some energy
            TargetGridPosition = new Vector2i(x, y);
            UpdateGridPosition(grid);
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) {
            if (!WithinArrayBoundaries(x, y)) return false;

            var targetPixel = grid[x, y];

            if (targetPixel == null) {
                Motion = PixelMotion.Float;
                return true;
            }

            if (targetPixel is Gas gasPixel) {
                if (gasPixel.GasDensity > GasDensity) {
                    Motion = PixelMotion.Swap;
                    return true;
                }
            }

            Motion = PixelMotion.Float;
            return false;
        }
        public override bool GasAbsorbtionRate(ElementType element, Pixel[,] grid) {
            int x = GridPosition.X;
            int y = GridPosition.Y;
            bool pixelDeleted = false;

            Color color = Color.White;
            color = PixelActions.ChoosePixelColor(Element);

            //int invisible = random.Next(WispRate);
            int invisible = RandomEngine.GetNumber(WispRate);

            if (grid[x + 1, y] is Gas && grid[x - 1, y] is Gas && grid[x, y + 1] is Gas && grid[x, y - 1] is Gas) {
                PixelColor = color;
                if (invisible == WispRate / 2) {
                    PixelColor = Color.Transparent;

                    //bool readyToDelete = new Random().Next(AbsorbtionRate) == AbsorbtionRate / 2;
                    bool readyToDelete = RandomEngine.RandomChance(AbsorbtionRate);

                    if (readyToDelete) {
                        grid[x, y] = null;
                        pixelDeleted = true;
                    }
                }
            } else if (grid[x + 1, y] is Gas && grid[x - 1, y] is Gas) {
                PixelColor = color;
                if (invisible == WispRate / 2) {
                    PixelColor = Color.Transparent;

                    //bool readyToDelete = new Random().Next(AbsorbtionRate) == AbsorbtionRate / 2;
                    bool readyToDelete = RandomEngine.RandomChance(AbsorbtionRate);

                    if (readyToDelete) {
                        grid[x, y] = null;
                        pixelDeleted = true;
                    }
                }

            } else if ((grid[x + 1, y] is not Gas && grid[x - 1, y] is not Gas) || (grid[x + 1, y] is Gas && grid[x - 1, y] is not Gas) || (grid[x + 1, y] is not Gas && grid[x - 1, y] is Gas)) {
                // if right is not gas AND left is not gas  OR
                // if right is gas AND left is not gas      OR
                // if right is not gas AND left is gas      OR

                //bool readyToDelete = new Random().Next(AbsorbtionRate * 10) == AbsorbtionRate / 2;
                bool readyToDelete = RandomEngine.RandomChance(AbsorbtionRate, 10);

                if (readyToDelete) {
                    grid[x, y] = null;
                    pixelDeleted = true;
                }
            }
            
            return pixelDeleted;
        }
        public override bool RemoveParticlesAtBorder(Pixel[,] grid, bool enabled) {
            // used for in-world game, disable for debugging
            if (!enabled) return false;

            int x = GridPosition.X;
            int y = GridPosition.Y;
            bool pixelDeleted = false;

            if (y - 1 == 0 || y - 1 == 1 || y - 1 == 2 || y - 1 == -1) {
                grid[x, y] = null;
                pixelDeleted = true;
            }

            return pixelDeleted;
        }
    }
}
