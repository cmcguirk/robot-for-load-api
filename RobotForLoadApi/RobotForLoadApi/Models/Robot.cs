using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotForLoadApi.Models
{
    public class Robot
    {
        [JsonProperty("robotId")]
        public string RobotId { get; set; }
        [JsonProperty("batteryLevel")]
        public double BatteryLevel { get; set; }
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }
    }
}
