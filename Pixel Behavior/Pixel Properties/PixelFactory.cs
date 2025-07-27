using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Behavior.Dusts;
using Pixel_Simulation.Pixel_Behavior.Gases;
using Pixel_Simulation.Pixel_Behavior.Liquids;
using Pixel_Simulation.Pixel_Behavior.Solids;
using Pixel_Simulation.Pixel_Behavior.Reactions.Particles;

namespace Pixel_Simulation.Pixel_Properties {
    public static class PixelFactory {       
        public static Dictionary<ElementType, Type> ColorSchemeMap = new Dictionary<ElementType, Type>() {
            #region Dust
            { ElementType.Sand, typeof(SandColorScheme) },
            { ElementType.Salt, typeof(SaltColorScheme) },
            { ElementType.CoalDust, typeof(CoalDustColorScheme) },
            { ElementType.CopperDust, typeof(CopperDustColorScheme) },
            { ElementType.Sawdust, typeof(SawdustColorScheme) },
            { ElementType.GoldDust, typeof(GoldDustColorScheme) },
            #endregion

            #region Liquid
            { ElementType.Water, typeof(WaterColorScheme) },
            { ElementType.Slime, typeof(SlimeColorScheme) },
            { ElementType.Lava, typeof(LavaColorScheme) },
            { ElementType.Acid, typeof(AcidColorScheme) },
            { ElementType.MoltenAluminium, typeof(MoltenAluminiumColorScheme) },
            { ElementType.Oil, typeof(OilColorScheme) },
            #endregion

            #region Gas
            { ElementType.Smoke, typeof(SmokeColorScheme) },
            { ElementType.Steam, typeof(SteamColorScheme) },
            { ElementType.Hydrogen, typeof(HydrogenColorScheme) },
            { ElementType.Methane, typeof(MethaneColorScheme) },
            { ElementType.AcidFumes, typeof(AcidFumesColorScheme) },
            { ElementType.Helium, typeof(HeliumColorScheme) },
            { ElementType.Propane, typeof(PropaneColorScheme) },
            #endregion

            #region Solid
            { ElementType.Stone, typeof(StoneColorScheme) },
            { ElementType.Wood, typeof(WoodColorScheme) },
            { ElementType.Titanium, typeof(TitaniumColorScheme) },
            { ElementType.Obsidian, typeof(ObsidianColorScheme) },
            { ElementType.Iron, typeof(IronColorScheme) },
            { ElementType.Polyethylene, typeof(PolyethyleneColorScheme) },
            { ElementType.Aluminium, typeof(AluminiumColorScheme) }
            #endregion
        };
        public static Pixel CreatePixel(ElementType element, int x, int y) {
            var pixelType = ElementTypeMap.GetPixelType(element);
            return (Pixel)Activator.CreateInstance(pixelType, x, y);
        }
        public static void AddPixelToGrid(int x, int y, Pixel[,] grid, ElementType element) {
            var coords = Screen.ConvertGridToScreenPosition(x, y);
            grid[x, y] = CreatePixel(element, coords.X, coords.Y);
        }
        public static object MapColorSchemes(ElementType element, string selectColor) {
            if (element == ElementType.Fire) {
                return FireColorScheme.FireColors;
            }

            if (ColorSchemeMap.TryGetValue(element, out var type)) {
                if (selectColor.Contains("Colors")) {
                    return (Color[])type.GetField(selectColor).GetValue(null); // colors array
                } else {
                    return (Color)type.GetField(selectColor).GetValue(null); // other colors
                }
            }
            return Color.White;
        }
    }
    public static class ElementTypeMap {
        public static Type GetPixelType(ElementType element) {
            return element switch {
                
                #region Dust
                ElementType.Sand => typeof(Sand),
                ElementType.Salt => typeof(Salt),
                ElementType.CoalDust => typeof(CoalDust),
                ElementType.CopperDust => typeof(CopperDust),
                ElementType.Sawdust => typeof(Sawdust),
                ElementType.GoldDust => typeof(GoldDust),
                #endregion

                #region Liquid
                ElementType.Water => typeof(Water),
                ElementType.Slime => typeof(Slime),
                ElementType.Lava => typeof(Lava),
                ElementType.Acid => typeof(Acid),
                ElementType.MoltenAluminium => typeof(MoltenAluminium),
                ElementType.Oil => typeof(Oil),
                #endregion

                #region Gas
                ElementType.Smoke => typeof(Smoke),
                ElementType.Steam => typeof(Steam),
                ElementType.Hydrogen => typeof(Hydrogen),
                ElementType.Methane => typeof(Methane),
                ElementType.AcidFumes => typeof(AcidFumes),
                ElementType.Helium => typeof(Helium),
                ElementType.Propane => typeof(Propane),
                #endregion 

                #region Solid
                ElementType.Stone => typeof(Stone),
                ElementType.Wood => typeof(Wood),
                ElementType.Titanium => typeof(Titanium),
                ElementType.Obsidian => typeof(Obsidian),
                ElementType.Iron => typeof(Iron),
                ElementType.Polyethylene => typeof(Polyethylene),
                ElementType.Aluminium => typeof(Aluminium),
                #endregion

                #region Other 
                ElementType.Fire => typeof(Fire),
                #endregion

                _ => throw new NotSupportedException($"Element type {element} is not supported.")
            };
        }
        public static bool SameType(Pixel pixel, ElementType elementType) {
            if (pixel == null) return false;
            var targetType = GetPixelType(elementType);
            return pixel.GetType() == targetType;
        }
    }
}
