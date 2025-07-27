using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Pixel_Simulation.Pixel_Behavior;
using Pixel_Simulation.Pixel_Properties;
using Pixel_Simulation.Menu.MenuComponents;
using Pixel_Simulation.Menu.Pixel_Selection;

namespace Pixel_Simulation.Menu {
    public static class PixelSelectionMenu {


        public static List<Button> ElementButtons1 = new List<Button>();
        public static List<Button> ElementButtons2 = new List<Button>();
        public static List<Button> MenuSwitchButtons = new List<Button>();
        public static List<Button> PaintScaleButtons = new List<Button>();
        public static List<Button> ExtraElementButtons = new List<Button>();
        public static List<Button> MiscButtons = new List<Button>();
        public static ElementType SelectedElement = ElementType.Sand;
        public static PaintScale SelectedPaintScale = PaintScale.Small;
        public static int ElementMenuIndex = 0;
        public static int ElementMenuCount = 1;

        private static int StartX = MenuManager.MenuDimensions.X;
        private static int StartY = MenuManager.MenuDimensions.Y;
        
        private static Rectangle[] GetElementButtonPositions1() {
            int col1 = 10, col2 = 90, col3 = 170;
            int row1 = 30, row2 = 60, row3 = 90, row4 = 120, row5 = 150, row6 = 180, row7 = 210, row8 = 240;
            int w = 70, h = 20;
            return new Rectangle[] {
                new Rectangle(StartX + col1, StartY + row1, w, h),  // Wood
                new Rectangle(StartX + col1, StartY + row2, w, h),  // Stone
                new Rectangle(StartX + col1, StartY + row3, w, h),  // Sand
                new Rectangle(StartX + col1, StartY + row4, w, h),  // Coal
                new Rectangle(StartX + col1, StartY + row5, w, h),  // Water
                new Rectangle(StartX + col1, StartY + row6, w, h),  // Lava
                new Rectangle(StartX + col1, StartY + row7, w, h),  // Smoke
                new Rectangle(StartX + col1, StartY + row8, w, h),  // Steam

                new Rectangle(StartX + col2, StartY + row1, w, h),  // Titanium
                new Rectangle(StartX + col2, StartY + row2, w, h),  // Obsidian
                new Rectangle(StartX + col2, StartY + row3, w, h),  // Salt
                new Rectangle(StartX + col2, StartY + row4, w, h),  // Copper
                new Rectangle(StartX + col2, StartY + row5, w, h),  // Slime
                new Rectangle(StartX + col2, StartY + row6, w, h),  // Acid
                new Rectangle(StartX + col2, StartY + row7, w, h),  // Methane
                new Rectangle(StartX + col2, StartY + row8, w, h),  // Hydrogen

                new Rectangle(StartX + col3, StartY + row1, w, h),  // Iron
                new Rectangle(StartX + col3, StartY + row2, w, h),  // Polyethylene
                new Rectangle(StartX + col3, StartY + row3, w, h),  // Gold
                new Rectangle(StartX + col3, StartY + row4, w, h),  // Sawdust
                new Rectangle(StartX + col3, StartY + row5, w, h),  // Molten Aluminium
                new Rectangle(StartX + col3, StartY + row6, w, h),  // Oil
                new Rectangle(StartX + col3, StartY + row7, w, h),  // Helium
                new Rectangle(StartX + col3, StartY + row8, w, h),  // Propane

                //new Rectangle(StartX + 20, StartY + 180, 60, 20), // Erase
            };
        }
        private static Rectangle[] GetElementButtonPositions2() {
            int col1 = 10, col2 = 90, col3 = 170;
            int row1 = 30, row2 = 60, row3 = 90, row4 = 120, row5 = 150, row6 = 180, row7 = 210, row8 = 240;
            int w = 70, h = 20;
            return new Rectangle[] {
                new Rectangle(StartX + col1, StartY + row1, w, h),  // Aluminium
            };
        }

        private static Rectangle[] GetMenuSwitchButtonPositions() {
            return new Rectangle[] {
                new Rectangle(StartX + 160, StartY + 5, 20, 20),
                new Rectangle(StartX + 190, StartY + 5, 20, 20),
            };
        }
        private static Rectangle[] GetPaintScaleButtonPositions() {
            return new Rectangle[] {
                new Rectangle(StartX + 40, StartY + 340, 20, 20),
                new Rectangle(StartX + 40, StartY + 365, 30, 30),
                new Rectangle(StartX + 40, StartY + 400, 40, 40),
                new Rectangle(StartX + 40, StartY + 445, 50, 50)
            };
        }
        private static Rectangle[] GetExtraElementButtonPositions() {
            return new Rectangle[] {
                new Rectangle(StartX + 20, StartY + 275, 40, 20)
            };
        }
        private static Rectangle[] GetMiscButtonPositions() {
            return new Rectangle[] {
                new Rectangle(StartX + 40, StartY + 600, 110, 20),
                new Rectangle(StartX + 40, StartY + 630, 110, 20),
                new Rectangle(StartX + 40, StartY + 660, 110, 20)
            };
        }

