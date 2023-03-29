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
    public sealed class ProgramTemplate : MyGridProgram
    {
        
#endif
       
        
        //=======================================================================
        //////////////////////////BEGIN//////////////////////////////////////////
        //=======================================================================

        private IMyTextSurface _monitor;
        private IMySlimBlock _slimBlock;
        private const string CockPitName = "Battlepit";

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            var helm = GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
            var helmAsMyTerminalBlock = GridTerminalSystem.GetBlockWithName(CockPitName) as IMyTerminalBlock;
            _slimBlock = helmAsMyTerminalBlock.CubeGrid.GetCubeBlock(helmAsMyTerminalBlock.Position);
            _monitor = helm.GetSurface(0);
            _monitor.FontColor = Color.White;
            _monitor.FontSize = 5;
            _monitor.Alignment = TextAlignment.CENTER;
        }
        
        public void Main(string args)
        {
            var maxIntegrity = _slimBlock.MaxIntegrity;
            var buildIntegrity = _slimBlock.BuildIntegrity;
            var currentDamage = _slimBlock.CurrentDamage;
            var percentage = (buildIntegrity - currentDamage) / maxIntegrity;
            var percentageToDisable = 100 * (percentage - 0.4f) / 0.6f; 
            
            _monitor.WriteText($"{Math.Round(percentageToDisable, 0)}%");
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