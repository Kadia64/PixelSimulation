using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Properties;
using System;


namespace Pixel_Simulation.Pixel_Behavior.Reactions {
    public class ChemicalReaction : Reaction {

        private ElementType InteractionElement;
        private ElementType TransformedElement;
        public ChemicalReaction(ElementType interactionElement, ElementType transformedElement) {
            this.InteractionElement = interactionElement;
            this.TransformedElement = transformedElement;
        }

        public override bool ReactionConditions(int x, int y, Pixel[,] grid) {
            

            return false;
        }
    }
}
