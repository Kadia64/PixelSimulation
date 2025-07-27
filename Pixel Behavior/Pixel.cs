using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Pixel_Simulation.Menu;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Pixel_Behavior.Dusts;
using Pixel_Simulation.Pixel_Behavior.Gases;
using Pixel_Simulation.Pixel_Behavior.Solids;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Behavior.Reactions;

namespace Pixel_Simulation.Pixel_Behavior {
    public abstract class Pixel {
        
        public int ID { get; set; }
        public Color PixelColor;
        public Vector2 ScreenPosition;
        public Vector2i GridPosition;
        public Vector2i OldGridPosition;
        public Vector2i TargetGridPosition;
        public Rectangle RectObj;
        public Texture2D PixelObj;
        public int FrameCountOnPixel = 0;
        
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        private Vector2 accumulatedMovement = Vector2.Zero;
        public float Gravity { get; set; } = 300.0f;
        public float Mass { get; set; }  = 1.0f;                   
        public float Restitution { get; set; }  = 0.3f;            
        public float AirResistance { get; set; } = 0.1f;          
        public float InitialSpreadVelocity { get; set; } = 60.0f;
        public float Health { get; set; } = 0.0f;
        public float Density { get; set; } = 1 - 0.5f;
        public int FreezingPoint { get; set; } = 0;
        public int MeltingPoint { get; set; } = 0;
        public int BoilingPoint { get; set; } = 0;
        public bool IsFlammable { get; set; } = false;
        public bool BurnState { get; set; } = false;
        public bool CanCorrode { get; set; } = true;

        public bool MarkedHealth = false;
        
        // Thermal properties
        public float CurrentTemperature { get; set; } = 20.0f; // Room temperature default
        public int CreationFrame { get; set; } = 0;
        public const float COOLING_RATE = 2.0f; // Degrees per second
        
        protected ReactionManager reactionManager = new ReactionManager();

        public PixelState State;
        public PixelMotion Motion;
        public ElementType Element;
        public Color[] ColorScheme;

        protected int stuckFrames = 0;
        protected const int maxStuckFrames = 30;
        public Func<int, int, int> Add = (x, y) => x + y;
        public Func<int, int, int> Subtract = (x, y) => x - y;

        protected Pixel(int x, int y) {
            this.RectObj = new Rectangle(x, y, Screen.PixelWidth, Screen.PixelHeight);
            this.PixelObj = Globals._worldPixel;

            this.ScreenPosition = new Vector2(x, y);
            this.GridPosition = Screen.ConvertScreenToGridPosition((int)ScreenPosition.X, (int)ScreenPosition.Y);
            this.TargetGridPosition = this.GridPosition;
            this.CreationFrame = Globals._FRAME_COUNTER_;
        }

        // abstract methods that must be implemented by derived classes
        public abstract bool CanMoveTo(int x, int y, Pixel[,] grid);

        // virtual methods that can be overridden
        #region update methods
        public virtual void UpdatePixelPosition() {
            ScreenPosition.X = RectObj.X;
            ScreenPosition.Y = RectObj.Y;

            // Update grid coordinates
            GridPosition = Screen.ConvertScreenToGridPosition((int)ScreenPosition.X, (int)ScreenPosition.Y);
        }
        public virtual void UpdateGridPosition(Pixel[,] grid) {
            if (Motion == PixelMotion.Fall || Motion == PixelMotion.Float) {
                // Normal falling - just clear old position
                if (grid[OldGridPosition.X, OldGridPosition.Y] == this) {
                    grid[OldGridPosition.X, OldGridPosition.Y] = null;
                }
            } else if (Motion == PixelMotion.Swap) {
                SwappingLogic(grid);
            }

            GridPosition = TargetGridPosition;
            grid[GridPosition.X, GridPosition.Y] = this;

            // Update screen position to match grid position
            RectObj.X = GridPosition.X * Screen.PixelWidth + Screen.StartMapPositionX;
            RectObj.Y = GridPosition.Y * Screen.PixelHeight + Screen.StartMapPositionY;

            FrameCountOnPixel = 0;
            UpdatePixelPosition();
        }
        public virtual void StoreOldPosition() {
            OldGridPosition.X = GridPosition.X;
            OldGridPosition.Y = GridPosition.Y;
        }
        public virtual void SwappingLogic(Pixel[,] grid) {
            // Swapping logic
            Pixel targetPixel = grid[TargetGridPosition.X, TargetGridPosition.Y];

            if (targetPixel != null) {
                // Move the target pixel to our old position
                targetPixel.GridPosition = this.OldGridPosition;
                targetPixel.TargetGridPosition = this.OldGridPosition;

                // Update target pixel's screen position
                targetPixel.RectObj.X = this.OldGridPosition.X * Screen.PixelWidth + Screen.StartMapPositionX;
                targetPixel.RectObj.Y = this.OldGridPosition.Y * Screen.PixelHeight + Screen.StartMapPositionY;
                targetPixel.UpdatePixelPosition();

                grid[OldGridPosition.X, OldGridPosition.Y] = targetPixel;
            }

            // Reset motion back to fall for next frame
            if (this is Dust) {
                Motion = PixelMotion.Fall;
            } else if (this is Liquid) {
                Motion = PixelMotion.Fall;
            } else if (this is Gas) {
                Motion = PixelMotion.Float;
            }
        }
        #endregion

