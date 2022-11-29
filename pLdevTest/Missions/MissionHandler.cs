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
            set { mission = value; }
        }

        private readonly static string[] missions =
        {
            "vertical movement basics",
            "horizontal movement basics",
            "TUTORIAL COMPLETE! WOHOO!"
        };

        public static string[] Missions
        {
            get { return missions; }
        }


        public static void CheckForMission()
        {
            bool missionComplete = false;
            switch(mission)
            {
                case 0:
                    if(Game1.playground.player.playerY == 4)
                    {
                        Debug.WriteLine("Mission 1 complete!");
                        missionComplete = true;
                    }
                    break;
                case 1:
                    if(Game1.playground.player.playerX == 4)
                    {
                        Debug.WriteLine("Mission 2 complete!");
                        missionComplete = true;
                    }
                    break;
            }

            if(missionComplete)
            {
                mission++;
            }
        }
    }
}
