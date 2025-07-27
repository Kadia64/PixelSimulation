using Pixel_Simulation.Pixel_Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public class TemperatureReaction : Reaction {

        private ElementType InteractionElement;
        private ElementType TransformedElement;
        public TemperatureReaction(ElementType interactionElement, ElementType transformedElement) {
            this.InteractionElement = interactionElement;
            this.TransformedElement = transformedElement;
        }

        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {

            float topTemperature = Globals.TemperatureMap[x, y + 1];
            float bottomTemperature = Globals.TemperatureMap[x, y - 1];

            int max = 400;

            if (Globals.TemperatureMap[x, y - 1] >= max ||
                Globals.TemperatureMap[x, y + 1] >= max ||
                Globals.TemperatureMap[x + 1, y] >= max ||
                Globals.TemperatureMap[x - 1, y] >= max ||
                Globals.TemperatureMap[x - 1, y - 1] >= max ||
                Globals.TemperatureMap[x + 1, y - 1] >= max ||
                Globals.TemperatureMap[x - 1, y + 1] >= max ||
                Globals.TemperatureMap[x + 1, y + 1] >= max) {

                var pixel0 = grid[x, y];
                if (pixel0 != null && ElementTypeMap.SameType(pixel0, InteractionElement)) {
                    pixel0.Health -= 1;

                    if (pixel0.Health <= 0) {
                        ReactionMethods.TransformPixel(x, y, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel1 = grid[x, y + 1];
                if (pixel1 != null && ElementTypeMap.SameType(pixel1, InteractionElement)) {
                    pixel1.Health -= 1;

                    if (pixel1.Health <= 0) {
                        ReactionMethods.TransformPixel(x, y + 1, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel2 = grid[x, y - 1];
                if (pixel2 != null && ElementTypeMap.SameType(pixel2, InteractionElement)) {
                    pixel2.Health -= 1;

                    if (pixel2.Health <= 0) {
                        ReactionMethods.TransformPixel(x, y - 1, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel3 = grid[x + 1, y];
                if (pixel3 != null && ElementTypeMap.SameType(pixel3, InteractionElement)) {
                    pixel3.Health -= 1;

                    if (pixel3.Health <= 0) {
                        ReactionMethods.TransformPixel(x + 1, y, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel4 = grid[x - 1, y];
                if (pixel4 != null && ElementTypeMap.SameType(pixel4, InteractionElement)) {
                    pixel4.Health -= 1;

                    if (pixel4.Health <= 0) {
                        ReactionMethods.TransformPixel(x - 1, y, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel5 = grid[x - 1, y - 1];
                if (pixel5 != null && ElementTypeMap.SameType(pixel5, InteractionElement)) {
                    pixel5.Health -= 1;

                    if (pixel5.Health <= 0) {
                        ReactionMethods.TransformPixel(x - 1, y - 1, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel6 = grid[x + 1, y - 1];
                if (pixel6 != null && ElementTypeMap.SameType(pixel6, InteractionElement)) {
                    pixel6.Health -= 1;

                    if (pixel6.Health <= 0) {
                        ReactionMethods.TransformPixel(x + 1, y - 1, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel7 = grid[x - 1, y + 1];
                if (pixel7 != null && ElementTypeMap.SameType(pixel7, InteractionElement)) {
                    pixel7.Health -= 1;

                    if (pixel7.Health <= 0) {
                        ReactionMethods.TransformPixel(x - 1, y + 1, grid, TransformedElement);
                        return true;
                    }
                }
                var pixel8 = grid[x + 1, y + 1];
                if (pixel8 != null && ElementTypeMap.SameType(pixel8, InteractionElement)) {
                    pixel8.Health -= 1;

                    if (pixel8.Health <= 0) {
                        ReactionMethods.TransformPixel(x + 1, y + 1, grid, TransformedElement);
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
