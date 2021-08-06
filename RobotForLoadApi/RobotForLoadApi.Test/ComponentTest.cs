using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using RobotForLoadApi.Models;
using RobotForLoadApi.RobotsApi;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RobotForLoadApi.Test
{
    public class ComponentTest
    {
        [Fact]
        public async Task RobotForLoadFindBest_Success()
        {
            using (var mockHttp = new MockHttpMessageHandler())
            {
                var robotsHttpClientSettings = new RobotsHttpClientSettings
                {
                    BaseUrl = "https://base/",
                    FallbackUrl = "https://fallback/"
                };

                var robots = new Robot[]
                {
                    new Robot { RobotId = "1", BatteryLevel = 20d, X = 10d, Y = 10d },
                    new Robot { RobotId = "2", BatteryLevel = 30d, X = 40d, Y = 15d },
                    new Robot { RobotId = "3", BatteryLevel = 40d, X = 30d, Y = 25d },
                    new Robot { RobotId = "4", BatteryLevel = 50d, X = 25d, Y = 30d }
                };

                mockHttp.When(robotsHttpClientSettings.BaseUrl + "*")
                    .Respond(MediaTypeNames.Application.Json, JsonConvert.SerializeObject(robots));

                var hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer().UseStartup<Startup>().ConfigureServices(services =>
                    {
                        services.AddSingleton(sp => robotsHttpClientSettings)
                            .AddSingleton(sp => new RobotsHttpClient(
                                mockHttp.ToHttpClient(),
                                sp.GetRequiredService<RobotsHttpClientSettings>()));
                    });
                });

                var loadPayload = new
                {
                    loadId = "231",
                    x = 29d,
                    y = 26d
                };

                using (var host = await hostBuilder.StartAsync())
                using (var client = host.GetTestClient())
                using (var postContent = new StringContent(
                    JsonConvert.SerializeObject(loadPayload),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json))
                using (var result = await client.PostAsync("/RobotForLoad/FindBest", postContent))
                {
                    result.Should().NotBeNull();
                    result.StatusCode.Should().Be(HttpStatusCode.OK);
                    result.Content.Should().NotBeNull();

                    var resultBestRobot = JsonConvert.DeserializeObject<BestRobotForLoad>(
                        await result.Content.ReadAsStringAsync());

                    resultBestRobot.Should().NotBeNull();
                    int expectedRobotIndex = 3;
                    resultBestRobot.RobotId.Should().Be(robots[expectedRobotIndex].RobotId);

                    resultBestRobot.DistanceToGoal.Should().Be(
                        Math.Sqrt(
                            Math.Pow(robots[expectedRobotIndex].X - loadPayload.x, 2d)
                            + Math.Pow(robots[expectedRobotIndex].Y - loadPayload.y, 2d)));

                    resultBestRobot.BatteryLevel.Should().Be(robots[expectedRobotIndex].BatteryLevel);
                }
            }
        }
    }
}
