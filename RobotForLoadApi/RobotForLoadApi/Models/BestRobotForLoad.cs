using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotForLoadApi.Models
{
    public class BestRobotForLoad
    {
        [JsonProperty("robotId")]
        public string RobotId { get; set; }
        [JsonProperty("distanceToGoal")]
        public double? DistanceToGoal { get; set; }
        [JsonProperty("batteryLevel")]
        public double? BatteryLevel { get; set; }
    }
}
