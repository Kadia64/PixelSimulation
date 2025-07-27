using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using Pixel_Simulation.Pixel_Properties;
using System;

namespace Pixel_Simulation.Pixel_Behavior.Liquids {
    public class Acid : Liquid {
        public Acid(int x, int y) : base(x, y) {
            this.Element = ElementType.Acid;
            this.PixelColor = AcidColorScheme.Light;
            this.ColorScheme = AcidColorScheme.AcidColors;
            this.State = PixelState.Falling;

            this.Mass = 5.0f;
            this.Restitution = 0.2f;
            this.AirResistance = 0.5f;
            this.InitialSpreadVelocity = 50.0f;
            this.LiquidDensity = 1;
            this.LiquidPulseChance = 5;
            this.EnableLiquidPulse = false;
            this.EvaporationRate = 700;
            this.EvaporatedParticle = ElementType.AcidFumes;
            this.CorrosionRate = 5;

            reactionManager.RegisterReaction(new Evaporation(this.EvaporationRate, this.EvaporatedParticle));
            reactionManager.RegisterReaction(new Corrosion(this.CorrosionRate, this.EvaporatedParticle));

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
            this.Velocity.Y = (float)(RandomEngine.GetDouble()) * 5.0f;
        }
    }
}
