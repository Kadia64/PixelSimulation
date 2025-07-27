using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;
using Pixel_Simulation.Climate;
using System;

namespace Pixel_Simulation.Pixel_Behavior.Liquids {
    public class MoltenAluminium : Liquid {
        public MoltenAluminium(int x, int y) : base(x, y) {
            this.Element = ElementType.MoltenAluminium;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = MoltenAluminiumColorScheme.MoltenAluminiumColors;
            this.State = PixelState.Falling;

            this.Mass = 15.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.InitialSpreadVelocity = 50.0f;
            this.LiquidPulseChance = 20;
            this.LiquidDensity = 90;
            this.FreezingPoint = 1220;

            // Start at hot temperature (1220 + 20%)
            this.CurrentTemperature = CalculateMeltingPoint(this.FreezingPoint);
            this.reactionManager.RegisterReaction(new FreezingPoint(this.FreezingPoint, ElementType.Aluminium));

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 5.0f;

            // for molten fluids, only give a radius of 2 for temperature 
            // this radius should have the initial temperature of 1220 + 20% degrees
            // always have this temperature radius at the top of the molten aluminium pixel (testing)
            // if the lava heat gets close to the molten aluminium, it should stay molten
        }

        public void CoolDown(float deltaTime) {
            CurrentTemperature -= COOLING_RATE * deltaTime;
            CurrentTemperature = Math.Max(CurrentTemperature, 20f); // Room temperature minimum
            
            // Update global temperature map to reflect cooling
            if (GridPosition.X >= 0 && GridPosition.X < Screen.GridWidth && 
                GridPosition.Y >= 0 && GridPosition.Y < Screen.GridHeight) {
                Globals.TemperatureMap[GridPosition.X, GridPosition.Y] = CurrentTemperature;
            }
        }
        
        public bool CheckForExternalHeatSources(Pixel[,] grid) {
            int x = GridPosition.X;
            int y = GridPosition.Y;
            const float MIN_SUSTAINING_TEMP = 1220f;
            
            // Check surrounding pixels for lava, fire, or other hot sources
            // Don't count other molten aluminum as external heat
            for (int dx = -1; dx <= 1; dx++) {
                for (int dy = -1; dy <= 1; dy++) {
                    if (dx == 0 && dy == 0) continue;
                    
                    int checkX = x + dx;
                    int checkY = y + dy;
                    
                    if (checkX >= 0 && checkX < Screen.GridWidth && 
                        checkY >= 0 && checkY < Screen.GridHeight) {
                        
                        var neighbor = grid[checkX, checkY];
                        if (neighbor != null && !(neighbor is MoltenAluminium)) {
                            if (Globals.TemperatureMap[checkX, checkY] > MIN_SUSTAINING_TEMP) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
