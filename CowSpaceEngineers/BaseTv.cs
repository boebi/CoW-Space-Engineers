#if DEBUG
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace SpaceEngineers
{
    public sealed class BaseTv : MyGridProgram
    {
#endif
        //=======================================================================
        //////////////////////////BEGIN//////////////////////////////////////////
        //=======================================================================
        private const int MinSteelPlates = 4000;
        private const int MinComputers = 300;
        private const int MinConstructionComponents = 2000;
        private const int MinInteriorPlates = 1000;
        private const int MinLargeSteelTube = 400;
        private const int MinMetalGrid = 400;
        private const int MinMotors = 800;
        private const int MinSmallSteelTube = 1000;
        private const int MinAutocannonMags = 50;
        private const int MinGattlingMags = 50;

        private List<IMyTerminalBlock> _cargoContainers;
        
        private const string ScreenName = "BIGSCREEN";
        private IMyTextPanel _screen;
        
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            _cargoContainers = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_cargoContainers);
            _screen = GridTerminalSystem.GetBlockWithName(ScreenName) as IMyTextPanel;
        }

        public void Main(string args)
        {
            var totalSteel = CheckItem(new MyItemType("MyObjectBuilder_Component", "SteelPlate"));
            var totalComputers = CheckItem(new MyItemType("MyObjectBuilder_Component", "Computer"));
            var totalConstructionComponents = CheckItem(new MyItemType("MyObjectBuilder_Component", "Construction"));
            var totalInteriorPlates = CheckItem(new MyItemType("MyObjectBuilder_Component", "InteriorPlate"));
            var totalLargeTubes = CheckItem(new MyItemType("MyObjectBuilder_Component", "LargeTube"));
            var totalMetalGrids = CheckItem(new MyItemType("MyObjectBuilder_Component", "MetalGrid"));
            var totalMotors = CheckItem(new MyItemType("MyObjectBuilder_Component", "Motor"));
            var totalSmallTubes = CheckItem(new MyItemType("MyObjectBuilder_Component", "SmallTube"));
            var totalAutocannon = CheckItem(new MyItemType("MyObjectBuilder_AmmoMagazine", "AutocannonClip"));
            var totalGattling = CheckItem(new MyItemType("MyObjectBuilder_AmmoMagazine", "NATO_25x184mm"));
            var totalBulletProofGlass = CheckItem(new MyItemType("MyObjectBuilder_Component", "BulletproofGlass"));
            var totalDisplays = CheckItem(new MyItemType("MyObjectBuilder_Component", "Display"));

            
            _screen.ContentType = ContentType.TEXT_AND_IMAGE;
            _screen.FontSize = 1.2f;
            _screen.BackgroundColor = Color.Black;
            _screen.FontColor = Color.White;
            _screen.WriteText($"Steel plates: {totalSteel} / {MinSteelPlates}\nComputers:{totalComputers} / {MinComputers}\nConstrComponent:{totalConstructionComponents} / {MinConstructionComponents}\nIntPlates:{totalInteriorPlates} / {MinInteriorPlates}\nSmallTubes:{totalSmallTubes} / {MinSmallSteelTube}\nLargeTubes:{totalLargeTubes} / {MinLargeSteelTube}\nMetalGrids:{totalMetalGrids} / {MinMetalGrid}\nMotors:{totalMotors} / {MinMotors}\nAutocannon:{totalAutocannon} / {MinAutocannonMags}\nGattling:{totalGattling} / {MinGattlingMags}\nBulletProofGlass:{totalBulletProofGlass}\nDisplays:{totalDisplays}");
        }

        private long CheckItem(MyItemType itemType)
        {
            long inventory = 0;
            foreach (var cargoContainer in _cargoContainers)
            {
                inventory += cargoContainer.GetInventory(0)
                    .GetItemAmount(itemType).RawValue;
            }

            inventory /= 1000000;

            return inventory;
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means.
        
            // This method is optional and can be removed if not
            // needed.
        }

        //=======================================================================
        //////////////////////////END////////////////////////////////////////////
        //=======================================================================
#if DEBUG
    }
}
#endif