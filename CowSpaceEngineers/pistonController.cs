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
//private IMyTextSurface myScreen;
private IMyPistonBase piston;

public Program()
{
	//myScreen = Me.GetSurface(0);
	//myScreen.ContentType = ContentType.TEXT_AND_IMAGE;
	//myScreen.FontSize = 2;
	//myScreen.FontColor = Color.White;
	//myScreen.BackgroundColor = Color.Black;

	piston = GridTerminalSystem.GetBlockWithName("Piston, Welderarray") as IMyPistonBase;
}

public void Main(string args, UpdateType updateSource)
{
	//myScreen.WriteText($"Welder array\ncontroller 1.0\n\n{args}");

	piston.Velocity = float.Parse(args);
}
//=======================================================================
//////////////////////////END////////////////////////////////////////////
//=======================================================================
#if DEBUG
    }
}
#endif
