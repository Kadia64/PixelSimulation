using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Obsidian : Solid {

        public Obsidian(int x, int y) : base(x, y) {
            this.Element = ElementType.Obsidian;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);

            this.Health = 780;
            this.Density = 0.9f;
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
        public override void Draw() {
            Globals._spriteBatch.Draw(PixelObj, RectObj, PixelColor);
        }
    }
}
