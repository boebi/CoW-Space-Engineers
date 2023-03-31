#if DEBUG {{{1
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
        
#endif }}}
//=======================================================================
//////////////////////////BEGIN//////////////////////////////////////////
//=======================================================================
private const string CockPitName = "Fighter Cockpit";
private IMyTextSurface myscreen;
private IMyTextSurface cockpitScreen;
private IMySlimBlock slimCockpit;
private IMyCockpit fatCockpit;
private IMyRadioAntenna antenna;
private Vector3D basePos;

public Program()
{
	Runtime.UpdateFrequency = UpdateFrequency.Update10;
	myscreen = Me.GetSurface(0);
	antenna = GridTerminalSystem.GetBlockWithName("Antenna") as IMyRadioAntenna;
	fatCockpit = GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
	slimCockpit = GetSlimBlockFromFat(fatCockpit);
	cockpitScreen = fatCockpit.GetSurface(1);
	cockpitScreen.FontSize = 3.5f;
	basePos = new Vector3D(0, 0, 0); // base not really here, just a placeholder
}

public void Main(string args)
{
	var cockpitHealth = slimCockpit.BuildLevelRatio;
	var percentageToDisable = Math.Round(100 * (cockpitHealth - 0.4f) / 0.6f, 0);

	var myPos = fatCockpit.GetPosition();
	var distance = Vector3D.Distance(basePos, myPos);
	antenna.Radius = (float)(distance + 300);
	var printDistance = string.Format("{0:0.0}km", distance*0.001);

	cockpitScreen.WriteText($"{percentageToDisable}%\n{printDistance}");
}

// functions
IMySlimBlock GetSlimBlockFromFat(IMyTerminalBlock block) { return block.CubeGrid.GetCubeBlock(block.Position); }
//=======================================================================
//////////////////////////END////////////////////////////////////////////
//=======================================================================
#if DEBUG
    }
}
#endif
