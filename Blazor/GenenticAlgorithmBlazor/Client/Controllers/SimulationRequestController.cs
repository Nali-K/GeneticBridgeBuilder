using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GenenticAlgorithmBlazor.Client.Interfaces;
using GenenticAlgorithmBlazor.Shared;
using Newtonsoft.Json;

namespace GenenticAlgorithmBlazor.Client.Controllers
{
    public class SimulationRequestController:ISimulationRequest
    {
        private HttpClient _httpClient;

        public SimulationRequestController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<SimulationAssigment> RequestSimulation(string page, SimulationAssigment assignment)
        {

            using (var client = new HttpClient())
            {
                

            //request.Headers.Add("Accept", "application/vnd.github.v3+json");
            //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            var jsonAssigment =JsonConvert.SerializeObject(assignment);
            var payload = new StringContent(jsonAssigment, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(new Uri("https://localhost:7141/"+page),payload);
            var test = await result.Content.ReadAsStringAsync();
            var id = 0;
            SimulationAssigment assignmentResult = null;
            if (int.TryParse(test,out id))
            {
                string resultString;
                
                while (assignmentResult == null)
                {
                    await Task.Delay(5000);
                    var url = "https://localhost:7141/" + page + "/get_by_id/" + id;
                    var resultGet = await client.GetAsync(new Uri(url));
                    resultString = await resultGet.Content.ReadAsStringAsync();
                    var boolResult = true;
                    var isbool = Boolean.TryParse(resultString,out boolResult);
                    if (!isbool)
                    {
                        assignmentResult = JsonConvert.DeserializeObject<SimulationAssigment>(resultString);
                        //Console.Write(assignmentResult.GetType());
                    }
                }
            }
            return assignmentResult;
            }
        }
    }
}