using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;
using Pixel_Simulation.Pixel_Behavior.Reactions;

namespace Pixel_Simulation.Pixel_Behavior.Dusts {
    public class Gunpowder : Dust {

        public Gunpowder(int x, int y) : base(x, y) {
            this.Element = ElementType.Gunpowder;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            //this.ColorScheme = SandColorScheme.SandColors;
            this.State = PixelState.Falling;

            this.Mass = 4.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.1f;
            this.Health = 80;
            this.Density = CalculateDensity(0.2f);

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 10.0f;
        }
    }
}
