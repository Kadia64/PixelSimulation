using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Dusts {
    public class Salt : Dust {

        public Salt(int x, int y) : base(x, y) {
            this.Element = ElementType.Salt;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = SaltColorScheme.SaltColors;
            this.State = PixelState.Falling;

            this.Mass = 2.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.Health = 40;            
            this.Density = CalculateDensity(0.1f);

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 10.0f;
        }
    }
}
