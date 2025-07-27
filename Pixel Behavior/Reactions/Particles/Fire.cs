using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.Climate;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Reactions.Particles {
    public class Fire : Particle {

        public Fire(int x, int y) : base(x, y) {
            this.Element = ElementType.Fire;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = FireColorScheme.FireColors;
            this.State = PixelState.Falling;

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 5.0f;            
        }

        public override void SimulateFloatingPhysics(Pixel[,] grid, float deltaTime) {
            int x = GridPosition.X;
            int y = GridPosition.Y;

            if (RemoveParticlesAtBorder(grid, false)) return;

            StoreOldPosition();
            CalculateFloatingPhysics(grid, deltaTime);

            if (Globals._FRAME_COUNTER_ % 100 == 0) {
                //TempColorsMapper.ApplyCircularHeat(x, y, 1200, 1, grid);
            }

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

        public override bool GasAbsorbtionRate(ElementType element, Pixel[,] grid) {
            int x = GridPosition.X;
            int y = GridPosition.Y;
            bool pixelDeleted = false;

            Color color = Color.White;
            color = PixelActions.ChoosePixelColor(Element);
            int randomLiveFrames = RandomEngine.GetNumber(20, 25);

            if (Globals._FRAME_COUNTER_ % randomLiveFrames == 0) {
                grid[x, y] = null;
                //Transform.TransformPixel(x, y, grid, ElementType.Smoke);
                return true;
            }

            return false;
        }
    }
}
