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
    public sealed class BaseMinimumConstructionShit : MyGridProgram
    {
#endif
        //=======================================================================
        //////////////////////////BEGIN//////////////////////////////////////////
        //=======================================================================
        private const string AssemblerName = "Assembler, Primary";
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
        private List<IMyAssembler> _assemblers;
        private IMyAssembler _primaryAssembler;
        
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            _cargoContainers = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_cargoContainers);
            _assemblers = new List<IMyAssembler>();
            GridTerminalSystem.GetBlocksOfType<IMyAssembler>(_assemblers);
            _primaryAssembler = GridTerminalSystem.GetBlockWithName(AssemblerName) as IMyAssembler;
        }

        public void Main(string args)
        {
            var totalSteel = CheckItem(MinSteelPlates, new MyItemType("MyObjectBuilder_Component", "SteelPlate"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/SteelPlate"));
            var totalComputers = CheckItem(MinComputers, new MyItemType("MyObjectBuilder_Component", "Computer"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/ComputerComponent"));
            var totalConstructionComponents = CheckItem(MinConstructionComponents, new MyItemType("MyObjectBuilder_Component", "Construction"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/ConstructionComponent"));
            var totalInteriorPlates = CheckItem(MinInteriorPlates, new MyItemType("MyObjectBuilder_Component", "InteriorPlate"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/InteriorPlate"));
            var totalLargeTubes = CheckItem(MinLargeSteelTube, new MyItemType("MyObjectBuilder_Component", "LargeTube"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/LargeTube"));
            var totalMetalGrids = CheckItem(MinMetalGrid, new MyItemType("MyObjectBuilder_Component", "MetalGrid"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/MetalGrid"));
            var totalMotors = CheckItem(MinMotors, new MyItemType("MyObjectBuilder_Component", "Motor"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/MotorComponent"));
            var totalSmallTubes = CheckItem(MinSmallSteelTube, new MyItemType("MyObjectBuilder_Component", "SmallTube"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/SmallTube"));
            var totalAutocannon = CheckItem(MinAutocannonMags, new MyItemType("MyObjectBuilder_AmmoMagazine", "AutocannonClip"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/AutocannonClip"));
            var totalGattling = CheckItem(MinGattlingMags, new MyItemType("MyObjectBuilder_AmmoMagazine", "NATO_25x184mm"), 
                MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/NATO_25x184mmMagazine"));

            Me.GetSurface(0).ContentType = ContentType.TEXT_AND_IMAGE;
            Me.GetSurface(0).FontSize = 1;
            Me.GetSurface(0).BackgroundColor = Color.Black;
            Me.GetSurface(0).FontColor = Color.White;
            Me.GetSurface(0).WriteText($"Steel plates: {totalSteel} / {MinSteelPlates}\nComputers:{totalComputers} / {MinComputers}\nConstrComponent:{totalConstructionComponents} / {MinConstructionComponents}\nIntPlates:{totalInteriorPlates} / {MinInteriorPlates}\nSmallTubes:{totalSmallTubes} / {MinSmallSteelTube}\nLargeTubes:{totalLargeTubes} / {MinLargeSteelTube}\nMetalGrids:{totalMetalGrids} / {MinMetalGrid}\nMotors:{totalMotors} / {MinMotors}\nAutocannon:{totalAutocannon} / {MinAutocannonMags}\nGattling:{totalGattling} / {MinGattlingMags}");
        }

        private long CheckItem(int minimum, MyItemType itemType, MyDefinitionId definitionId)
        {
            long inventory = 0;
            foreach (var cargoContainer in _cargoContainers)
            {
                inventory += cargoContainer.GetInventory(0)
                    .GetItemAmount(itemType).RawValue;
            }

            inventory /= 1000000;

            long queue = 0;
            long totalQueue = 0;
            foreach (var assembler in _assemblers)
            {
                var itemsInQueue = new List<MyProductionItem>();
                assembler.GetQueue(itemsInQueue);
                totalQueue = itemsInQueue.Count;
                queue += itemsInQueue.Where(_ => _.BlueprintId == definitionId).Sum(_ => _.Amount.RawValue);
            }

            queue = queue / 1000000 - 1;
            var total = inventory + queue;

            if (total < minimum && totalQueue == 0)
            {
                _primaryAssembler.AddQueueItem(definitionId, (float)minimum-total);
            }

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