        public static void CreateButtons() {
            var elementPositions1 = GetElementButtonPositions1();
            var elementPositions2 = GetElementButtonPositions2();
            var extraElementPositions = GetExtraElementButtonPositions();
            var paintScalePositions = GetPaintScaleButtonPositions();
            var miscPositions = GetMiscButtonPositions();
            var menuSwitchPositions = GetMenuSwitchButtonPositions();

            #region elements 1
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[0],
                text: "Wood",
                selection: ElementType.Wood,
                color: WoodColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[1],
                text: "Stone",
                selection: ElementType.Stone,
                color: StoneColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[2],
                text: "Sand",
                selection: ElementType.Sand,
                color: SandColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[3],
                text: "Coal",
                selection: ElementType.CoalDust,
                color: CoalDustColorScheme.DisplayColor,
                textColor: Color.White
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[4],
                text: "Water",
                selection: ElementType.Water,
                color: WaterColorScheme.DisplayColor,
                textColor: Color.White
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[5],
                text: "Lava",
                selection: ElementType.Lava,
                color: LavaColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[6],
                text: "Smoke",
                selection: ElementType.Smoke,
                color: SmokeColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[7],
                text: "Steam",
                selection: ElementType.Steam,
                color: SteamColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[8],
                text: "Titanium",
                selection: ElementType.Titanium,
                color: TitaniumColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[9],
                text: "Obsidian",
                selection: ElementType.Obsidian,
                color: ObsidianColorScheme.DisplayColor,
                textColor: Color.White
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[10],
                text: "Salt",
                selection: ElementType.Salt,
                color: SaltColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[11],
                text: "Copper",
                selection: ElementType.CopperDust,
                color: CopperDustColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[12],
                text: "Slime",
                selection: ElementType.Slime,
                color: SlimeColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[13],
                text: "Acid",
                selection: ElementType.Acid,
                color: Color.Lime
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[14],
               text: "Methane",
               selection: ElementType.Methane,
               color: MethaneColorScheme.DisplayColor,
               textColor: Color.White
           ));
            ElementButtons1.Add(new Button(
                buttonDimensions: elementPositions1[15],
                text: "Hydrogen",
                selection: ElementType.Hydrogen,
                color: HydrogenColorScheme.DisplayColor,
                textColor: Color.White
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[16],
               text: "Iron",
               selection: ElementType.Iron,
               color: IronColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[17],
               text: "Polyethylene",
               selection: ElementType.Polyethylene,
               color: PolyethyleneColorScheme.DisplayColor,
               textClickedColor: Color.Black
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[18],
               text: "Gold",
               selection: ElementType.GoldDust,
               color: GoldDustColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[19],
               text: "Sawdust",
               selection: ElementType.Sawdust,
               color: SawdustColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[20],
               text: "Aluminium",
               selection: ElementType.MoltenAluminium,
               color: MoltenAluminiumColorScheme.DisplayColor
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[21],
               text: "Oil",
               selection: ElementType.Oil,
               color: OilColorScheme.DisplayColor,
               textColor: Color.White
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[22],
               text: "Helium",
               selection: ElementType.Helium,
               color: HeliumColorScheme.DisplayColor,
               textColor: Color.White
            ));
            ElementButtons1.Add(new Button(
               buttonDimensions: elementPositions1[23],
               text: "Propane",
               selection: ElementType.Propane,
               color: PropaneColorScheme.DisplayColor
            ));
            #endregion

            #region extra elements 1
            ElementButtons1.Add(new Button (
                buttonDimensions: extraElementPositions[0],
                text: "Fire",
                selection: ElementType.Fire,
                color: FireColorScheme.DisplayColor
            ));
            #endregion

            #region elements 2
            ElementButtons2.Add(new Button(
                buttonDimensions: elementPositions2[0],
                text: "Aluminium",
                selection: ElementType.Aluminium,
                color: AluminiumColorScheme.DisplayColor
            ));
            #endregion

            MenuSwitchButtons.Add(new Button(
                buttonDimensions: menuSwitchPositions[0],
                text: "<",
                selection: null,
                color: Color.White,
                textColor: Color.Black
            ));
            MenuSwitchButtons.Add(new Button(
                buttonDimensions: menuSwitchPositions[1],
                text: ">",
                selection: null,
                color: Color.White,
                textColor: Color.Black
            ));


