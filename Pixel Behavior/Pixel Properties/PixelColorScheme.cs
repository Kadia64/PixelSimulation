using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Pixel_Simulation.Pixel_Behavior;

namespace Pixel_Simulation.Pixel_Properties {
    public class ColorSchemeImporter {
        private string filePath;
        private JObject ColorData;
        public ColorSchemeImporter(string path) {
            string projectDirectory = Directory.GetCurrentDirectory();
            for (int i = 0; i < 3; ++i) {
                projectDirectory = Directory.GetParent(projectDirectory).FullName;
            }
            this.filePath = Path.Combine(projectDirectory, path);
            this.ColorData = GetColorSchemeData();
        }        
        public void ImportDustColorSchemes() {
            JArray dustArray = (JArray)ColorData["Dust"];
            JObject dustObj = (JObject)dustArray[0];

            foreach (var property in dustObj.Properties()) {
                string elementName = property.Name; 
                JObject colorScheme = (JObject)property.Value;
                
                // Try to find matching ElementType
                if (Enum.TryParse<ElementType>(elementName, out ElementType elementType)) {
                    // Get the corresponding color scheme type from PixelFactory
                    if (PixelFactory.ColorSchemeMap.TryGetValue(elementType, out Type colorSchemeType)) {
                        SetColorSchemeFromJson(colorSchemeType, colorScheme);
                    }
                }
            }
        }
        public void ImportLiquidColorSchemes() {
            JArray dustArray = (JArray)ColorData["Liquid"];
            JObject dustObj = (JObject)dustArray[0];

            foreach (var property in dustObj.Properties()) {
                string elementName = property.Name;
                JObject colorScheme = (JObject)property.Value;

                // Try to find matching ElementType
                if (Enum.TryParse<ElementType>(elementName, out ElementType elementType)) {
                    // Get the corresponding color scheme type from PixelFactory
                    if (PixelFactory.ColorSchemeMap.TryGetValue(elementType, out Type colorSchemeType)) {
                        SetColorSchemeFromJson(colorSchemeType, colorScheme);
                    }
                }
            }
        }
        public void ImportGasColorSchemes() {
            JArray dustArray = (JArray)ColorData["Gas"];
            JObject dustObj = (JObject)dustArray[0];

            foreach (var property in dustObj.Properties()) {
                string elementName = property.Name;
                JObject colorScheme = (JObject)property.Value;

                // Try to find matching ElementType
                if (Enum.TryParse<ElementType>(elementName, out ElementType elementType)) {
                    // Get the corresponding color scheme type from PixelFactory
                    if (PixelFactory.ColorSchemeMap.TryGetValue(elementType, out Type colorSchemeType)) {
                        SetColorSchemeFromJson(colorSchemeType, colorScheme);
                    }
                }
            }
        }
        public void ImportSolidColorSchemes() {
            JArray dustArray = (JArray)ColorData["Solid"];
            JObject dustObj = (JObject)dustArray[0];

            foreach (var property in dustObj.Properties()) {
                string elementName = property.Name;
                JObject colorScheme = (JObject)property.Value;

                // Try to find matching ElementType
                if (Enum.TryParse<ElementType>(elementName, out ElementType elementType)) {
                    // Get the corresponding color scheme type from PixelFactory
                    if (PixelFactory.ColorSchemeMap.TryGetValue(elementType, out Type colorSchemeType)) {
                        SetColorSchemeFromJson(colorSchemeType, colorScheme);
                    }
                }
            }
        }
        private JObject GetColorSchemeData() {
            var data = File.ReadAllText(filePath);
            return JObject.Parse(data);
        }
        private void SetColorSchemeFromJson(Type colorSchemeType, JObject colorScheme) {
            var fields = colorSchemeType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            
            // First pass: Set individual color fields
            foreach (var field in fields) {
                if (field.FieldType == typeof(Color) && colorScheme.ContainsKey(field.Name)) {
                    Color? color = ParseColorFromJson(colorScheme[field.Name]);
                    if (color.HasValue) {
                        field.SetValue(null, color.Value);
                    }
                }
                else if (field.FieldType == typeof(Color?) && colorScheme.ContainsKey(field.Name)) {
                    Color? color = ParseColorFromJson(colorScheme[field.Name]);
                    if (color.HasValue) {
                        field.SetValue(null, color.Value);
                    }
                }
            }
            
            // Second pass: Update color arrays after all individual colors are set
            foreach (var field in fields) {
                if (field.FieldType == typeof(Color[]) && field.Name.EndsWith("Colors")) {
                    UpdateColorArray(colorSchemeType, field);
                }
            }
        }
        private void UpdateColorArray(Type colorSchemeType, System.Reflection.FieldInfo colorArrayField) {
            var colorFields = colorSchemeType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => (f.FieldType == typeof(Color) || f.FieldType == typeof(Color?)) && 
                           f.Name != "DisplayColor" && f.Name != "ElementName")
                .OrderBy(f => GetColorOrder(f.Name)) // Order by importance: Light, Medium, Dark, ExtraDark
                .ToList();

