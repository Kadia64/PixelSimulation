using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Iron : Solid {

        public Iron(int x, int y) : base(x, y) {
            this.Element = ElementType.Iron;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = IronColorScheme.IronColors;

            this.Health = 400;
            this.Density = 0.759f;
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
    }
}