            PaintScaleButtons.Add(new Button(
                buttonDimensions: paintScalePositions[0],
                text: "1x1",
                selection: PaintScale.Tiny,
                color: Color.White,
                highlightColor: Color.LightGray
            ));
            PaintScaleButtons.Add(new Button(
                buttonDimensions: paintScalePositions[1],
                text: "2x2",
                selection: PaintScale.Small,
                color: Color.White,
                highlightColor: Color.LightGray
            ));
            PaintScaleButtons.Add(new Button(
                buttonDimensions: paintScalePositions[2],
                text: "4x4",
                selection: PaintScale.Medium,
                color: Color.White,
                highlightColor: Color.LightGray
            ));
            PaintScaleButtons.Add(new Button(
                buttonDimensions: paintScalePositions[3],
                text: "6x6",
                selection: PaintScale.Large,
                color: Color.White,
                highlightColor: Color.LightGray
            ));

            MiscButtons.Add(new Button(
                buttonDimensions: miscPositions[0],
                text: "Clear Grid",
                selection: null,
                color: Color.LightGray
            ));
            MiscButtons.Add(new Button(
                buttonDimensions: miscPositions[1],
                text: "Random Square",
                selection: null,
                color: Color.LightGray
            ));
            MiscButtons.Add(new Button(
               buttonDimensions: miscPositions[2],
               text: "Apply Physics",
               selection: null,
               color: Color.LightGray
            ));
        }
        public static void CheckButtonBoundaries(Pixel[,] pixels) {
            if (ElementMenuIndex == 0) {
                for (int i = 0; i < ElementButtons1.Count; ++i) {
                    ElementButtons1[i].CheckMouseBoundaries();
                }
            } else if (ElementMenuIndex == 1) {
                for (int i = 0; i < ElementButtons2.Count; ++i) {
                    ElementButtons2[i].CheckMouseBoundaries();
                }
            }
            for (int i = 0; i < PaintScaleButtons.Count; ++i) {
                PaintScaleButtons[i].CheckMouseBoundaries();
            }           
            for (int i = 0; i < MiscButtons.Count; ++i) {
                MiscButtons[i].CheckMouseBoundaries();

                if (MiscButtons[i].IsPressed) {
                    switch (i) {
                        case 0:
                            PixelActions.ClearAllPixels(pixels);
                            MiscButtons[i].IsPressed = false;
                            break;
                        case 1:
                            PixelActions.DrawRandomBox(pixels);
                            MiscButtons[i].IsPressed = false;
                            break;
                        case 2:
                            PixelActions.ApplyPhysics();
                            MiscButtons[i].IsPressed = false;
                            break;
                    }
                }
            }

            MenuSwitchButtons[0].CheckMouseBoundaries();
            if (MenuSwitchButtons[0].IsPressed) {
                --ElementMenuIndex;
                if (ElementMenuIndex < 0) {
                    ElementMenuIndex = 0;
                }
                MenuSwitchButtons[0].IsPressed = false;
            }

            MenuSwitchButtons[1].CheckMouseBoundaries();
            if (MenuSwitchButtons[1].IsPressed) {
                ++ElementMenuIndex;
                if (ElementMenuIndex > ElementMenuCount) {
                    ElementMenuIndex = ElementMenuCount;
                }
                MenuSwitchButtons[1].IsPressed = false;
            }
        }
        public static void DrawButtons() {

            if (ElementMenuIndex == 0) {
                for (int i = 0; i < ElementButtons1.Count; ++i) {
                    var currentButton = ElementButtons1[i];
                    if (ElementButtons1[i].ElementSelection == SelectedElement) {
                        ElementButtons1[i].BorderColor = Color.Green;
                    } else {
                        ElementButtons1[i].BorderColor = Color.Black;
                    }
                    ElementButtons1[i].DrawButton();
                }
            } else if (ElementMenuIndex == 1) {
                for (int i = 0; i < ElementButtons2.Count; ++i) {
                    var currentButton = ElementButtons2[i];
                    if (ElementButtons2[i].ElementSelection == SelectedElement) {
                        ElementButtons2[i].BorderColor = Color.Green;
                    } else {
                        ElementButtons2[i].BorderColor = Color.Black;
                    }
                    ElementButtons2[i].DrawButton();
                }
            }


            for (int i = 0; i < PaintScaleButtons.Count; ++i) {
                var currentButton = PaintScaleButtons[i];
                if (PaintScaleButtons[i].PaintScaleSelection == SelectedPaintScale) {
                    PaintScaleButtons[i].BorderColor = Color.Green;
                } else {
                    PaintScaleButtons[i].BorderColor = Color.Black;
                }
                PaintScaleButtons[i].DrawButton();
            }
            for (int i = 0; i < MiscButtons.Count; ++i) {
                MiscButtons[i].DrawButton();
            }
            MenuSwitchButtons[0].DrawButton();
            MenuSwitchButtons[1].DrawButton();
        }
    }
    
}
