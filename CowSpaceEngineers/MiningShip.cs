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
    public sealed class MiningShip : MyGridProgram
    {
#endif }}}
//=======================================================================
//=======================================================================
//////////////// The cockpit has to be called "Drillpit"/////////////////
///////////////////////## BEGIN ##///////////////////////////////////////
private const string CockPitName = "Drillpit";

private readonly IMyCockpit _helm;
private readonly IMyTextSurface _drillMonitor;
private readonly IMyTextSurface _batteryMonitor;

private List<IMyTerminalBlock> _drills;
private List<IMyTerminalBlock> _cargoContainers;
private List<IMyBatteryBlock> _batteryList;

public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update10;
    _helm =  GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
    _drillMonitor = _helm.GetSurface(1);
    _batteryMonitor = _helm.GetSurface(2);

    InitSurfaces();
    InitBlockLists();
}

public void Main(string argument, UpdateType updateSource)
{
    var drillPercentage = DrillCargoPercentage();
    var cargoPercentage = CargoContainerPercentage();

    var almostFull = drillPercentage > 85;
    _drillMonitor.BackgroundColor = almostFull ? Color.Red : Color.Black;

    _drillMonitor.WriteText($"Drill\n{drillPercentage}%\nCargo\n{cargoPercentage}%");
    string storedPower = formatTotalStoredPower();
    _batteryMonitor.WriteText($"batt:\n{storedPower}");
}

private void InitBlockLists() {
	_drills = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(_drills);

	_cargoContainers = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_cargoContainers);

	_batteryList = new List<IMyBatteryBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(_batteryList);
}

private void InitSurfaces()
{
    _drillMonitor.FontSize = 3;
    _drillMonitor.Alignment = TextAlignment.CENTER;

    _batteryMonitor.FontSize = 3;
}

private double DrillCargoPercentage()
{
    float totalDrillCapacity = 0;
    float currentDrillCapacity = 0;
    foreach (var drill in _drills)
    {
        totalDrillCapacity += drill.GetInventory(0).MaxVolume.RawValue;
        currentDrillCapacity += drill.GetInventory(0).CurrentVolume.RawValue;
    }

    var drillPercentage = 100.0f * currentDrillCapacity / totalDrillCapacity;
    return Math.Round(drillPercentage, 0);
}

private double CargoContainerPercentage()
{
    float totalCargoCapacity = 0;
    float currentCargoCapacity = 0;
    foreach (var container in _cargoContainers)
    {
        totalCargoCapacity += container.GetInventory(0).MaxVolume.RawValue;
        currentCargoCapacity += container.GetInventory(0).CurrentVolume.RawValue;
    }

    var cargoContainerPercentage = 100.0f * currentCargoCapacity / totalCargoCapacity;
    return Math.Round(cargoContainerPercentage, 0);
}

private string formatTotalStoredPower() {
	float MWhTotal = 0;
	foreach (var battery in _batteryList) {
		MWhTotal += battery.CurrentStoredPower;
	}

	if (MWhTotal >= 10.0f) {
		return string.Format("{0:0.0} MWh", MWhTotal);
	} else {
		return string.Format("{0:0.0} kWh", MWhTotal*1000);
	}
}

///////////////////////## END ##///////////////////////////////////////
//=======================================================================
//=======================================================================
#if DEBUG
    }
}
#endif
