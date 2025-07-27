using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Gases;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using System;

namespace Pixel_Simulation.Pixel_Behavior {
    public abstract class Dust : Pixel {
        public bool EnableWallBreakup = true;
        public float WallBreakVelY = 5.0f;
        public int MaxWallHeight = 2;
        public float SpreadValue = 25.0f;
        public int AccumulatedSpreadValue = 30;
        bool IsFlammable = false;
        protected Dust(int x, int y) : base(x, y) {

        }
        public override void SimulateFallingPhysics(Pixel[,] grid, float deltaTime) {
            int x = GridPosition.X;
            int y = GridPosition.Y;

            base.StoreOldPosition();
            base.CalculateFallingPhysics(grid, deltaTime);

            // check if your out of bounds
            if (!WithinArrayBoundaries(x, y)) {
                State = PixelState.Static;
                Velocity = Vector2.Zero;
                TargetGridPosition = GridPosition;
                return;
            }

            // check if you collide with another pixel
            bool collideWithSolid = Collisions.CheckCollideWithSolid(grid[x, y], grid);
            if (collideWithSolid) {
                State = PixelState.Static;
                Velocity = Vector2.Zero;
                TargetGridPosition = GridPosition;
                return;
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
                    stuckFrames = 0;
                    return;
                }
            }
            #endregion      

            if (y + 1 < Screen.GridHeight) {

#if true
                Pixel belowPixel = grid[x, y + 1];

                if (CanMoveTo(x, y + 1, grid) || belowPixel.State == PixelState.Falling) {
                    TargetGridPosition = new Vector2i(x, y + 1);
                    State = PixelState.Falling;
                    UpdateGridPosition(grid);
                    stuckFrames = 0;
                    return;
                }

                BounceEffect();

                bool canMoveLeft = (x - 1 >= 0) && CanMoveTo(x - 1, y + 1, grid);
                bool canMoveRight = (x + 1 < Screen.GridWidth) && CanMoveTo(x + 1, y + 1, grid);

                if (canMoveRight) {
                    TargetGridPosition = new Vector2i(x + 1, y + 1);
                    //Velocity.X += random.Next(AccumulatedSpreadValue);
                    Velocity.X += RandomEngine.GetNumber(AccumulatedSpreadValue);
                    State = PixelState.Falling;
                    UpdateGridPosition(grid);
                    stuckFrames = 0;
                    return;
                }
                if (canMoveLeft) {
                    TargetGridPosition = new Vector2i(x - 1, y + 1);
                    //Velocity.X -= random.Next(AccumulatedSpreadValue);
                    Velocity.X -= RandomEngine.GetNumber(AccumulatedSpreadValue);
                    State = PixelState.Falling;
                    UpdateGridPosition(grid);
                    stuckFrames = 0;
                    return;
                }
#endif

#if true
                ++stuckFrames;
                if (stuckFrames > maxStuckFrames) {
                    bool canMoveHorizontalLeft = (x - 1 >= 0) && CanMoveTo(x - 1, y, grid);
                    bool canMoveHorizontalRight = (x + 1 < Screen.GridWidth) && CanMoveTo(x + 1, y, grid);

                    if (canMoveHorizontalLeft && canMoveHorizontalRight) {
                        //bool moveRight = random.NextDouble() > 0.5;
                        bool moveRight = RandomEngine.GetDouble() > 0.5;
                        TargetGridPosition = new Vector2i(x + (moveRight ? 1 : -1), y);
                        Velocity.X += moveRight ? SpreadValue : -SpreadValue;
                        State = PixelState.Falling;
                        UpdateGridPosition(grid);
                        stuckFrames = 0;
                        return;
                    } else if (canMoveHorizontalRight) {
                        TargetGridPosition = new Vector2i(x + 1, y);
                        Velocity.X += SpreadValue;
                        State = PixelState.Falling;
                        UpdateGridPosition(grid);
                        stuckFrames = 0;
                        return;
                    } else if (canMoveHorizontalLeft) {
                        TargetGridPosition = new Vector2i(x - 1, y);
                        Velocity.X -= SpreadValue;
                        State = PixelState.Falling;
                        UpdateGridPosition(grid);
                        stuckFrames = 0;
                        return;
                    }
                }
#endif
            }

