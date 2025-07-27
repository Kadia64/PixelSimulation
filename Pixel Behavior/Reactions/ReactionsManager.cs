using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Properties;
using System;
using System.Collections.Generic;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {

    public abstract class Reaction {
        protected ElementType sourceElement;
        protected ElementType targetElement;
        public abstract bool ReactionConditions(int x, int y, Pixel[,] grid);
        public virtual bool WithinBounds(int x, int y, int offsetX, int offsetY) {
            return (x - 1 >= 0 && x + 1 < Screen.GridWidth && y - 1 >= 0 && y + 1 < Screen.GridHeight);
        }
    }

    public class ReactionManager {
        private List<Reaction> reactions = new List<Reaction>();
        public void RegisterReaction(Reaction reaction) {
            reactions.Add(reaction);
        }
        public bool ProcessReactions(int x, int y, int offsetX, int offsetY, Pixel[,] grid) {
            foreach (var reaction in reactions) {
                if (reaction.WithinBounds(x, y, offsetX, offsetY)) {
                    if (reaction.ReactionConditions(x, y, grid)) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public static class ReactionMethods {
        public static void TransformPixel(int x, int y, Pixel[,] grid, ElementType targetPixel) {
            DeletePixel(x, y, grid);
            PixelFactory.AddPixelToGrid(x, y, grid, targetPixel);
        }
        public static void DeletePixel(int x, int y, Pixel[,] grid) {
            grid[x, y] = null;
        }
    }    
}
