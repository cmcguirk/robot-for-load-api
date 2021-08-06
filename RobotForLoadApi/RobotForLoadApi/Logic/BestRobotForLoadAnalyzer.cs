using RobotForLoadApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotForLoadApi.Logic
{
    public class BestRobotForLoadAnalyzer
    {
        public static BestRobotForLoad FindBestRobotForLoad(Robot[] robots, Load load)
        {
            var bestRobot = new BestRobotForLoad();

            for (int i = 0; i < robots.Length; i++)
            {
                var robot = robots[i];
                var robotDistance = Math.Sqrt(Math.Pow(robot.X - load.X.Value, 2d) + Math.Pow(robot.Y - load.Y.Value, 2d));

                if (bestRobot.DistanceToGoal <= 10d)
                {
                    if (robotDistance <= 10d && robot.BatteryLevel > bestRobot.BatteryLevel)
                    {
                        bestRobot.RobotId = robot.RobotId;
                        bestRobot.DistanceToGoal = robotDistance;
                        bestRobot.BatteryLevel = robot.BatteryLevel;
                    }
                }
                else if (!(robotDistance >= bestRobot.DistanceToGoal))
                {
                    bestRobot.RobotId = robot.RobotId;
                    bestRobot.DistanceToGoal = robotDistance;
                    bestRobot.BatteryLevel = robot.BatteryLevel;
                }
            }

            return bestRobot;
        }
    }
}
