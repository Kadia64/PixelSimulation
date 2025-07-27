using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public class MeltingPoint : Reaction {

        private int ThresholdTemperature;
        private ElementType MeltedParticle;
        public MeltingPoint(int theshold, ElementType meltedParticle) {
            this.ThresholdTemperature = theshold;
            this.MeltedParticle = meltedParticle;
        }
        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {

            if (Globals.TemperatureMap[x, y] >= ThresholdTemperature) {
                ReactionMethods.TransformPixel(x, y, grid, MeltedParticle);
                return true;
            }

            return false;
        }

    }
}
