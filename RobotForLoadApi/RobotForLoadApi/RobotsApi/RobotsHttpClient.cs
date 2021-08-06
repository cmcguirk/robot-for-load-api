using Newtonsoft.Json;
using RobotForLoadApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RobotForLoadApi.RobotsApi
{
    public class RobotsHttpClient
    {
        private const string ROBOTS_GET_URL = "robots";
        private HttpClient _client;
        private RobotsHttpClientSettings _settings;

        public RobotsHttpClient(HttpClient client, RobotsHttpClientSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task<Robot[]> GetRobots(CancellationToken cancelToken = default)
        {
            try
            {
                using (var response = await _client.GetAsync(_settings.BaseUrl + "robots", cancelToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Robot[]>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (HttpRequestException)
            {
            }
            catch (OperationCanceledException)
            {
            }

            using (var response = await _client.GetAsync(_settings.FallbackUrl + "robots", cancelToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Robot[]>(await response.Content.ReadAsStringAsync());
                }
            }

            return null;
        }
    }
}
