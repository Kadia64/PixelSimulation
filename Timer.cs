using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pixel_Simulation {
    public class Timer {
        private float elapsedTime;
        private float targetTime;
        private bool isRunning;
        private bool hasReachedTarget;
        
        public float ElapsedTime => elapsedTime;
        public float TargetTime => targetTime;
        public bool IsRunning => isRunning;
        public bool HasReachedTarget => hasReachedTarget;
        
        public Timer(float targetTime = 0f) {
            this.targetTime = targetTime;
            Reset();
        }
        
        public void Start() {
            isRunning = true;
            hasReachedTarget = false;
        }
        
        public void Stop() {
            isRunning = false;
        }
        
        public void Reset() {
            elapsedTime = 0f;
            isRunning = false;
            hasReachedTarget = false;
        }
        
        public void SetTarget(float seconds) {
            targetTime = seconds;
            hasReachedTarget = elapsedTime >= targetTime;
        }
        
        public void Update(GameTime gameTime) {
            if (!isRunning) return;
            
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (targetTime > 0 && elapsedTime >= targetTime && !hasReachedTarget) {
                hasReachedTarget = true;
                // Optionally auto-stop when target is reached
                // isRunning = false;
            }
        }
        
        // Convenience method to check if timer has reached a specific time
        public bool HasReached(float seconds) {
            return elapsedTime >= seconds;
        }
        
        // Get remaining time until target
        public float GetRemainingTime() {
            if (targetTime <= 0) return 0f;
            return Math.Max(0f, targetTime - elapsedTime);
        }
        
        // Get time as formatted string (MM:SS.ss)
        public string GetFormattedTime() {
            TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
            return $"{time.Minutes:D2}:{time.Seconds:D2}.{time.Milliseconds / 10:D2}";
        }
    }
}