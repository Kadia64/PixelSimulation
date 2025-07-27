using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pixel_Simulation.Climate {
    public static class TempColorsMapper {
        // use linear interoplation instead of color mapping
        private static bool Write = false;

        public static Texture2D _texture;        
        public static Color[,] ColorMap = new Color[Screen.GridWidth, Screen.GridHeight];       
        public static Dictionary<int, Color> LookupTable = new Dictionary<int, Color>();
        public static Dictionary<int, int> PossibleTemperatures = new Dictionary<int, int>();
        public static Dictionary<int, Color> TemporaryTable = new Dictionary<int, Color>();
        private static List<Color> ColorList = new List<Color>();
        private static int Transparency = 10;

        public static void BuildTable() {
            Globals._tempColorPixel = new Texture2D(Globals._graphicsDevice, 1, 1);
            Globals._tempColorPixel.SetData<Color>(new Color[] { Color.White });
            _texture = Globals._tempColorPixel;

            GetPurpleColors();
            GetBlueColors();
            GetGreenColors();
            GetOrangeColors();
            GetRedColors();
            GetPinkColors();

            // below freezing: -492-1
            // cold: 0-48
            // lukewarm: 49-70
            // warm: 71-82
            // hot: 83-128
            // boiling: 129-1024
            // melting: 1025-4104
            // hot asf: 4105-6921

            var temperatures = LookupTable.Keys.ToList();
            var colors = LookupTable.Values.ToList();

            int stored = 0;
            for (int i = -496; i <= 6921; ++i) {
                int value = 0;

                if (i < 0) {
                    if (i % 4 == 0) {
                        stored = i;
                    }
                } else if (i >= 0 && i <= 128) {
                    stored = i;
                } else if (i >= 129 && i <= 1024) {
                    if (i % 8 == 0) {
                        stored = i;
                    }
                } else if (i >= 1025 && i <= 1656) {
                    if (i % 9 == 0) {
                        stored = i;
                    }
                } else if (i >= 1657 && i <= 4104) {
                    if (i % 9 == 0) {
                        stored = i;
                    }
                } else if (i >= 4105 && i <= 4473) {
                    if (i % 9 == 0) {
                        stored = i;
                    }
                } else if (i >= 4474 && i <= 6921) {
                    if (i % 9 == 0) {
                        stored = i;
                    }
                }

                value = stored;

                PossibleTemperatures.Add(i, value);
            }

            var temp_keys = PossibleTemperatures.Keys.ToList();
            var temp_values = PossibleTemperatures.Values.ToList();

            var table_keys = LookupTable.Keys.ToList();
            var table_values = LookupTable.Values.ToList();
        }

        public static void DrawThermometer() {
            int pixelSize = 2;
            int thickness = 20;
            int _startX_ = 10;
            int _startY_ = 10;
            int startX = _startX_;
            int startY = _startY_;

            for (int i = 0; i < ColorList.Count; ++i) {
                var color = ColorList[i];
                var newColor = new Color(color.R, color.G, color.B, (byte)255);
                ColorList[i] = newColor;

                for (int y = 0; y < thickness; ++y) {
                    startY += 1;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    Globals._spriteBatch.Draw(_texture, pixel, ColorList[i]);
                }
                startX += pixelSize;
                startY = _startY_;
            }

            int yValue = thickness + startY + 5;
            Debug.Write("-492 - (-1)", 120, yValue);
            Debug.Write("0 - 48", 290, yValue);
            Debug.Write("49 - 70", 362, yValue);
            Debug.Write("71 - 82", 397, yValue + 20);
            Globals._shapes.DrawVerticalLine(new Point(412, yValue), new Point(412, yValue + 20), Color.White);

            Debug.Write("83 - 128", 445, yValue);
            Debug.Write("129 - 1024", 590, yValue);
            Debug.Write("1025 - 4104", 980, yValue);
            Debug.Write("4105 - 6921", 1750, yValue);
        }
        private static Dictionary<int, Color> ReverseKeys(Dictionary<int, Color> dict) {
            var keys = dict.Keys.ToList();
            List<Color> values = dict.Values.ToList();
            int Length = keys.Count;

            for (int i = 0; i < Length; ++i) {
                dict.Remove(keys[i]);
            }

            keys.Reverse();

            for (int i = 0; i < Length; ++i) {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }
        private static Dictionary<TKey, TValue> JoinDictionaries<TKey, TValue>(params Dictionary<TKey, TValue>[] dictionaries) {
            var merged = new Dictionary<TKey, TValue>();

            foreach (var dictionary in dictionaries) {
                foreach (var kvp in dictionary) {
                    merged[kvp.Key] = kvp.Value;
                }
            }

            return merged;
        }

        #region Color Ranges
        // -492 - 0   -> 123 passes, 123/492 = 4 degrees per color
        // 4 per degree
        // total range: -492 - 0 (4 * 123)
        public static void GetPurpleColors() { 
            int startX = 5;
            int startY = 100;
            
            int pixelSize = 2;
            int r = 30;
            int g = 0;
            int b = 70;

            bool addB = false;
            int iterations = 1;

            for (int y = 0; y < 500; ++y) {
                int add_r = 0;
                int add_g = 0;

                if (addB) {
                    add_r = 5;
                    add_g = 10;
                }
                var color = new Color(r + add_r, g + add_g, b, Transparency);
                int temp = iterations;
                while (true) {
                    if (temp % 4 != 0) {
                        temp += 1;
                    } else {
                        TemporaryTable.Add(temp * -1, color);
                        ColorList.Add(color);
                        if (Write) Console.WriteLine(temp * -1);
                        break;
                    }
                }

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }

                startY += pixelSize;
                startX = 5;

                r += 1;

                addB = !addB;

                if (!addB) {
                    b += 3;
                }
                if (b > 255) {
                    break;
                }

                iterations += 4;                
            }
            TemporaryTable = ReverseKeys(TemporaryTable);
            LookupTable = JoinDictionaries(LookupTable, TemporaryTable);
            TemporaryTable.Clear();
            //LookupTable = LookupTable.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        // 1 - 48:21 -> 69
        // 1 per degree
        // total range: 1 - 49
        // 1 per degree
        // total range: 50 - 69
        public static void GetBlueColors() { 
            int startX = 5;
            int startY = 100;

            int pixelSize = 2;

            int r = 0;
            int g = 0;
            int b = 110;
            int iterations1 = 0;
            int iterations2 = 49;

            // blue 48
            if (Write) Console.WriteLine();
            for (int i = 0; i < 400; ++i) {                

                var color = new Color(r, g, b, Transparency);
                LookupTable.Add(iterations1, color);
                ColorList.Add(color);
                if (Write) Console.WriteLine(iterations1);

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;

                b += 3;


                if (b > 255) {
                    break;
                }
                ++iterations1;
            }

            r = 148;
            g = 170;
            // cyan 21
            if (Write) Console.WriteLine();
            for (int i = 0; i < 400; ++i) {

                var color = new Color(r, g, b, Transparency);
                LookupTable.Add(iterations2, color);
                ColorList.Add(color);
                if (Write) Console.WriteLine(iterations2);

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;

                g += 4;
                b += 3;

                if (g > 255) {
                    break;
                }

                ++iterations2;
            }
            //Console.WriteLine($"{iterations1}: {iterations2}");
        }

        // 70 - 11:45 -> 126
        // 1 per degree
        // total: 70 - 81
        // 1 per degree
        // total range: 82 - 126
        public static void GetGreenColors() {
            int startX = 5;
            int startY = 100;

            int pixelSize = 2;

            int r = 0;
            int g = 160;
            int b = 0;
            int iterations1 = 71;
            int iterations2 = 83;

            // green 11
            if (Write) Console.WriteLine();
            for (int i = 0; i < 400; ++i) {

                var color = new Color(r, g, b, Transparency);
                LookupTable.Add(iterations1, color);
                ColorList.Add(color);
                if (Write) Console.WriteLine(iterations1);

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;

                g += 5;

                if (g > 218) {
                    break;
                }
                ++iterations1;
            }

            r = 255;
            g = 240;
            b = 150;
            // yellow 45
            if (Write) Console.WriteLine();
            for (int i = 0; i < 400; ++i) {

                var color = new Color(r, g, b, Transparency);
                LookupTable.Add(iterations2, color);
                ColorList.Add(color);
                if (Write) Console.WriteLine(iterations2);

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;

                if (i % 6 == 0) {
                    g -= 1;
                } else {
                    b -= 4;
                }

                if (b < 0) {
                    break;
                }
                ++iterations2;
            }
            //Console.WriteLine($"{iterations1}: {iterations2}");
        }

        // 111 passes -> goal: 127 - 1000, difference: 1000 - 127 = 873
        // 873/111 = 8 degrees per color
        // multiplier: 8 * 111 = 888
        // final: 127 + 888 = 1015
        // total range: 127 - 1015
        public static void GetOrangeColors() { 
            int startX = 5;
            int startY = 100;

            int pixelSize = 2;
            int r = 200;
            int g = 50;
            int b = 0;
            bool addB = false;

            int iterations = 129;

            // orange 111
            if (Write) Console.WriteLine();
            for (int y = 0; y < 400; ++y) {

                int add_b = 0;
                int add_g = 0;

                if (addB) {
                    add_g = 3;
                    add_b = 2;
                }
                var color = new Color(r, g + add_g, b + add_b, Transparency);
                int temp = iterations;
                while (true) {
                    if (temp % 8 != 0) {
                        temp += 1;
                    } else {
                        TemporaryTable.Add(temp, color);
                        ColorList.Add(color);
                        if (Write) Console.WriteLine(temp);
                        break;
                    }
                }

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;

                addB = !addB;

                if (!addB) {
                    r += 1;
                    g += 1;
                }

                if (r > 255) {
                    break;
                }
                iterations += 8;
            }
            TemporaryTable = ReverseKeys(TemporaryTable);
            LookupTable = JoinDictionaries(LookupTable, TemporaryTable);
            TemporaryTable.Clear();
        }

        // 271 + 70 = 341 passes -> goal: 1016 - 4000, difference: 4000 - 1016 = 2984
        // 2984/341 = 9 degrees per color
        // multiplier: 9 * 341 = 3069
        // final: 1016 + 3069 = 4085
        // total range: 1016 - 4085
        public static void GetRedColors() { 
            int startX = 5;
            int startY = 100;

            int pixelSize = 2;
            int r = 255;
            int g = 70;
            int b = 70;
            bool addB = false;

            int iterations1 = 1025;
            int iterations2 = 1657;

            // bright red 70
            if (Write) Console.WriteLine();
            for (int y = 0; y < 400; ++y) {

                int add_b = 0;
                int add_g = 0;

                if (addB) {
                    add_g = 3;
                    add_b = 3;
                }
                var color = new Color(r, g - add_g, b - add_b, Transparency);
                int temp = iterations1;
                while (true) {
                    if (temp % 9 != 0) {
                        temp += 1;
                    } else {                        
                        LookupTable.Add(temp, color);
                        ColorList.Add(color);
                        if (Write) Console.WriteLine(temp);
                        break;
                    }
                }

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;


                addB = !addB;

                if (addB) {
                    g -= 2;
                    b -= 2;
                }

                if (g < 0) {
                    break;
                }
                iterations1 += 9;
            }

            r = 255;
            g = 0;
            b = 0;

            // dark red 271
            if (Write) Console.WriteLine();
            for (int y = 0; y < 400; ++y) {

                int add_b = 0;
                int add_g = 0;

                if (addB) {
                    add_g = 4;
                    add_b = 4;
                }
                var color = new Color(r, g + add_g, b + add_b, Transparency);
                int temp = iterations2;
                while (true) {
                    if (temp % 9 != 0) {
                        temp += 1;
                    } else {
                        LookupTable.Add(temp, color);
                        ColorList.Add(color);
                        if (Write) Console.WriteLine(temp);
                        break;
                    }
                }


                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;


                addB = !addB;

                if (addB) {
                    r -= 1;
                }


                if (r < 120) {
                    break;
                }
                iterations2 += 9;
            }
            //Console.WriteLine($"{iterations1}: {iterations2}");
        }

        // 271 + 40 = 311 passes -> goal: 4086 - 7000, difference: 7000 - 4086 = 2914
        // 2914/311 = 9 degrees per color
        // multiplier: 9 * 311 = 2799
        // final: 4086 + 2799 = 6885
        // total range: 4086 - 6885
        public static void GetPinkColors() {
            int startX = 5;
            int startY = 100;

            int pixelSize = 2;
            int r = 255;
            int g = 80;
            int b = 140;
            bool addB = false;

            int iterations1 = 4105;
            int iterations2 = 4474;

            // bright pink 70
            if (Write) Console.WriteLine();
            for (int y = 0; y < 400; ++y) {

                int add_r = 0;
                int add_g = 0;
                int add_b = 0;

                if (addB) {
                    add_r = 4;
                    add_g = 1;
                    add_b = 3;
                }
                var color = new Color(r - add_r, g - add_g, b - add_b, Transparency);
                int temp = iterations1;
                while (true) {
                    if (temp % 9 != 0) {
                        temp += 1;
                    } else {
                        LookupTable.Add(temp, color);
                        ColorList.Add(color);
                        if (Write) Console.WriteLine(temp);
                        break;
                    }
                }


                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;

                addB = !addB;

                if (addB) {
                    g -= 4;
                    b -= 2;
                }


                if (g < 0) {
                    break;
                }
                iterations1 += 9;
            }

            r = 255;
            g = 0;
            b = 89;

            // dark pink 271
            if (Write) Console.WriteLine();
            for (int y = 0; y < 400; ++y) {

                int add_b = 0;
                int add_g = 0;

                if (addB) {
                    add_g = 4;
                    add_b = 4;
                }
                var color = new Color(r, g + add_g, b + add_b, Transparency);
                int temp = iterations2;
                while (true) {
                    if (temp % 9 != 0) {
                        temp += 1;
                    } else {
                        LookupTable.Add(temp, color);
                        ColorList.Add(color);
                        if (Write) Console.WriteLine(temp);
                        break;
                    }
                }

                for (int x = 0; x < 10; ++x) {
                    startX += pixelSize;
                    Rectangle pixel = new Rectangle(startX, startY, pixelSize, pixelSize);
                    //Globals._spriteBatch.Draw(_texture, pixel, color);
                }
                startY += pixelSize;
                startX = 5;


                addB = !addB;

                if (addB) {
                    r -= 1;
                }

                if (r < 120) {
                    break;
                }
                iterations2 += 9;
            }
            //Console.WriteLine($"{iterations1}: {iterations2}");
        }
        #endregion

        public static float FluctuateTemperature() {
            float overallTemp = Globals.GlobalTemperature;

            float MIN = Globals.TemperatureFluctuation;
            float MAX = Globals.TemperatureFluctuation;

            float minRange = RandomEngine.GetNumber(0, (int)MIN);
            float maxRange = RandomEngine.GetNumber(0, (int)MAX);
            float minimumRange = Globals.GlobalTemperature - (minRange);
            float maximumRange = Globals.GlobalTemperature + (maxRange);

            bool sign = RandomEngine.GetNumber(2) == 0;

            return (sign) ? minimumRange : maximumRange;
        }
        
        private static List<Vector2i> circleOffsets = new List<Vector2i>();
        public static int Radius = 20;
        public static void PreCalculateCircleOffsets(int radius) {
            circleOffsets.Clear();

            // Check every position in a square around the center
            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    float distance = (float)Math.Sqrt(x * x + y * y);

                    // Only keep positions that are inside the circle
                    if (distance <= radius) {
                        circleOffsets.Add(new Vector2i(x, y));
                    }
                }
            }
        }
        public static void ApplyCircularHeat(int centerX, int centerY, float maxTemperature, int radius, Pixel[,] grid, ElementType hotElement) {
            foreach (var offset in circleOffsets) {
                int x = centerX + offset.X;
                int y = centerY + offset.Y;
   
                // Bounds checking
                if (x < 0 || x >= Screen.GridWidth || y < 0 || y >= Screen.GridHeight)
                    continue;

                // Calculate distance for temperature falloff
                float distance = (float)Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
                float temperature = CalculateTemperatureByDistance(distance, maxTemperature, Radius);                  

                if (!ElementTypeMap.SameType(grid[x, y], hotElement)) {
                    Globals.TemperatureMap[x, y] = Math.Max(Globals.TemperatureMap[x, y], temperature);
                }
            }
        }
        private static float CalculateTemperatureByDistance(float distance, float maxTemp, int radius) {
            if (distance == 0) return maxTemp; // Center pixel

            // Linear decrease: 100% at center, 0% at edge
            float falloffFactor = 1.0f - (distance / radius);
            return maxTemp * falloffFactor;
        }

        public static void AdjustTemperature(int x, int y) {
            if (Globals._FRAME_COUNTER_ % 20 != 0) return;

            int rate = 40;
            if (Globals.TemperatureMap[x, y] != Globals.InitialTemperatureMap[x, y]) {

                if (Globals.TemperatureMap[x, y] - rate < Globals.InitialTemperatureMap[x, y]) {
                    Globals.TemperatureMap[x, y] = Globals.InitialTemperatureMap[x, y];
                } else {
                    Globals.TemperatureMap[x, y] -= rate;
                }                

            }
        }

        private static int tempFrameCounter = 0;
        private const int Freq = 10;
        public static void ComputeTemperatureMap(Pixel[,] grid) {
            ++tempFrameCounter;
            if (tempFrameCounter % Freq == 0) {
                return;
            }

            for (int x = 0; x < Screen.GridWidth; x++) {
                for (int y = 0; y < Screen.GridHeight; y++) {
                    
                    AdjustTemperature(x, y);

                    float temperatureLevel = Globals.TemperatureMap[x, y];
                    int mappedMultiple = PossibleTemperatures[(int)temperatureLevel];
                    ColorMap[x, y] = LookupTable[mappedMultiple];
                }
            }
        }

        public static void DrawTemperatureMap(Pixel[,] grid) {
            for (int x = 0; x < Screen.GridWidth; x++) {
                for (int y = 0; y < Screen.GridHeight; y++) {
                    var rect = new Rectangle(x * 2, y * 2, 2, 2);

                    if (grid[x, y] == null) {
                        Globals._spriteBatch.Draw(_texture, rect, ColorMap[x, y]);
                    }
                }
            }
        }
    }
}
