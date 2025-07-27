using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pixel_Simulation.View {
    public class Camera {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        
        // Camera movement settings
        public float MoveSpeed { get; set; } = 300f;
        public float ZoomSpeed { get; set; } = 0.1f;
        public float MinZoom { get; set; } = 0.1f;
        public float MaxZoom { get; set; } = 5f;
        
        // Viewport info
        private int viewportWidth;
        private int viewportHeight;
        
        // World bounds (optional - set to limit camera movement)
        public Rectangle? WorldBounds { get; set; }
        
        public Camera(int viewportWidth, int viewportHeight) {
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
            
            Position = new Vector2(400.0f, 400.0f);
            Zoom = 1f;
            Rotation = 0f;
            Origin = new Vector2(viewportWidth / 2f, viewportHeight / 2f);
        }
        
        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;
            
            // WASD movement
            if (keyboardState.IsKeyDown(Keys.W)) {
                movement.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.S)) {
                movement.Y += 1;
            }
            if (keyboardState.IsKeyDown(Keys.A)) {
                movement.X -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.D)) {
                movement.X += 1;
            }
            
            // Debug output to see if keys are being detected
            if (movement.X != 0 || movement.Y != 0) {
                //System.Console.WriteLine($"Camera movement: X={movement.X}, Y={movement.Y}, Position: {Position}");
            }
            
            // Store position before movement for debugging
            Vector2 oldPosition = Position;
            
            // Normalize diagonal movement
            if (movement.Length() > 0) {
                movement.Normalize();
                Position += movement * MoveSpeed * deltaTime / Zoom; // Move slower when zoomed in
            }
            
            // Optional: Clamp camera position to world bounds
            if (WorldBounds.HasValue) {
                ClampToWorldBounds();
                
                // Debug if clamping is affecting horizontal movement
                if (oldPosition != Position && movement.X != 0) {
                    //System.Console.WriteLine($"Position changed by bounds: Old={oldPosition}, New={Position}, Bounds={WorldBounds}");
                }
            }
        }
        
        public void AdjustZoom(float zoomAmount) {
            Zoom += zoomAmount * ZoomSpeed;
            Zoom = MathHelper.Clamp(Zoom, MinZoom, MaxZoom);
        }
        
        private void ClampToWorldBounds() {
            if (!WorldBounds.HasValue) return;
            
            var bounds = WorldBounds.Value;
            var halfViewWidth = (viewportWidth / 2f) / Zoom;
            var halfViewHeight = (viewportHeight / 2f) / Zoom;
            
            Position = new Vector2(
                MathHelper.Clamp(Position.X, bounds.Left + halfViewWidth, bounds.Right - halfViewWidth),
                MathHelper.Clamp(Position.Y, bounds.Top + halfViewHeight, bounds.Bottom - halfViewHeight)
            );
        }
        
        public Matrix GetTransformMatrix() {
            return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0));
        }
        
        public Matrix GetInverseTransformMatrix() {
            return Matrix.Invert(GetTransformMatrix());
        }
        
        // Convert screen position to world position (useful for mouse input)
        public Vector2 ScreenToWorld(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition, GetInverseTransformMatrix());
        }
        
        // Convert world position to screen position
        public Vector2 WorldToScreen(Vector2 worldPosition) {
            return Vector2.Transform(worldPosition, GetTransformMatrix());
        }
        
        // Check if a world rectangle is visible on screen (for culling)
        public bool IsVisible(Rectangle worldRectangle) {
            var viewRect = GetViewRectangle();
            return viewRect.Intersects(worldRectangle);
        }
        
        // Get the rectangle representing what's visible in world space
        public Rectangle GetViewRectangle() {
            var inverseTransform = GetInverseTransformMatrix();
            var topLeft = Vector2.Transform(Vector2.Zero, inverseTransform);
            var bottomRight = Vector2.Transform(new Vector2(viewportWidth, viewportHeight), inverseTransform);
            
            return new Rectangle(
                (int)Math.Floor(topLeft.X),
                (int)Math.Floor(topLeft.Y),
                (int)Math.Ceiling(bottomRight.X - topLeft.X),
                (int)Math.Ceiling(bottomRight.Y - topLeft.Y)
            );
        }
        
        // Focus camera on a specific world position
        public void FocusOn(Vector2 worldPosition) {
            Position = worldPosition;
        }
        
        // Smoothly move camera towards a target position
        public void LerpTo(Vector2 targetPosition, float lerpSpeed, float deltaTime) {
            Position = Vector2.Lerp(Position, targetPosition, lerpSpeed * deltaTime);
        }
    }
    public enum CoordinateSystem {
        World,  // Affected by camera (moves with world)
        Screen  // Fixed position (UI elements)
    }
}