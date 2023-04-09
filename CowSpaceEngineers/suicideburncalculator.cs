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
private const string CockPitName = "Fighter Cockpit"; // for debugging purposes

private IMyTextSurface myScreen;
private IMyCockpit myCockpit;

private int internalClock = 0;

private const float sbc_kNtotal = 43.2f;	// needs to be set by a space engineer
private int sbc_ComputeAlgo = -1;
private string sbcFormatted = "";

public Program()
{
	Runtime.UpdateFrequency = UpdateFrequency.Update10;
	// The constructor, called only once every session and
	// always before any other method is called. Use it to
	// initialize your script.
	//myScreen = Me.GetSurface(0);
	myScreen = (GridTerminalSystem.GetBlockWithName(CockPitName) as IMyTextSurfaceProvider).GetSurface(0);
	myScreen.ContentType = ContentType.TEXT_AND_IMAGE; // optional, can be initialized manually from control panel in game
	myScreen.FontSize = 2; // also optional. 1 is fine for large displays, but 2 is better for small grid laptop and cockpit displays
	myScreen.FontColor = Color.White; // https://github.com/malware-dev/MDK-SE/wiki/VRageMath.Color
	myScreen.BackgroundColor = Color.Black;

	myCockpit = GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
}

public void Main(string args, UpdateType updateSource)
{
	if (updateSource == UpdateType.Trigger) {
		if (args == "sbc") {
			sbc_ComputeAlgo = 3;sbcFormatted = "calculating..";
		}
	} else {
		if ((internalClock = ++internalClock % 10) == 0) {
			float velocity = (float)myCockpit.GetShipVelocities().LinearVelocity.Length();
			if (velocity < 1.0f) {
				sbcFormatted = "init zero";
				sbc_ComputeAlgo = -1;
			} else if (sbc_ComputeAlgo == 0) {
				sbcFormatted = GetFormattedDistanceToStop(myCockpit, velocity);
			}

			if (sbc_ComputeAlgo > -1) {
				sbc_ComputeAlgo--;
			}
			myScreen.WriteText($"{sbcFormatted}");
		}
	}
}

private string GetFormattedDistanceToStop(IMyCockpit controller, float velocity) {
	float gridmasstonnes = controller.CalculateShipMass().TotalMass * 0.001f;
	float acceleration = sbc_kNtotal / gridmasstonnes;
	float secondsToStop = velocity / acceleration;
	float distanceToStop = secondsToStop * secondsToStop * acceleration * 0.5f;
	return string.Format("{0:0} m", distanceToStop);

}

///////////////////////## END ##/////////////////////////////////////////
//=======================================================================
//=======================================================================
#if DEBUG
    }
}
#endif