        #region Physics
        public virtual void CalculateFallingPhysics(Pixel[,] grid, float deltaTime) {
            Acceleration = new Vector2(0, Gravity);

            // add other forces here (wind, etc.)


            // update velocity
            Velocity += Acceleration * deltaTime;

            // air resistance
            Velocity *= (1.0f - AirResistance * deltaTime);

            // accumulatedMovement basically acts as a "physics-to-grid translator" that
            // preserves the smooth nature of physics while working with the grid system
            accumulatedMovement += Velocity * deltaTime;

            // convert accumulated movement to grid steps            
            
            Vector2i gridMovement = new Vector2i(
                (int)Math.Floor(Math.Abs(accumulatedMovement.X)),
                (int)Math.Floor(Math.Abs(accumulatedMovement.Y))
            );
            //gridMovement.X += 1;
            if (Math.Abs(accumulatedMovement.X) > 0.2f && gridMovement.X == 0) {
                gridMovement.X = Math.Sign(accumulatedMovement.X); // Move just 1 step
            }

            // Apply the movement direction
            if (accumulatedMovement.X < 0) gridMovement.X = -gridMovement.X;
            if (accumulatedMovement.Y < 0) gridMovement.Y = -gridMovement.Y;

            // Set target position based on physics
            if (gridMovement.X != 0 || gridMovement.Y != 0) {

                // adding the current grid position to the updated movement position
                // creating a target position where to move to
                TargetGridPosition = new Vector2i(
                    GridPosition.X + gridMovement.X,
                    GridPosition.Y + gridMovement.Y
                );


                // subtracts the movement we just used from the accumulator
                // preserving the leftover fractional movement for the next frame, ex:
                // accumulatedMovement.X = -2.7   
                // gridMovement.X = -2  two spaces to the left
                // accumulatedMovement.X = -2.7 - (-2) = -0.7 (leftover for next frame)
                accumulatedMovement.X -= gridMovement.X;
                accumulatedMovement.Y -= gridMovement.Y;

                State = PixelState.Falling;
            } else {
                TargetGridPosition = GridPosition;

                if (Math.Abs(accumulatedMovement.Y) > 0.1f) {
                    State = PixelState.Falling;
                }
            }
        }
        public virtual void CalculateFloatingPhysics(Pixel[,] grid, float deltaTime) {
            Acceleration = new Vector2(0, -Gravity);
            // Add other forces here (wind, convection currents, etc.)

            // Update velocity
            Velocity += Acceleration * deltaTime;

            // Air resistance (gases still experience this, maybe even more so)
            Velocity *= (1.0f - AirResistance * deltaTime);

            // Physics-to-grid translator that preserves smooth movement
            accumulatedMovement += Velocity * deltaTime;

            // Convert accumulated movement to grid steps            
            Vector2i gridMovement = new Vector2i(
                (int)Math.Floor(Math.Abs(accumulatedMovement.X)),
                (int)Math.Floor(Math.Abs(accumulatedMovement.Y))
            );

            // Ensure small movements still register
            if (Math.Abs(accumulatedMovement.X) > 0.2f && gridMovement.X == 0) {
                gridMovement.X = Math.Sign(accumulatedMovement.X);
            }

            // Apply movement direction
            if (accumulatedMovement.X < 0) gridMovement.X = -gridMovement.X;
            if (accumulatedMovement.Y < 0) gridMovement.Y = -gridMovement.Y;

            // Set target position based on physics
            if (gridMovement.X != 0 || gridMovement.Y != 0) {
                TargetGridPosition = new Vector2i(
                    GridPosition.X + gridMovement.X,
                    GridPosition.Y + gridMovement.Y
                );

                // Preserve leftover fractional movement
                accumulatedMovement.X -= gridMovement.X;
                accumulatedMovement.Y -= gridMovement.Y;
                State = PixelState.Floating; // New state for gas particles
            } else {
                TargetGridPosition = GridPosition;
                if (Math.Abs(accumulatedMovement.Y) > 0.1f) {
                    State = PixelState.Floating;
                }
            }
        }
        public virtual void BounceEffect() {
            if (Velocity.Y > 0) {
                Velocity.Y = -Velocity.Y * Restitution;
                if (Math.Abs(Velocity.Y) < 10f) {
                    Velocity.Y = 0;
                }
            }
        }
        public virtual float CalculateDensity(float density) {
            return 1 - density;
        }
        public virtual int CalculateMeltingPoint(int meltingPoint) {
            const float percentage = 0.2f; // added 20% of the melting point
            return (int)(meltingPoint + (meltingPoint * percentage));
        }        
        public virtual void SimulateFallingPhysics(Pixel[,] grid, float deltaTime) { } // implementation: Dust.cs
        public virtual void SimulateLiquidPhysics(Pixel[,] grid, float deltaTime) { } // implementation: Liquid.cs
        public virtual void SimulateFloatingPhysics(Pixel[,] grid, float deltaTime) { } // implementation: Gas.cs
        public virtual void BreakLeftTallWall(Pixel[,] grid) { } // implementation: Dust.cs
        public virtual void BreakRightTallWall(Pixel[,] grid) { } // implementation: Dust.cs
        public virtual void BreakSingleTallWall(Pixel[,] grid) { } // implementation: Dust.cs
        public virtual bool CanProjectToX(int x, int Xlength, Func<int, int, int> operation, int y, ref int movementCount, Pixel[,] grid) { return false; } // implementation: Liquid.cs
        public virtual void CycleLiquidColors() { } // implementation: Liquid.cs
        public virtual bool GasAbsorbtionRate(ElementType element, Pixel[,] grid) { return false; } // implementation: Gas.cs        
        public virtual bool RemoveParticlesAtBorder(Pixel[,] grid, bool enabled) { return false; } // implementation: Gas.cs
        public virtual void SolidReactions(int x, int y, Pixel[,] grid) { }
        #endregion

