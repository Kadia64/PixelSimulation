using System;

namespace Pixel_Simulation {
    public static class RandomEngine {

        public static Random random = new Random();

        public static bool Flip() {
            return random.Next(2) == 0;
        }
        public static int GetNumber(int low = 0, int high = 1) {
            if (high != 1) {
                return random.Next(low, high);
            } else {
                return random.Next(low);
            }
        }
        public static float GetDouble() {
            return (float)random.NextDouble();
        }
        public static bool RandomChance(int range) {
            return random.Next(range) == range / 2;
        }
        public static bool RandomChance(int range, int multiplier) {
            return random.Next(range * multiplier) == range / 2;
        }
        
        // Weighted random around a center point (normal distribution)
        public static int GetWeightedRandom(float low, float high, float center) {
            // Generate two uniform random numbers
            float u1 = (float)random.NextDouble();
            float u2 = (float)random.NextDouble();
            
            // Box-Muller transform to create normal distribution
            float standardNormal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2));
            
            // Scale and shift to fit our range around the center
            float range = high - low;
            float normalizedCenter = (center - low) / range;
            
            // Standard deviation controls how "tight" the distribution is around center
            float standardDeviation = 0.25f; // Adjust this to control spread
            
            // Apply the normal distribution
            float normalizedResult = normalizedCenter + (standardNormal * standardDeviation);
            
            // Clamp to ensure we stay within bounds
            normalizedResult = Math.Max(0f, Math.Min(1f, normalizedResult));
            
            // Scale back to original range
            return (int)(low + (normalizedResult * range));
        }
        
        // Simpler weighted random with customizable bias strength
        public static float GetBiasedRandom(float low, float high, float center, float bias = 2.0f) {
            float random1 = GetDouble();
            float random2 = GetDouble();
            
            // Average multiple random numbers to create bias toward center
            float biasedRandom = random1;
            for (int i = 1; i < bias; i++) {
                biasedRandom = (biasedRandom + GetDouble()) / 2f;
            }
            
            // Map to range around center
            float range = high - low;
            float normalizedCenter = (center - low) / range;
            
            // Blend between uniform and biased distribution
            float result = (biasedRandom * 0.3f) + (normalizedCenter * 0.7f);
            
            // Add some controlled randomness back
            result += (GetDouble() - 0.5f) * 0.4f;
            
            // Clamp and scale
            result = Math.Max(0f, Math.Min(1f, result));
            return low + (result * range);
        }
        
        // Triangular distribution - simpler but effective
        public static float GetTriangularRandom(float low, float high, float center) {
            float u = GetDouble();
            float range = high - low;
            float centerNorm = (center - low) / range;
            
            if (u < centerNorm) {
                return low + (float)Math.Sqrt(u * centerNorm) * range;
            } else {
                return high - (float)Math.Sqrt((1 - u) * (1 - centerNorm)) * range;
            }
        }
    }
}
