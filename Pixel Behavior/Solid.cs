using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Solids;

namespace Pixel_Simulation.Pixel_Behavior {
    public abstract class Solid : Pixel {
        protected Solid(int x, int y) : base(x, y) {

        }

        public override void SolidReactions(int x, int y, Pixel[,] grid) {
            if (reactionManager.ProcessReactions(x, y, 1, 1, grid)) {
                return;
            }
        }
    }
}