            // completly stuck
            State = PixelState.Static;
            Velocity *= 0.8f; // Slightly less damping to maintain some energy
            TargetGridPosition = new Vector2i(x, y);
            UpdateGridPosition(grid);

            // Reset stuck counter when truly at rest
            if (Math.Abs(Velocity.X) < 1f && Math.Abs(Velocity.Y) < 1f) {
                stuckFrames = 0;
            }
        }
        public override void BreakLeftTallWall(Pixel[,] grid) {

            int currentX = GridPosition.X;
            int currentY = GridPosition.Y;

            // Add bounds checking - need space to move left and check above
            if (currentX < 2 || currentY < MaxWallHeight) return;

            // Only check if this pixel is settled (not falling)
            if (this.State == PixelState.Static) {
                // Check if there's empty space to the left for the entire column height
                // and pixels exist in current column for 8 positions up
                bool canBreakLeft = true;

                for (int i = 0; i < MaxWallHeight; i++) {
                    // Check if current column has pixels
                    if (grid[currentX, currentY - i] == null) {
                        canBreakLeft = false;
                        break;
                    }

                    // Check if left column is empty
                    if (grid[currentX - 1, currentY - i] != null) {
                        canBreakLeft = false;
                        break;
                    }
                }

                if (canBreakLeft) {
                    // Move 2 spaces to the left to avoid immediate re-collision
                    //CommitPosition(new Vector2i(currentX - 1, currentY));
                    TargetGridPosition = new Vector2i(currentX - 1, currentY);
                    Velocity.X = -10f; // Give it some horizontal momentum
                    Velocity.Y = WallBreakVelY;  // Small downward velocity to help it fall
                    State = PixelState.Falling;
                    stuckFrames = 0;
                    //Console.WriteLine($"Left wall break at ({currentX}, {currentY})");
                    return;
                }
            }
        }
        public override void BreakRightTallWall(Pixel[,] grid) {

            int currentX = GridPosition.X;
            int currentY = GridPosition.Y;

            // Add bounds checking - need space to move right and check above
            if (currentX >= Screen.GridWidth - 2 || currentY < MaxWallHeight) return;

            // Only check if this pixel is settled (not falling)
            if (this.State == PixelState.Static) {
                // Check if there's empty space to the right for the entire column height
                // and pixels exist in current column for 8 positions up
                bool canBreakRight = true;

                for (int i = 0; i < MaxWallHeight; i++) {
                    // Check if current column has pixels
                    if (grid[currentX, currentY - i] == null) {
                        canBreakRight = false;
                        break;
                    }

                    // Check if right column is empty
                    if (grid[currentX + 1, currentY - i] != null) {
                        canBreakRight = false;
                        break;
                    }
                }

                if (canBreakRight) {
                    // Move 2 spaces to the right to avoid immediate re-collision
                    //CommitPosition(new Vector2i(currentX + 1, currentY));
                    TargetGridPosition = new Vector2i(currentX + 1, currentY);
                    Velocity.X = 10f;  // Give it some horizontal momentum
                    Velocity.Y = WallBreakVelY;  // Small downward velocity to help it fall
                    State = PixelState.Falling;
                    stuckFrames = 0;
                    //Console.WriteLine($"Right wall break at ({currentX}, {currentY})");
                    return;
                }
            }
        }
        public override void BreakSingleTallWall(Pixel[,] grid) {

            int currentX = GridPosition.X;
            int currentY = GridPosition.Y;

            // Add bounds checking
            //if (currentX < 2 || currentX >= ScreenGlobals.GridWidth - 2 || currentY < 8) return;
            if (currentY + 1 >= Screen.GridHeight - 2) return;

            if (grid[currentX, currentY + 1] == null) {
                //grid[currentX, currentY].IsFalling = true;
                grid[currentX, currentY].State = PixelState.Falling;
            }
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) {
            if (!WithinArrayBoundaries(x, y)) return false;

            var targetPixel = grid[x, y];

            if (targetPixel == null) {
                Motion = PixelMotion.Fall; // Normal falling
                return true;
            }

            if (targetPixel is Liquid || targetPixel is Gas) {
                Motion = PixelMotion.Swap; // Set swap motion
                return true; // Sand can move into water by swapping
            }

            Motion = PixelMotion.Fall; // Reset to fall for other cases
            return false;
        }
    }
}
