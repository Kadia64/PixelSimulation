using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Liquids {
    public class Oil : Liquid {
        public Oil(int x, int y) : base(x, y) {
            this.Element = ElementType.Oil;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = OilColorScheme.OilColors;
            this.State = PixelState.Falling;

            this.Mass = 10.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.InitialSpreadVelocity = 50.0f;
            this.LiquidDensity = 10;

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 5.0f;
        }
    }
}
