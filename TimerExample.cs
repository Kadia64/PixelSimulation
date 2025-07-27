using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior;
using System;

namespace Pixel_Simulation {
    public static class TimerExample {
        
        // Example: Measure how long pixel updates take
        public static void MeasurePixelUpdatePerformance(Pixel[,] grid, PixelUpdater updater, GameTime gameTime) {
            Timer performanceTimer = new Timer();
            
            // Start timing
            performanceTimer.Start();
            
            // The code you want to measure
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            updater.PixelUpdateDirections(grid, deltaTime);
            
            // Stop timing
            performanceTimer.Stop();
            
            // Check results
            Console.WriteLine($"Pixel update took: {performanceTimer.ElapsedTime * 1000:F2}ms");
            
            // Alert if performance is poor
            if (performanceTimer.HasReached(0.016f)) { // 16ms = 60fps target
                Console.WriteLine("WARNING: Frame time exceeded 60fps target!");
            }
        }
        
        // Example: Timed event system
        public static void TimedEventExample(GameTime gameTime) {
            Timer eventTimer = new Timer(3.0f); // 3 second timer
            
            // Update the timer
            eventTimer.Update(gameTime);
            
            // Start the timer on first call
            if (!eventTimer.IsRunning && !eventTimer.HasReachedTarget) {
                eventTimer.Start();
            }
            
            // When timer reaches target, do something
            if (eventTimer.HasReachedTarget) {
                Console.WriteLine("3 seconds have passed! Triggering event...");
                
                // Reset for next cycle
                eventTimer.Reset();
            }
        }
        
        // Example: Multiple checkpoint timer
        public static void CheckpointTimerExample(GameTime gameTime) {
            Timer checkpointTimer = new Timer();
            bool hasStarted = false;
            
            if (!hasStarted) {
                checkpointTimer.Start();
                hasStarted = true;
                Console.WriteLine("Checkpoint timer started...");
            }
            
            checkpointTimer.Update(gameTime);
            
            // Check various checkpoints
            if (checkpointTimer.HasReached(1.0f)) {
                Console.WriteLine("Checkpoint 1: 1 second passed");
            }
            
            if (checkpointTimer.HasReached(2.5f)) {
                Console.WriteLine("Checkpoint 2: 2.5 seconds passed");
            }
            
            if (checkpointTimer.HasReached(5.0f)) {
                Console.WriteLine("Checkpoint 3: 5 seconds passed - Final checkpoint!");
                checkpointTimer.Reset(); // Reset for next cycle
                hasStarted = false;
            }
        }
    }
}