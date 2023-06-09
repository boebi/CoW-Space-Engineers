﻿#if DEBUG {{{1
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
private IMyTextSurface myScreen;

public Program()
{
	// The constructor, called only once every session and
	// always before any other method is called. Use it to
	// initialize your script.
	myScreen = Me.GetSurface(0);
	myScreen.ContentType = ContentType.TEXT_AND_IMAGE; // optional, can be initialized manually from control panel in game
	myScreen.FontSize = 2; // also optional. 1 is fine for large displays, but 2 is better for small grid laptop and cockpit displays
	myScreen.FontColor = Color.White; // https://github.com/malware-dev/MDK-SE/wiki/VRageMath.Color
	myScreen.BackgroundColor = Color.Black;
}

public void Main(string args, UpdateType updateSource)
{
	myScreen.WriteText("Hell-o-world!");
}

public void Save()
{
}

///////////////////////## END ##/////////////////////////////////////////
//=======================================================================
//=======================================================================
#if DEBUG
    }
}
#endif
