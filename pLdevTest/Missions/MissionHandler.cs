using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    public static class MissionHandler
    {
        private static int mission = 0;
        public static int Mission
        {
            get { return mission; }
        }

        private readonly static string[] missions =
        {
            "Up and Down!",
            "Left and Right!",
            "Declaration of...",
            "If only...",
            "Else:",
            "Only if else..."
        };
        public readonly static String[,][] MissionsInfoText =
        {
            {
                new[]
                {
                    "Move the robot vertically with:",
                    "robot.x = 4"
                }
            },
            {
                new[]
                {
                    "Move the robot horizontally with:",
                    "robot.y = 4"
                }
            }
        };
        public readonly static Color[,][] MissionsInfoColor =
{
            {
                new[]
                {
                    Color.Black,
                    PlayGround.pgColor
                }
            },
            {
                new[]
                {
                    Color.Black,
                    PlayGround.pgColor
                }
            }
        };

        public static string[] Missions
        {
            get { return missions; }
        }

        public static bool[] MissionsComplete =
        {
            false,
            false,
            false,
            false,
        };

        public static void CheckForMission()
        {
            bool missionComplete = false;
            if (MissionsComplete[mission])
            {
                missionComplete = true;
            }

            if(missionComplete)
            {
                mission++;
            }
        }
    }
}