            List<Color> colors = new List<Color>();
            foreach (var field in colorFields) {
                object value = field.GetValue(null);
                
                if (field.FieldType == typeof(Color)) {
                    Color color = (Color)value;
                    if (color != default(Color)) {
                        colors.Add(color);
                    }
                }
                else if (field.FieldType == typeof(Color?) && value != null) {
                    Color color = (Color)value;
                    if (color != default(Color)) {
                        colors.Add(color);
                    }
                }
            }

            if (colors.Count > 0) {
                colorArrayField.SetValue(null, colors.ToArray());
            }
        }
        private int GetColorOrder(string fieldName) {
            return fieldName switch {
                "Light" => 1,
                "Medium" => 2,
                "Dark" => 3,
                "ExtraDark" => 4,
                _ => 99 // Unknown fields go last
            };
        }
        private Color? ParseColorFromJson(JToken colorToken) {
            try {
                if (colorToken is JArray colorArray) {
                    // Handle array format: [255, 128, 64]
                    int[] colorValues = colorArray.Select(token => (int)token).ToArray();
                    if (colorValues.Length >= 3) {
                        return new Color(colorValues[0], colorValues[1], colorValues[2]);
                    }
                }
                else if (colorToken is JValue colorValue) {
                    // Handle single value format (hex string, etc.)
                    string colorString = colorValue.ToString();
                    if (colorString.StartsWith("#") && colorString.Length == 7) {
                        // Parse hex color: #FF8040
                        int r = Convert.ToInt32(colorString.Substring(1, 2), 16);
                        int g = Convert.ToInt32(colorString.Substring(3, 2), 16);
                        int b = Convert.ToInt32(colorString.Substring(5, 2), 16);
                        return new Color(r, g, b);
                    }
                }
            }
            catch (Exception ex) {
                // Log error or handle gracefully
                Console.WriteLine($"Error parsing color from JSON: {ex.Message}");
            }
            return null;
        }        
    }
    
    #region Dusts
    public struct SandColorScheme {
        public static readonly ElementType ElementName = ElementType.Sand;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] SandColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct SaltColorScheme {
        public static readonly ElementType ElementName = ElementType.Salt;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] SaltColors = new Color[] {
            Light, Medium
        };
    }
    public struct CoalDustColorScheme {
        public static readonly ElementType ElementName = ElementType.CoalDust;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] CoalDustColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct CopperDustColorScheme {
        public static readonly ElementType ElementName = ElementType.CopperDust;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] CopperDustColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct SawdustColorScheme {
        public static readonly ElementType ElementName = ElementType.Sawdust;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] SawdustColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct GoldDustColorScheme {
        public static readonly ElementType ElementName = ElementType.GoldDust;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] GoldDustColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct GunpowderColorScheme {
        public static readonly ElementType ElementName = ElementType.Gunpowder;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] GunpowderColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    };
    #endregion

    #region Liquids
    public struct WaterColorScheme {
        public static readonly ElementType ElementName = ElementType.Water;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] WaterColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }    
    public struct SlimeColorScheme {
        public static readonly ElementType ElementName = ElementType.Slime;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] SlimeColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct LavaColorScheme {
        public static readonly ElementType ElementName = ElementType.Lava;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] LavaColors = new Color[] {
            Light, Medium,
        };
    }
    public struct AcidColorScheme {
        public static readonly ElementType ElementName = ElementType.Acid;
        public static Color DisplayColor;
        public static Color Light;
        public static Color? Medium = null;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] AcidColors = new Color[] {
            Light,
        };
    }
    public struct MoltenAluminiumColorScheme {
        public static readonly ElementType ElementName = ElementType.MoltenAluminium;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] MoltenAluminiumColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct OilColorScheme {
        public static readonly ElementType ElementName = ElementType.Oil;
        public static Color DisplayColor;
        public static Color? Light = null;
        public static Color? Medium = null;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] OilColors = new Color[] {
            Dark, ExtraDark
        };
    }
    #endregion

    #region Gases
    public struct SmokeColorScheme {
        public static readonly ElementType ElementName = ElementType.Smoke;
        public static Color DisplayColor = Color.Gray;
        public static Color? Light = null;
        public static Color? Medium = null;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] SmokeColors = new Color[] {
            Dark, ExtraDark
        };
    }
    public struct SteamColorScheme {
        public static readonly ElementType ElementName = ElementType.Steam;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] SteamColors = new Color[] {
            Light, Medium
        };
    }
    public struct HydrogenColorScheme {
        public static readonly ElementType ElementName = ElementType.Hydrogen;
        public static Color DisplayColor;
        public static Color Light;
        public static Color? Medium = null;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] HydrogenColors = new Color[] {
            Light
        };
    }
    public struct MethaneColorScheme {
        public static readonly ElementType ElementName = ElementType.Methane;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] MethaneColors = new Color[] {
            Light, Medium
        };
    }   
    public struct AcidFumesColorScheme {
        public static readonly ElementType ElementName = ElementType.AcidFumes;
        public static Color DisplayColor;
        public static Color Light;
        public static Color? Medium = null;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] AcidFumesColors = new Color[] {
            Light
        };
    }
    public struct PropaneColorScheme {
        public static readonly ElementType ElementName = ElementType.Propane;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] PropaneColors = new Color[] {
            Light, Medium
        };
    }
    public struct HeliumColorScheme {
        public static readonly ElementType ElementName = ElementType.Helium;
        public static Color DisplayColor;
        public static Color Light;
        public static Color? Medium = null;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] HeliumColors = new Color[] {
            Light
        };
    }
    #endregion

    #region Solids
    public struct StoneColorScheme {
        public static readonly ElementType ElementName = ElementType.Stone;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] StoneColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct WoodColorScheme {
        public static readonly ElementType ElementName = ElementType.Wood;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] WoodColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct TitaniumColorScheme {
        public static readonly ElementType ElementName = ElementType.Titanium;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] TitaniumColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct ObsidianColorScheme {
        public static readonly ElementType ElementName = ElementType.Obsidian;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] ObsidianColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct IronColorScheme {
        public static readonly ElementType ElementName = ElementType.Iron;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] IronColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }
    public struct PolyethyleneColorScheme {
        public static readonly ElementType ElementName = ElementType.Polyethylene;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color? Dark = null;
        public static Color? ExtraDark = null;
        public static Color[] PolyethyleneColors = new Color[] {
            Light, Medium
        };
    }
    public struct AluminiumColorScheme {
        public static readonly ElementType ElementName = ElementType.Aluminium;
        public static Color DisplayColor;
        public static Color Light;
        public static Color Medium;
        public static Color Dark;
        public static Color ExtraDark;
        public static Color[] AluminiumColors = new Color[] {
            Light, Medium, Dark, ExtraDark
        };
    }

    #endregion

    #region Other
    public struct FireColorScheme {
        public static readonly ElementType ElementName = ElementType.Fire;
        public static readonly Color DisplayColor = new Color(255, 133, 0);
        public static readonly Color Light1 = new Color(255, 177, 0);
        public static readonly Color Light2 = new Color(236, 164, 0);
        public static readonly Color Medium1 = new Color(255, 155, 0);
        public static readonly Color Medium2 = new Color(242, 147, 0);
        public static readonly Color Dark1 = new Color(255, 133, 0);
        public static readonly Color Dark2 = new Color(234, 122, 0);
        public static readonly Color ExtraDark1 = new Color(255, 89, 0);
        public static readonly Color ExtraDark2 = new Color(236, 83, 0);
        public static readonly Color Ash1 = new Color(96, 94, 94);
        public static readonly Color Ash2 = new Color(226, 226, 226);
        public static readonly Color[] FireColors = new Color[] {
            Light1, Light2, Medium1, Medium2, Dark1, Dark2, ExtraDark1, ExtraDark2
        };
    }
    #endregion
}
