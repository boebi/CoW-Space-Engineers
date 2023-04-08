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
private float kNtotal = 43.2f;
private string sbcFormatted = "0 m";

public Program()
{
	Runtime.UpdateFrequency = UpdateFrequency.Update100;
	// The constructor, called only once every session and
	// always before any other method is called. Use it to
	// initialize your script.
	myScreen = Me.GetSurface(0);
	myScreen = (GridTerminalSystem.GetBlockWithName(CockPitName) as IMyTextSurfaceProvider).GetSurface(0);
	myScreen.ContentType = ContentType.TEXT_AND_IMAGE; // optional, can be initialized manually from control panel in game
	myScreen.FontSize = 2; // also optional. 1 is fine for large displays, but 2 is better for small grid laptop and cockpit displays
	myScreen.FontColor = Color.White; // https://github.com/malware-dev/MDK-SE/wiki/VRageMath.Color
	myScreen.BackgroundColor = Color.Black;

	myCockpit = GridTerminalSystem.GetBlockWithName(CockPitName) as IMyCockpit;
}

public void Main(string args, UpdateType updateSource)
{
	float velocity = (float)myCockpit.GetShipVelocities().LinearVelocity.Length();
	if (velocity < 1.0f) {
		sbcFormatted = "RESET";
	}
	if (args == "sbc") {
		float gridmasstonnes = myCockpit.CalculateShipMass().TotalMass * 0.001f;
		float acceleration = kNtotal / gridmasstonnes;
		float secondsToStop = velocity / acceleration;
		float distanceToStop = secondsToStop * secondsToStop * acceleration * 0.5f;
		sbcFormatted = string.Format("{0:0} m", distanceToStop);
		//myScreen.WriteText($"{velocity}\n{gridmasstonnes}\n{acceleration}\n{sbc}");
	}
	myScreen.WriteText($"{sbcFormatted}");
}

///////////////////////## END ##/////////////////////////////////////////
//=======================================================================
//=======================================================================
#if DEBUG
    }
}
#endif
