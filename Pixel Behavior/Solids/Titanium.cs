using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Titanium : Solid {

        public Titanium(int x, int y) : base(x, y) {
            this.Element = ElementType.Titanium;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = TitaniumColorScheme.TitaniumColors;

            this.Health = 800;
            this.Density = 0.9f;
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
    }
}
