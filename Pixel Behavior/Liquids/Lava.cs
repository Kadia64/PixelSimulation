using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;
using Pixel_Simulation.Pixel_Behavior.Reactions;

namespace Pixel_Simulation.Pixel_Behavior.Liquids {
    public class Lava : Liquid {
        public Lava(int x, int y) : base(x, y) {
            this.Element = ElementType.Lava;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = LavaColorScheme.LavaColors;
            this.State = PixelState.Falling;

            this.Mass = 12.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.InitialSpreadVelocity = 50.0f;
            this.LiquidDensity = 100;

            reactionManager.RegisterReaction(new TemperatureRadius(1800, this.Element));

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 5.0f;            
        }
    }
}