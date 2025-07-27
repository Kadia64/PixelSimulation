using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Dusts {
    public class Sawdust : Dust {

        public Sawdust(int x, int y) : base(x, y) {
            this.Element = ElementType.Sawdust;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = SawdustColorScheme.SawdustColors;
            this.State = PixelState.Falling;

            this.Mass = 4.5f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.1f;
            this.Health = 75;
            this.Density = CalculateDensity(0.8f);
            this.IsFlammable = true;

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 10.0f;
        }
    }
}