        public virtual bool WithinArrayBoundaries(int x, int y) {
            return (x >= 0 && x < Screen.GridWidth && y >= 0 && y < Screen.GridHeight);
        }
        public virtual void Draw() {
            Globals._spriteBatch.Draw(PixelObj, RectObj, PixelColor);
        }
    }    
    public enum PixelState {
        Falling, Floating, Static
    }
    public enum PixelMotion {
        Fall,    // free falling with no pixel in its target position
        Float,
        Swap,    // swapping with another pixel
        Delete   // removing pixel from the grid
    }
    public enum ElementType {

        #region Dusts
        Sand,
        CoalDust,
        CopperDust,
        Salt,
        Sawdust,
        GoldDust,
        Gunpowder,
        Sodium,
        Snow,
        IronDust,
        #endregion

        #region Liquids
        Water,
        Lava,
        Slime,
        Acid,
        MoltenAluminium,
        Oil,
        WetConcrete,
        MoltenSteel,
        MoltenIron,
        LiquidNitrogen,
        #endregion

        #region Gases
        Smoke,
        Steam,
        Methane,
        Hydrogen,
        AcidFumes,
        Propane,
        Helium,
        Fluorine,
        Ammonia,
        SulfurDioxide,
        #endregion 

        #region Solids
        Wood,
        Stone,
        Titanium,
        Iron,
        Polyethylene,
        Obsidian,
        Aluminium,
        Steel,
        //Copper,
        //Gold,
        Diamond,
        #endregion


        Fire,
        Erase
    }
}
