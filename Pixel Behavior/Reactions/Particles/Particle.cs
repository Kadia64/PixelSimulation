using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pixel_Simulation.Pixel_Behavior.Reactions.Particles {
    public class Particle : Pixel {

        protected Particle(int x, int y) : base(x, y) { 
        
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) {
            if (!WithinArrayBoundaries(x, y)) return false;

            var targetPixel = grid[x, y];

            if (targetPixel == null) {
                Motion = PixelMotion.Float;
                return true;
            }

            if (targetPixel is Gas gasPixel) {
                Motion = PixelMotion.Swap;
                return true;
            }

            Motion = PixelMotion.Float;
            return false;
        }
    }
}
