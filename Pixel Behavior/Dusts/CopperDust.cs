using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Dusts {
    public class CopperDust : Dust {

        public CopperDust(int x, int y) : base(x, y) {
            this.Element = ElementType.CopperDust;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = CopperDustColorScheme.CopperDustColors;
            this.State = PixelState.Falling;

            this.Mass = 6.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.SpreadValue = 10.0f;
            this.MaxWallHeight = 5;
            this.Health = 220.0f;
            this.Density = CalculateDensity(0.5f);

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 10.0f;
        }
    }
}
