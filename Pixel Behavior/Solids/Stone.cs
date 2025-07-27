using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Stone : Solid {        

        public Stone(int x, int y) : base(x, y) {
            this.Element = ElementType.Stone;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = StoneColorScheme.StoneColors;

            this.Health = 250;
            this.Density = 0.7f;
        }        
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
    }
}
