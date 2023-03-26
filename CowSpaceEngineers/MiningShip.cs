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

namespace SpaceEngineers
{
    public sealed class MiningShip : MyGridProgram
    {
#endif
        //=======================================================================
        //////////////////////////BEGIN//////////////////////////////////////////
		//////////////// The cockpit has to be called "Drillpit"/////////////////
        //=======================================================================
        private const string CockPitName = "Drillpit";
        
        private readonly IMyCockpit _helm;
        private List<IMyTerminalBlock> _drills;
        private List<IMyTerminalBlock> _cargoContainers;
        private IMyTextSurface _drillMonitor;
        private IMyTextSurface _cargoMonitor;

        public Program()
        {
            _helm =  GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            
            InitializeDrills();
            InitializeCargoContainers();
        }

        private void InitializeDrills()
        {
            _drills = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(_drills);
            _drillMonitor = _helm.GetSurface(1);
            _drillMonitor.FontColor = Color.White;
            _drillMonitor.FontSize = 5;
            _drillMonitor.Alignment = TextAlignment.CENTER;
        }

        private void InitializeCargoContainers()
        {
            _cargoContainers = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_cargoContainers);
            _cargoMonitor = _helm.GetSurface(0);
            _cargoMonitor.FontColor = Color.White;
            _cargoMonitor.FontSize = 5;
            _cargoMonitor.Alignment = TextAlignment.CENTER;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            var almostFull = WriteDrillCargoPercentage();
            WriteCargoContainerPercentage(almostFull);
        }

        private bool WriteDrillCargoPercentage()
        {
            float totalDrillCapacity = 0;
            float currentDrillCapacity = 0;
            foreach (var drill in _drills)
            {
                totalDrillCapacity += drill.GetInventory(0).MaxVolume.RawValue;
                currentDrillCapacity += drill.GetInventory(0).CurrentVolume.RawValue;
            }

            var drillPercentage = 100.0f * currentDrillCapacity / totalDrillCapacity;
            var drillRoundedPercentage = Math.Round(drillPercentage, 0);

            var almostFull = drillRoundedPercentage > 85;
            _drillMonitor.BackgroundColor = almostFull ? Color.Red : Color.Black;
            
            _drillMonitor.WriteText($"Drill\n{drillRoundedPercentage}%");

            return almostFull;
        }

        private void WriteCargoContainerPercentage(bool almostFull)
        {
            float totalCargoCapacity = 0;
            float currentCargoCapacity = 0;
            foreach (var container in _cargoContainers)
            {
                totalCargoCapacity += container.GetInventory(0).MaxVolume.RawValue;
                currentCargoCapacity += container.GetInventory(0).CurrentVolume.RawValue;
            }

            var cargoContainerPercentage = 100.0f * currentCargoCapacity / totalCargoCapacity;
            var cargoContainerRoundedPercentage = Math.Round(cargoContainerPercentage, 0);
            
            _cargoMonitor.BackgroundColor = almostFull ? Color.Red : Color.Black;
            
            _cargoMonitor.WriteText($"Cargo\n{cargoContainerRoundedPercentage}%");
        }
        
        
        //=======================================================================
        //////////////////////////END//////////////////////////////////////////
        //=======================================================================
#if DEBUG
    }
}
#endif