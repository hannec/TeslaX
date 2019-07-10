﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TeslaX.Properties;

namespace TeslaX
{
    public static partial class TechSettings
    {
        public static int BlocksAhead = 3; // 0 = crash.
        public static int BlocksBehind = 1; // 0 = crash.

        // Possible deviation of RGB values when comparing.
        public static int Distortion = 2;

        // Order of locations (horizontal) to check for player, relative to last known position.
        // For each command, every location from x1 to x2 is checked, looking for player facing same or different (from current) direction.
        // Main tool in calibrating LastKnown for accuracy.
        public static (int x1, int x2, bool SameDirection)[] Order = new (int, int, bool)[]
        {
            (0, 0, true), // First guess: same as before (center of the screen).
            (0, 0, false), // Mid-loop swap compatibility. To be commented in/out depending on situation.
            (1, 24, true), // Second guess: ahead (before or after reaching center).
            (-24, -1, true), // MLSC.
            (-24, 24, false), // MLSC.
        };

        public static Func<Point, Point, bool> PlayerSpikeCondition =
            (ov, nv) => Math.Abs(ov.X - nv.X) > 32 || (ov.X == Window.Width / 2 - 16 && ov != nv);

        public static int PlayerSpikeLength = 250;

        public static Func<int, int, bool> DistanceSpikeCondition =
            ((ov, nv) => Math.Abs(ov - nv) > 24 || nv == -1);

        public static int DistanceSpikeLength = 150;

        // Whether "Uncertain" block should be functionally treated as Block instead of Air.
        public static bool UncertainIsBlock = false;

        // Largest gem to be considered. Might affect performance, not sure.
        // 0 - no gems
        // 1 - yellow (1)
        // 2 - blue   (5)
        // 3 - red    (10)
        // 4 - green  (50)
        // 5 - purple (100)
        public static int BiggestGem = 3;
    }
}