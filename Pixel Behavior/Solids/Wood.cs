using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Wood : Solid {

        public Wood(int x, int y) : base(x, y) {
            this.Element = ElementType.Wood;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = WoodColorScheme.WoodColors;

            this.Health = 100.0f;
            this.Density = 0.5f;
        }        
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
    }
}
