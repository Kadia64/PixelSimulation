using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Liquids {
    public class Water : Liquid {
        public Water(int x, int y) : base(x, y) {
            this.Element = ElementType.Water;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.State = PixelState.Falling;
            this.ColorScheme = WaterColorScheme.WaterColors;

            this.Mass = 5.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.InitialSpreadVelocity = 50.0f;
            this.LiquidDensity = 15;
            this.Health = 100;

            reactionManager.RegisterReaction(new TemperatureReaction(ElementType.Water, ElementType.Steam));

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 5.0f;
        } 
    }
}
