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
///////////////////////## BEGIN ##///////////////////////////////////////
private const string CockPitName = "Drillpit"; // The cockpit has to be called "Drillpit"
private const float sbc_kNtotal = 1.0f;	// needs to be set by a space engineer

private readonly IMyCockpit _helm;
private readonly IMyTextSurface _drillMonitor;
private readonly IMyTextSurface _batteryMonitor;

private readonly List<IMyTerminalBlock> _drills = new List<IMyTerminalBlock>();
private readonly List<IMyTerminalBlock> _cargoContainers = new List<IMyTerminalBlock>();
private readonly List<IMyBatteryBlock> _batteryList = new List<IMyBatteryBlock>();

private int _internalClock = 0;
private int sbc_ComputeAlgo = -1;
private string sbc_Formatted = "";

public Program()
{
	IMyTextSurface myScreen = Me.GetSurface(0);
	myScreen.ContentType = ContentType.TEXT_AND_IMAGE;
	myScreen.FontSize = 2;
	myScreen.WriteText("MiningShipOS 1.42");

	Runtime.UpdateFrequency = UpdateFrequency.Update10;
	_helm =  GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
	if (_helm == null) {
		myScreen.WriteText("\nerror:\nmissing Drillpit", true);
	} else {
		myScreen.WriteText("\nDrillpit init OK", true);
	}
	_drillMonitor = _helm.GetSurface(1);
	_batteryMonitor = _helm.GetSurface(2);

	InitSurfaces();
	InitBlockLists();
	myScreen.WriteText($"\ndrills: {_drills.Count}", true);
	myScreen.WriteText($"\ncargos: {_cargoContainers.Count}", true);
	myScreen.WriteText($"\nbatteries: {_batteryList.Count}", true);
}

public void Main(string argument, UpdateType updateSource)
{
	if (updateSource == UpdateType.Trigger) {
		switch (argument) {
			case "mc":
				// placeholder: reset master caution
				break;
			case "sbc":
				sbc_ComputeAlgo=4;sbc_Formatted="calculating..";
				break;
		}
	} else {
		var drillPercentage = DrillCargoPercentage();
		var cargoPercentage = CargoContainerPercentage();
		var almostFull = drillPercentage > 85;
		_drillMonitor.BackgroundColor = almostFull ? Color.Red : Color.Black;
		_drillMonitor.WriteText($"Drill\n{drillPercentage}%\nCargo\n{cargoPercentage}%");

		if ((_internalClock = ++_internalClock % 10) == 0) {
			float velocity = (float)_helm.GetShipVelocities().LinearVelocity.Length();
			if (velocity < 1.0f) {
				sbc_Formatted = "init zero";
				sbc_ComputeAlgo = -1;
			} else if (sbc_ComputeAlgo == 0) {
				sbc_Formatted = GetFormattedDistanceToStop(_helm, velocity);
			}

			if (sbc_ComputeAlgo > -1) sbc_ComputeAlgo--;

			// myScreen.WriteText($"{sbc_Formatted}");
			string storedPower = formatTotalStoredPower();
			_batteryMonitor.WriteText($"batt:\n {storedPower}\nsbc:\n {sbc_Formatted}");

		}
	}
}

private void InitBlockLists() {
	//_drills = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(_drills);

	//_cargoContainers = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_cargoContainers);

	//_batteryList = new List<IMyBatteryBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(_batteryList);
}

private void InitSurfaces()
{
	_drillMonitor.ContentType = ContentType.TEXT_AND_IMAGE;
	_drillMonitor.FontSize = 3;
	_drillMonitor.Alignment = TextAlignment.CENTER;

	_batteryMonitor.ContentType = ContentType.TEXT_AND_IMAGE;
	_batteryMonitor.FontSize = 3;
}

private double DrillCargoPercentage()
{
	if (_drills.Count == 0) return 0;
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
	if (_cargoContainers.Count == 0) return 0;
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

private string GetFormattedDistanceToStop(IMyCockpit controller, float velocity) {
	float gridmasstonnes = controller.CalculateShipMass().TotalMass * 0.001f;
	float acceleration = sbc_kNtotal / gridmasstonnes;
	float secondsToStop = velocity / acceleration;
	float distanceToStop = secondsToStop * secondsToStop * acceleration * 0.5f;
	return string.Format("{0:0} m", distanceToStop);
}
///////////////////////## END ##///////////////////////////////////////
//=======================================================================
//=======================================================================
#if DEBUG
    }
}
#endif
