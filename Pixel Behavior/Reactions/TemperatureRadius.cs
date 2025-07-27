using Pixel_Simulation.Climate;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public class TemperatureRadius : Reaction {
        private int TemperatureLevel;
        private ElementType Element;
        public TemperatureRadius(int temperature, ElementType element) {
            this.TemperatureLevel = temperature;
            this.Element = element;
        }
        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {
            var pixel = grid[x, y];
            if (pixel == null || !ElementTypeMap.SameType(pixel, Element)) return false;
            
            // Original logic for other elements that use Temperature reaction
            if (ElementTypeMap.SameType(grid[x, y], Element) &&
                (grid[x, y - 1] is null && ElementTypeMap.SameType(grid[x, y + 1], Element)) ||
                (grid[x - 1, y] is Solid && ElementTypeMap.SameType(grid[x + 1, y], Element)) ||
                (grid[x + 1, y] is Solid && ElementTypeMap.SameType(grid[x - 1, y], Element)) ||
                (grid[x, y + 1] is Solid && ElementTypeMap.SameType(grid[x, y - 1], Element))) {

                if (Globals._FRAME_COUNTER_ % 70 == 0) {
                    TempColorsMapper.ApplyCircularHeat(x, y, TemperatureLevel, 5, grid, Element);
                }
            }

            return false;
        }        

    }
}
