using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Pixel_Simulation.View;

namespace Pixel_Simulation {
    public class Shapes {

        public Shapes() {

        }

        public void DrawVerticalLine(Point startY, Point endY, Color color, int thickness = 1) {
            if (startY.X != endY.X) return;
            int x = startY.X;

            for (int y = startY.Y; y < endY.Y; ++y) {
                var rectObj = new Rectangle(x, y, 1 * thickness, 1 * thickness);
                Globals._spriteBatch.Draw(Globals._reusablePixel, rectObj, color);
            }
        }
        public void DrawHorizontalLine(Point startX, Point endX, Color color, int thickness = 1) {
            if (startX.Y != endX.Y) return;
            int y = startX.Y;

            for (int x = startX.X; x < endX.X; ++x) {
                var rectObj = new Rectangle(x, y, 1 * thickness, 1 * thickness);
                Globals._spriteBatch.Draw(Globals._reusablePixel, rectObj, color);
            }
        }
        public void DrawTriangle(Point point1, Point point2, Point point3) {

        }

        public void DrawBoxOutline(Rectangle dim, Color color, int thickness = 1) {
            var topLinePoint1 = new Point(dim.X, dim.Y);
            var topLinePoint2 = new Point(dim.X + dim.Width, dim.Y);
            var bottomLinePoint1 = new Point(dim.X, dim.Y + dim.Height);
            var bottomLinePoint2 = new Point(dim.X + dim.Width, dim.Y + dim.Height);
            var leftLinePoint1 = new Point(dim.X, dim.Y);
            var leftLinePoint2 = new Point(dim.X, dim.Y + dim.Height);
            var rightLinePoint1 = new Point(dim.X + dim.Width, dim.Y);
            var rightLinePoint2 = new Point(dim.X + dim.Width, dim.Y + dim.Height);

            DrawHorizontalLine(topLinePoint1, topLinePoint2, color, thickness);
            DrawHorizontalLine(bottomLinePoint1, bottomLinePoint2, color, thickness);
            DrawVerticalLine(leftLinePoint1, leftLinePoint2, color, thickness);
            DrawVerticalLine(rightLinePoint1, rightLinePoint2, color, thickness);
        }
        public void FillOutline(Rectangle dim, Color color, int thickness = 1) {
            dim.X += thickness;
            dim.Y += thickness;
            dim.Width -= thickness;
            dim.Height -= thickness;
            Globals._spriteBatch.Draw(Globals._reusablePixel, dim, color);
        }

        // Enhanced methods with coordinate system specification
        public void DrawBoxOutline(Rectangle rect, Color color, CoordinateSystem coordinateSystem, int thickness = 1) {
            Rectangle transformedRect = TransformRectangle(rect, coordinateSystem);
            DrawBoxOutline(transformedRect, color, thickness);
        }

        // Convenience overload for x, y, width, height
        public void DrawBoxOutline(int x, int y, int width, int height, Color color, CoordinateSystem coordinateSystem, int thickness = 1) {
            DrawBoxOutline(new Rectangle(x, y, width, height), color, coordinateSystem, thickness);
        }

        private Rectangle TransformRectangle(Rectangle rect, CoordinateSystem coordinateSystem) {
            switch (coordinateSystem) {
                case CoordinateSystem.World:
                    // Already in world coordinates, no transformation needed
                    return rect;
                    
                case CoordinateSystem.Screen:
                    // Transform screen coordinates to world coordinates
                    if (Globals._camera != null) {
                        var topLeft = new Vector2(rect.X, rect.Y);
                        var transformedTopLeft = Globals._camera.ScreenToWorld(topLeft);
                        return new Rectangle((int)transformedTopLeft.X, (int)transformedTopLeft.Y, rect.Width, rect.Height);
                    }
                    return rect;
                    
                default:
                    return rect;
            }
        }
    }
    public class WorldShapes {

        public void DrawVerticalLine(Point startY, Point endY, Color color, int thickness = 1) {
            if (startY.X != endY.X) return;
            int x = startY.X;

            for (int y = startY.Y; y < endY.Y; ++y) {
                var rectObj = new Rectangle(x, y, 1 * thickness, 1 * thickness);
                Globals._spriteBatch.Draw(Globals._reusablePixel, rectObj, color);
            }
        }
        public void DrawHorizontalLine(Point startX, Point endX, Color color, int thickness = 1) {
            if (startX.Y != endX.Y) return;
            int y = startX.Y;

            for (int x = startX.X; x < endX.X; ++x) {
                var rectObj = new Rectangle(x, y, 1 * thickness, 1 * thickness);
                Globals._spriteBatch.Draw(Globals._reusablePixel, rectObj, color);
            }
        }
        public void DrawBoxOutline(Rectangle dim, Color color, int thickness = 1) {
            var topLinePoint1 = new Point(dim.X, dim.Y);
            var topLinePoint2 = new Point(dim.X + dim.Width, dim.Y);
            var bottomLinePoint1 = new Point(dim.X, dim.Y + dim.Height);
            var bottomLinePoint2 = new Point(dim.X + dim.Width, dim.Y + dim.Height);
            var leftLinePoint1 = new Point(dim.X, dim.Y);
            var leftLinePoint2 = new Point(dim.X, dim.Y + dim.Height);
            var rightLinePoint1 = new Point(dim.X + dim.Width, dim.Y);
            var rightLinePoint2 = new Point(dim.X + dim.Width, dim.Y + dim.Height);

            DrawHorizontalLine(topLinePoint1, topLinePoint2, color, thickness);
            DrawHorizontalLine(bottomLinePoint1, bottomLinePoint2, color, thickness);
            DrawVerticalLine(leftLinePoint1, leftLinePoint2, color, thickness);
            DrawVerticalLine(rightLinePoint1, rightLinePoint2, color, thickness);
        }
    }
}
