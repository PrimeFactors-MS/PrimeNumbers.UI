using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrimeNumbers.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using TextJson = System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace PrimeNumbers.UI.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<PrimeCheckResponse> CheckPrime([FromBody] PrimeCheckRequest primeCheckRequest)
        {
            PrimeCheckResponse dbResponse = await GetPrimesFromDb(primeCheckRequest);
            if (dbResponse != null)
            {
                return dbResponse;
            }

            try
            {
                return await GetPrimesFromApi(primeCheckRequest);
            }
            catch (Exception e)
            {
                return new PrimeCheckResponse
                {
                    ErrorMessage = e.Message
                };
            }
        }

        private async Task<PrimeCheckResponse> GetPrimesFromApi(PrimeCheckRequest request)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://on-demand-finder-service:30005")
            };
            HttpResponseMessage response = await client.GetAsync($"OnDemandPrime?Number={request.Number}");

            if (response.IsSuccessStatusCode)
            {
                Stream responseBody = await response.Content.ReadAsStreamAsync();
                PrimeCheckApiResponse result = await TextJson.JsonSerializer.DeserializeAsync<PrimeCheckApiResponse>(responseBody);
                return new PrimeCheckResponse { IsPrime = result.IsPrime, Primes = result.Primes };
            }
            else
            {
                throw new Exception($"Api server returned {response.StatusCode.ToString()}");
            }
        }

        private async Task<PrimeCheckResponse> GetPrimesFromDb(PrimeCheckRequest request)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://primes-db-service:30006")
            };
            HttpResponseMessage response = await client.GetAsync($"Primes?number={request.Number}");

            if (response.IsSuccessStatusCode)
            {
                Stream responseBody = await response.Content.ReadAsStreamAsync();
                PrimeDbRecord result = await TextJson.JsonSerializer.DeserializeAsync<PrimeDbRecord>(responseBody);
                return new PrimeCheckResponse { IsPrime = result.IsPrime, Primes = result.PrimeFactors };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"Api server returned {response.StatusCode.ToString()}");
            }
        }
    }
}
