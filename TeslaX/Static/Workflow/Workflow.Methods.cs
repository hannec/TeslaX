﻿using System.Drawing;

namespace TheLeftExit.TeslaX.Static
{
    internal static partial class Workflow
    {
        private class NextBlockInfo
        {
            public int Distance;
            public short Foreground;
            public short Background;

            private bool inRange() =>
                UserSettings.Current.Range == 0 || Distance <= UserSettings.Current.Range;

            public bool IsBlock() =>
                inRange() && (UserSettings.Current.BlockID == 0 || Foreground == UserSettings.Current.BlockID || Background == UserSettings.Current.BlockID);

            public bool IsDoor() =>
                inRange() && (UserSettings.Current.BlockID == 0 || Foreground == UserSettings.Current.DoorID || Background == UserSettings.Current.DoorID);
        }

        private static NextBlockInfo GetNextBlockInfo(this ProcessHandle handle)
        {
            Point rawPlayer = handle.GetPlayer();
            bool rawDirection = handle.GetDirection();

            Point player = new Point(rawPlayer.X / 32, (rawPlayer.Y - 2 + 31) / 32);

            int firstSearchedX = player.X + (rawDirection ? 1 : -1);
            int increment = rawDirection ? 1 : -1;

            NextBlockInfo res = null;
            int NextBlockX = -1;

            for (int i = firstSearchedX; i >= 0 && i < 100; i += increment)
            {
                short fore = handle.GetBlock(i, player.Y);
                short back = handle.GetBackground(i, player.Y);
                if (fore != 0 || back != 0)
                {
                    res = new NextBlockInfo()
                    {
                        Background = back,
                        Foreground = fore
                    };
                    NextBlockX = i;
                    break;
                }
            }

            if (NextBlockX == -1)
                return null;

            res.Distance = ((rawPlayer.X - 6) - (NextBlockX * 32)) * (rawDirection ? -1 : 1) - 32;
            return res;
        }

        private static bool Move(int distance, bool direction)
        {
            if (distance == -1)
                return false;
            int target = direction ? UserSettings.Current.DistanceRight : UserSettings.Current.DistanceLeft;
            return distance > target;
        }
    }
}