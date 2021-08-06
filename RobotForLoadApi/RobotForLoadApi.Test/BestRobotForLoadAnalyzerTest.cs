using FluentAssertions;
using RobotForLoadApi.Logic;
using RobotForLoadApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RobotForLoadApi.Test
{
    public class BestRobotForLoadAnalyzerTest
    {
        [Fact]
        public void FindBestRobotForLoad_ShorterDistanceBestAbove10()
        {
            var robots = new Robot[]
            {
                new Robot { RobotId = "1", BatteryLevel = 20d, X = 10d, Y = 10d },
                new Robot { RobotId = "2", BatteryLevel = 30d, X = 40d, Y = 15d }
            };

            var load = new Load { X = 0, Y = 0 };

            var result = BestRobotForLoadAnalyzer.FindBestRobotForLoad(robots, load);

            result.Should().NotBeNull();
            var expectedRobotIndex = 0;
            result.RobotId.Should().Be(robots[expectedRobotIndex].RobotId);

            result.DistanceToGoal.Should().Be(
                Math.Sqrt(
                    Math.Pow(robots[expectedRobotIndex].X - load.X.Value, 2d)
                    + Math.Pow(robots[expectedRobotIndex].Y - load.Y.Value, 2d)));

            result.BatteryLevel.Should().Be(robots[expectedRobotIndex].BatteryLevel);
        }
    }
}
