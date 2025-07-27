using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Simulation.Pixel_Behavior.Reactions.Particles {
    public static class ReactionMapper {

        // coal -> burns at 800 degrees
        // copper -> molten copper at 1980 degrees
        // gold -> molten gold at 1940 degrees
        // salt -> surrounded by water turns to salt water
        // sand -> ?
        // sawdust -> burns at 350 degrees

        // acid fumes -> ?
        // helium -> ?
        // hydrogen -> flammable gas, ignites near fire
        // methane -> flammable gas, ignites near fire, explosive within pressure
        // propane -> flammable gas, ignites near fire, explosive
        // smoke -> ?
        // steam -> converts to water at 250 degrees

        // acid -> corrodes solids, dusts, liquids : releases acid fumes particles
        public static void Acid_Evaporation() {

        }

        // lava -> converts water to steam, has a chance to convert it to stone/obsidian
        // molten aluminium -> converts to solid aluminium or aluminium dust below 1220 degrees
        // oil -> flammable, ignites near fire
        // slime -> ?
        // water -> converts to steam at 250 degrees : removes fire

        // iron -> converts to molten iron at 2800 degrees,
        // obsidian -> ?
        // polyethylene -> converts to molten at 180 degrees,
        // stone -> ?
        // titanium -> converts to molten titanium at 3030 degrees,
        // wood -> burns when near 400 degrees



    }
}
