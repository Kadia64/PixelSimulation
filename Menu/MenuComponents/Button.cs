using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Menu;

namespace Pixel_Simulation.Menu.MenuComponents {
    public class Button {

        public Rectangle ButtonDimensions;
        public string ButtonText;
        public Color ButtonColor, BorderColor, HighlightColor;
        public Color TextColor, TextHighlightColor, TextClickedColor, CurrentTextColor;
        public bool IsHighlighted = false;
        public bool CurrentButtonSelected = false;
        public bool IsPressed = false;
        public ElementType? ElementSelection = null;
        public PaintScale? PaintScaleSelection = null;

        private Vector2 textOffset;

         public Button(Rectangle buttonDimensions, string text, object selection, Color color, Color borderColor = default, Color highlightColor = default, Color textColor = default, Color textHighlightColor = default, Color textClickedColor = default, Vector2 textOffset = new Vector2()) {
            ButtonDimensions = buttonDimensions;
            ButtonText = text;
            ButtonColor = color;
            BorderColor = borderColor == default ? Color.Black : borderColor;
            HighlightColor = highlightColor == default ? Color.White : highlightColor;
            TextColor = textColor == default ? Color.Black : textColor;
            TextHighlightColor = textHighlightColor == default ? Color.Blue : textHighlightColor;
            TextClickedColor = textClickedColor == default ? Color.BlueViolet : textClickedColor;
            CurrentTextColor = textColor;
            this.textOffset = textOffset;

            if (selection is ElementType elementSelection) {
                ElementSelection = elementSelection;
            } else if (selection is PaintScale paintScale) {
                PaintScaleSelection = paintScale;
            }
        }
        public void CheckMouseBoundaries() {
            var mousePosition = Globals._mouse.GetMousePosition();
            if (ButtonDimensions.Intersects(Globals._mouse.CheckMouseCollisionRect())) {
                IsHighlighted = true;

                if (Globals._mouse.LeftClick() || Globals._mouse.LeftClickHold()) {
                    IsPressed = true;
                    CurrentButtonSelected = true;
                    CurrentTextColor = TextClickedColor;
                    MatchButtonSelections();
                } else {
                    IsPressed = false;
                    CurrentTextColor = TextHighlightColor;
                }

            } else {
                IsHighlighted = false;
                CurrentTextColor = TextColor;
            }
        }
        private void MatchButtonSelections() {
            if (ElementSelection != null) {
                PixelSelectionMenu.SelectedElement = (ElementType)ElementSelection;
            } else if (PaintScaleSelection != null) {
                PixelSelectionMenu.SelectedPaintScale = (PaintScale)PaintScaleSelection;
            }
        }

        public void DrawButton() {
            var drawColor = IsHighlighted ? HighlightColor : ButtonColor;
            Globals._shapes.FillOutline(ButtonDimensions, drawColor);
            Globals._shapes.DrawBoxOutline(ButtonDimensions, BorderColor, 2);

            Vector2 textSize = Globals._debugFont.MeasureString(ButtonText);
            Vector2 buttonCenter = new Vector2(
                ButtonDimensions.X + ButtonDimensions.Width / 2f,
                ButtonDimensions.Y + ButtonDimensions.Height / 2f
            );            
            Globals._spriteBatch.DrawString(Globals._debugFont, ButtonText, buttonCenter, CurrentTextColor, 0f, textSize / 2f, 1f, SpriteEffects.None, 0f);
        }
    }    
}
