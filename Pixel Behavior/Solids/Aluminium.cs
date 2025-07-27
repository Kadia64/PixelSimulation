using Pixel_Simulation.Menu.Pixel_Selection;
using Pixel_Simulation.Pixel_Behavior.Reactions;
using Pixel_Simulation.Pixel_Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Simulation.Pixel_Behavior.Solids {
    public class Aluminium : Solid {

        public Aluminium(int x, int y) : base(x, y) { 
            this.Element = ElementType.Aluminium;
            this.PixelColor = PixelActions.ChoosePixelColor(this.Element);
            this.ColorScheme = AluminiumColorScheme.AluminiumColors;

            this.Health = 450;
            this.Density = 0.7f;
            this.MeltingPoint = 1220;

            // to handle the freezing point
            // when the lava hits the solid aluminium with 1800 degrees, melt the aluminium at 1800 degrees
            // each position at the molten aluminium needs to upate the temparature map to 1800 degrees
            // molten aluminium has a starting temperature of 1220 + 20% degrees 
            // only decrease the temperature of molten aluminium if another temperature factor doesn't effect it.
            // example, if lava is around molten aluminium, it stays molten aluminium. If molten aluminium is by itself, it will decreaase temperature to 1220 degrees, and turn to aluminium

            // Solid aluminum should melt at its base melting point (1220), not the +20% version
            this.reactionManager.RegisterReaction(new MeltingPoint(this.MeltingPoint, ElementType.MoltenAluminium));
        }
        public override bool CanMoveTo(int x, int y, Pixel[,] grid) { return false; }
    }
}
