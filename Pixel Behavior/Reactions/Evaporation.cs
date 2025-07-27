using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Pixel_Behavior.Liquids;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public class Evaporation : Reaction {
        private int EvaporationRate;
        private ElementType EvaporatedParticle;
        public Evaporation(int evaporationRate, ElementType evaporatedParticle) {
            this.EvaporationRate = evaporationRate;
            this.EvaporatedParticle = evaporatedParticle;
        }

        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {

            if (grid[x, y] is Acid) {
                if (TryEvaporate(x, y, grid)) {
                    return true;
                }
            }

            return false;
        }

        private bool TryEvaporate(int x, int y, Pixel[,] grid) {

            bool chanceToEvaporate = RandomEngine.RandomChance(EvaporationRate);

            if (chanceToEvaporate) {
                ReactionMethods.TransformPixel(x, y, grid, EvaporatedParticle);
                return true;
            }

            return false;
        }
    }
}
