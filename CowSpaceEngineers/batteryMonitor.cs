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
//=======================================================================
///////////////////////## BEGIN ##///////////////////////////////////////
private readonly List<IMyBatteryBlock> _batteryList = new List<IMyBatteryBlock>();
private IMyTextSurface myscreen;

public Program() {
	// Runtime.UpdateFrequency = UpdateFrequency.Update100;
	// myscreen = (GridTerminalSystem.GetBlockWithName("Cockpit") as IMyTextSurfaceProvider).GetSurface(0);
	myscreen = Me.GetSurface(0);
	myscreen.ContentType = ContentType.TEXT_AND_IMAGE;
	myscreen.FontSize = 2;

	batteryListInit();
}

public void Main() {
	var storedPower = string.Format("{0:0}%", 100f*StoredMWhSum(_batteryList, true)); // percentage
	//var storedPower = FormatMWh(StoredMWhSum(_batteryList)); // absolute amount of energy
	myscreen.WriteText($"batt:\n{storedPower}");
}

private void batteryListInit() {
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(_batteryList);
}

private string FormatMWh(float MWh) {
	if (MWh >= 10.0f) {
		return string.Format("{0:0.0} MWh", MWh);
	} else {
		return string.Format("{0:0.0} kWh", MWh*1000);
	}
}

private float StoredMWhSum(List<IMyBatteryBlock> batteryList, bool returnRatio = false) {
	float MWhStored = 0;
	float MWhMax = 0;
	foreach (var battery in batteryList) {
		MWhStored += battery.CurrentStoredPower;
		if (returnRatio) MWhMax += battery.MaxStoredPower;
	}
	return returnRatio ? MWhStored/MWhMax : MWhStored;
}
///////////////////////## END ##/////////////////////////////////////////
//=======================================================================
//=======================================================================
#if DEBUG
    }
}
#endif
