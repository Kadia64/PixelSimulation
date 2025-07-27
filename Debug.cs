using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pixel_Simulation {
    public static class Debug {

        public static int _debug_counter1_ = 0;

        public static void Write(string text, int x, int y) {
            var font = Globals._debugFont;
            Globals._spriteBatch.DrawString(font, text, new Vector2(x, y), Color.White);
        }
        public static void DebugOutput(string text) {
            System.Diagnostics.Debug.WriteLine(text);
        }

        public static void DebugCountTrigger() {
            ++_debug_counter1_;
        }
        public static void DebugFunctionCounter() {
            Globals._spriteBatch.DrawString(Globals._debugFont, $"Function Count: {_debug_counter1_}", new Vector2(10, 10), Color.White);
        }
    }
}
