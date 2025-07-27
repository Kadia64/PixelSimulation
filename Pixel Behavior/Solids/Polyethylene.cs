using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Polyethylene : Solid {

        public Polyethylene(int x, int y) : base(x, y) {
            this.Element = ElementType.Polyethylene;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = PolyethyleneColorScheme.PolyethyleneColors;

            this.Health = 350.0f;
            this.Density = 0.6f;
            this.CanCorrode = false;
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
    }
}
