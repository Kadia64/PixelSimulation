using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Pixel_Simulation.Input {
    public class MouseUserInput {

        public bool LeftDrag, RightDrag;

        public Vector2 newMousePos, oldMousePos, firstMousePos, newMouseAdjustedPos;

        public bool LeftClicked, LeftClickedHold, LeftClickedReleased, RightClicked, RightClickedHold, RightClickedReleased;

        public MouseState newMouse, oldMouse, firstMouse;
        public MouseStates CurrentMouseState;

        public MouseState First {
            get { return firstMouse; }
        }

        public MouseState New {
            get { return newMouse; }
        }

        public MouseState Old {
            get { return oldMouse; }
        }

        public MouseUserInput() {
            LeftDrag = false;
            RightDrag = false;
            newMouse = Mouse.GetState();
            oldMouse = newMouse;
            firstMouse = newMouse;

            newMousePos = new Vector2(newMouse.Position.X, newMouse.Position.Y);
            oldMousePos = new Vector2(newMouse.Position.X, newMouse.Position.Y);
            firstMousePos = new Vector2(newMouse.Position.X, newMouse.Position.Y);

            LeftClicked = false;
            LeftClickedHold = false;
            LeftClickedReleased = false;
            RightClicked = false;
            RightClickedHold = false;
            RightClickedReleased = false;

            GetMouseAndAdjust();
        }

        public void Update() {
            GetMouseAndAdjust();

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released) {
                firstMouse = newMouse;
                firstMousePos = newMousePos = GetScreenPos(firstMouse);
            }
        }
        public void UpdateOld() {
            oldMouse = newMouse;
            oldMousePos = GetScreenPos(oldMouse);
        }

        public float GetDistanceFromClick() { return 0.0f; }

        public void GetMouseAndAdjust() {
            newMouse = Mouse.GetState();
            newMousePos = GetScreenPos(newMouse);
        }

        public float GetMouseWheelChange() { return 0.0f; }

        // returns a vector of a given mouse position
        public Vector2 GetScreenPos(MouseState MOUSE) {
            return new Vector2(MOUSE.Position.X, MOUSE.Position.Y);
        }

        // returns a vector of the newest mouse position
        public Vector2 GetMousePosition() {
            return new Vector2(newMouse.Position.X, newMouse.Position.Y);
        }

        // returns a rectangle to check if the mouse is intersecting with another rectangle
        public Rectangle CheckMouseCollisionRect() {
            var position = GetMousePosition();
            return new Rectangle((int)position.X, (int)position.Y, 1, 1);
        }



        public bool LeftClick() {
            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= Screen.ScreenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= Screen.ScreenHeight) {
                return true;
            }

            return false;
        }
        public bool LeftClickHold() {
            bool holding = false;

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= Screen.ScreenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= Screen.ScreenHeight) {
                holding = true;

                if (Math.Abs(newMouse.Position.X - firstMouse.Position.X) > 8 || Math.Abs(newMouse.Position.Y - firstMouse.Position.Y) > 8) {
                    LeftDrag = true;
                }
            }
            return holding;
        }
        public bool LeftClickRelease() {
            if (newMouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed) {
                LeftDrag = false;
                return true;
            }
            return false;
        }
        public bool RightClick() {
            if (newMouse.RightButton == ButtonState.Pressed && oldMouse.RightButton != ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= Screen.ScreenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= Screen.ScreenHeight) {
                return true;
            }
            return false;
        }
        public bool RightClickHold() {
            bool holding = false;

            if (newMouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= Screen.ScreenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= Screen.ScreenHeight) {
                holding = true;

                if (Math.Abs(newMouse.Position.X - firstMouse.Position.X) > 8 || Math.Abs(newMouse.Position.Y - firstMouse.Position.Y) > 8) {
                    RightDrag = true;
                }
            }
            return holding;
        }
        public bool RightClickRelease() {
            if (newMouse.RightButton == ButtonState.Released && oldMouse.RightButton == ButtonState.Pressed) {
                RightDrag = false;
                return true;
            }

            return false;
        }

        public void InitializeMouseStates() {
            Globals._mouse.LeftClicked = (Globals._mouse.LeftClick());
            Globals._mouse.LeftClickedHold = Globals._mouse.LeftClickHold();
            Globals._mouse.LeftClickedReleased = (Globals._mouse.LeftClickRelease());

            Globals._mouse.RightClicked = (Globals._mouse.RightClick());
            Globals._mouse.RightClickedHold = (Globals._mouse.RightClickHold());
            Globals._mouse.RightClickedReleased = (Globals._mouse.RightClickRelease());
        }
        public void DebugMouseClicksOutput() {
            Debug.Write($"{Globals._mouse.GetScreenPos(Globals._mouse.New)}", 10, 10);
            Debug.Write($"Left Clicked: {Globals._mouse.LeftClicked}", 10, 25);
            Debug.Write($"Left Clicked Hold: {Globals._mouse.LeftClickedHold}", 10, 40);
            Debug.Write($"Left Clicked Released: {Globals._mouse.LeftClickedReleased}", 10, 55);
            Debug.Write($"Left Clicked Dragging: {Globals._mouse.LeftDrag}", 10, 70);

            Debug.Write($"Right Clicked: {Globals._mouse.RightClicked}", 10, 85);
            Debug.Write($"Right Clicked Hold: {Globals._mouse.RightClickedHold}", 10, 100);
            Debug.Write($"Right Clicked Released: {Globals._mouse.RightClickedReleased}", 10, 115);
            Debug.Write($"Right Clicked Dragging: {Globals._mouse.RightDrag}", 10, 130);
        }
    }
    public enum MouseStates {
        LeftClick, LeftClickHold, LeftClickRelease, LeftDrag,
        RightClick, RightClickHold, RightClickRelease, RightDrag,
        None
    }
}
