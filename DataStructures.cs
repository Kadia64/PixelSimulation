using System;

namespace Pixel_Simulation {
    public struct Vector2i {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2i(int x, int y) {
            X = x;
            Y = y;
        }
        public override string ToString() {
            return $"X: {X}, Y: {Y}";
        }
    }


}
