using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pixel_Simulation.Pixel_Behavior.Liquids;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public class FreezingPoint : Reaction {
        private int ThresholdTemperature;
        private ElementType SolidParticle;
        public FreezingPoint(int threshold, ElementType solidParticle) {
            this.ThresholdTemperature = threshold;
            this.SolidParticle = solidParticle;
        }
        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {

            return false;
            /*var pixel = grid[x, y];
            if (pixel == null) return false;
            
            // Handle MoltenAluminium freezing using its current temperature
            if (pixel is Liquids.MoltenAluminium moltenPixel) {
                if (moltenPixel.CurrentTemperature <= ThresholdTemperature) {
                    ReactionMethods.TransformPixel(x, y, grid, SolidParticle);
                    return true;
                }
            }
            // For other materials, use the global temperature map
            else if (Globals.TemperatureMap[x, y] <= ThresholdTemperature) { 
                ReactionMethods.TransformPixel(x, y, grid, SolidParticle);
                return true;
            }

            return false;*/
        }
    }
}
