using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Behavior.Gases;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Climate;

namespace Pixel_Simulation.Pixel_Behavior {
    public abstract class Liquid : Pixel {
        public int LiquidDensity = 0;
        public bool EnableLiquidPulse = true;
        public int LiquidPulseChance = 1000;
        public int EvaporationRate = 0;
        public ElementType EvaporatedParticle = ElementType.Erase;
        public int CorrosionRate = 3;

        public int CreationFrame = Globals._FRAME_COUNTER_;
        public float CurrentTemperature = 0;
        public float CoolingRate = 2.0f;

        protected Liquid(int x, int y) : base(x, y) {

        }
        public override void SimulateLiquidPhysics(Pixel[,] grid, float deltaTime) {
            int x = GridPosition.X;
            int y = GridPosition.Y;
            if (EnableLiquidPulse) CycleLiquidColors();

            // if the reaction executes, return early
            if (reactionManager.ProcessReactions(x, y, 2, 2, grid)) {
                return;
            }

            if (x + 1 < Screen.GridWidth && x - 1 > 0 && y + 1 < Screen.GridHeight && y - 1 > 0) {

                // sourrounded by the same type of pixels
                var currentType = this.GetType();
                if (grid[x + 1, y] != null && grid[x + 1, y].GetType() == currentType &&
                    grid[x - 1, y] != null && grid[x - 1, y].GetType() == currentType &&
                    grid[x, y + 1] != null && grid[x, y + 1].GetType() == currentType &&
                    grid[x, y - 1] != null && grid[x, y - 1].GetType() == currentType) {

                    //++FrameCountOnPixel;
                    return;
                }
            }

            if (RemoveParticlesAtBorder(grid, false)) return;

            base.StoreOldPosition();
            base.CalculateFallingPhysics(grid, deltaTime);

            if (!WithinArrayBoundaries(x, y)) {
                State = PixelState.Static;
                Velocity = Vector2.Zero;
                TargetGridPosition = GridPosition;
                return;
            }       

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

            if (CanMoveTo(x, y + 1, grid)) {
                TargetGridPosition = new Vector2i(x, y + 1);
                State = PixelState.Falling;
                UpdateGridPosition(grid);
                return;
            }
            if (CanMoveTo(x + 1, y + 1, grid)) {
                TargetGridPosition = new Vector2i(x + 1, y + 1);
                State = PixelState.Falling;
                UpdateGridPosition(grid);
                return;
            }
            if (CanMoveTo(x - 1, y + 1, grid)) {
                TargetGridPosition = new Vector2i(x - 1, y + 1);
                State = PixelState.Falling;
                UpdateGridPosition(grid);
                return;
            }

            // NOT WORKING -- Fucking fix it
            const int DISPERSE = 10;
            const float SPREAD_VELOCITY = 10f;
            bool randomDirection = RandomEngine.Flip();
            int randPosition = RandomEngine.GetNumber(DISPERSE);
            int movementCount = 0;

            if (randomDirection) {
                if (CanProjectToX(x, randPosition, Add, y, ref movementCount, grid)) {
                    TargetGridPosition = new Vector2i(x + movementCount, y);
                    Velocity.X += SPREAD_VELOCITY;
                    UpdateGridPosition(grid);
                    return;
                }
            } else {
                if (CanProjectToX(x, randPosition, Subtract, y, ref movementCount, grid)) {
                    TargetGridPosition = new Vector2i(x - movementCount, y);
                    Velocity.X -= SPREAD_VELOCITY;
                    UpdateGridPosition(grid);
                    return;
                }
            }
            
            State = PixelState.Static;
            Velocity *= 0.8f; // Slightly less damping to maintain some energy
            TargetGridPosition = new Vector2i(x, y);
            UpdateGridPosition(grid);
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) {
            if (!WithinArrayBoundaries(x, y)) return false;

            var targetPixel = grid[x, y];

            if (targetPixel == null) {
                Motion = PixelMotion.Fall;
                return true;
            }

            if (targetPixel is Liquid liquidPixel) {
                if (liquidPixel.LiquidDensity > 0 && LiquidDensity > 0) {
                    if (liquidPixel.LiquidDensity < LiquidDensity) {
                        Motion = PixelMotion.Swap;
                        return true;
                    }
                }
            }
            if (targetPixel is Gas) {
                Motion = PixelMotion.Swap;
                return true;
            }


            Motion = PixelMotion.Fall;
            return false;
        }
        public override bool CanProjectToX(int x, int Xlength, Func<int, int, int> operation, int y, ref int movementCount, Pixel[,] grid) {
            bool canMove = false;
            PixelMotion finalMotion = PixelMotion.Fall;

            for (int tx = 1; tx <= Xlength; ++tx) {
                int targetX = operation(x, tx);
                
                if (!WithinArrayBoundaries(targetX, y)) {
                    break;
                }
                
                var targetPixel = grid[targetX, y];
                
                // Can move to empty space
                if (targetPixel == null) {
                    movementCount = tx;
                    canMove = true;
                    finalMotion = PixelMotion.Fall;
                } else if (targetPixel is Liquid liquidPixel) {

                    if ((liquidPixel.LiquidDensity > 0 && LiquidDensity > 0) && (liquidPixel.LiquidDensity < this.LiquidDensity)) {
                        movementCount = tx;
                        canMove = true;
                        finalMotion = PixelMotion.Swap;
                        break;
                    }
                    //if (targetPixel.LiquidDensity > 0 && LiquidDensity > 0) && (targetPixel.LiquidDensity < this.LiquidDensity) { }                    
                }                    
                else {
                    break; // Hit an obstacle we can't move through
                }
            }
            
            Motion = finalMotion;
            return canMove;
        }
        public override void CycleLiquidColors() {                    
            int colorChangeChance = RandomEngine.GetNumber(LiquidPulseChance);
            bool colorChange = colorChangeChance == LiquidPulseChance / 2;
            
            if (colorChange) {
                PixelColor = ColorScheme[RandomEngine.GetNumber(ColorScheme.Length)];
            }
        }
    }
}
