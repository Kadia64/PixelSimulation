using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Pixel_Behavior.Gases {
    public class AcidFumes : Gas {

        public AcidFumes(int x, int y) : base(x, y) {
            this.Element = ElementType.AcidFumes;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = AcidFumesColorScheme.AcidFumesColors;
            this.State = PixelState.Floating;

            this.Mass = 0.8f;
            this.Gravity = 100.0f;
            this.InitialSpreadVelocity = 20.0f;
            this.GasDensity = 5;

            this.Velocity.X = (float)(RandomEngine.GetDouble() - 0.5) * this.InitialSpreadVelocity;
        }
    }
}
