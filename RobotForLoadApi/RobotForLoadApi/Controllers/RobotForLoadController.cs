using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotForLoadApi.Logic;
using RobotForLoadApi.Models;
using RobotForLoadApi.RobotsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RobotForLoadApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RobotForLoadController : ControllerBase
    {
        private readonly RobotsHttpClient _robotsClient;

        public RobotForLoadController(RobotsHttpClient robotsClient)
        {
            _robotsClient = robotsClient;
        }

        [HttpPost("FindBest")]
        public async Task<IActionResult> FindBest([FromBody]Load load)
        {
            var robots = await _robotsClient.GetRobots();

            if (robots == null)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new ErrorResponse { Message = "error retrieving robots" });
            }

            return Ok(BestRobotForLoadAnalyzer.FindBestRobotForLoad(robots, load));
        }
    }
}
