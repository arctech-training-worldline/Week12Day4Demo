using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Week12Day4DemoApi.Dtos;

namespace WebApplication1.Controllers
{
    public class HotelsController : Controller
    {
        private const string MediaType = "application/json";
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(IHttpClientFactory clientFactory, ILogger<HotelsController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        // GET: HotelsController
        public async Task<ActionResult> Index()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44311/api/Hotels");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var hotels = JsonConvert.DeserializeObject<List<HotelDto>>(content);

                return View(hotels);
            }

            _logger.LogWarning("Hotels Get Api : expected HTTP OK but got {ResponseCode}", response.StatusCode);
            return View();
        }

        // GET: HotelsController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44311/api/Hotels/{id}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var hotels = JsonConvert.DeserializeObject<HotelDto>(content);

                return View(hotels);
            }

            _logger.LogWarning("Hotels Get Api : expected HTTP OK but got {ResponseCode}", response.StatusCode);
            return View();
        }

        // GET: HotelsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HotelsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(HotelDto hotel)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44311/api/Hotels");
                
                var jsonText = JsonConvert.SerializeObject(hotel);
                request.Content = new StringContent(jsonText, Encoding.UTF8, MediaType);

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if(response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: HotelsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44311/api/Hotels/{id}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var hotel = JsonConvert.DeserializeObject<HotelDto>(content);

                return View(hotel);
            }

            _logger.LogWarning("Hotels Get Api : expected HTTP OK but got {ResponseCode}", response.StatusCode);
            return View();
        }

        // POST: HotelsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, HotelDto hotel)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44311/api/Hotels/{id}");

                var jsonText = JsonConvert.SerializeObject(hotel);
                request.Content = new StringContent(jsonText, Encoding.UTF8, MediaType);

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Hotels Put Api : expected HTTP OK but got {ResponseCode}", response.StatusCode);
                return View();
            }
            catch
            {
                return View();
            }
        }

        //GET: HotelsController/Delete/5
        
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44311/api/Hotels/{id}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var hotels = JsonConvert.DeserializeObject<HotelDto>(content);

                return View(hotels);
            }

            _logger.LogWarning("Hotels Get Api : expected HTTP OK but got {ResponseCode}", response.StatusCode);
            return View();
        }

        // POST: HotelsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44311/api/Hotels/{id}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Hotels Put Api : expected HTTP OK but got {ResponseCode}", response.StatusCode);
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
