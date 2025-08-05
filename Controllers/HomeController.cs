using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.Common;
using P_CStore.Models;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace P_CStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient client;
        private string url;

        public HomeController(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            int page = 1; // Página inicial
            int pageSize = 10;

            List<IndexModel> lstProduct = new List<IndexModel>();

            url = $"https://localhost:7049/Store/IndexView/{page}/{pageSize}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                lstProduct = JsonConvert.DeserializeObject<List<IndexModel>>(content);
            }

            return View(lstProduct);
        }

        public async Task<IActionResult> ProductView(int idProduct)
        {
            ProductViewModel product = new ProductViewModel();
             url = $"https://localhost:7049/Store/ProductDetails/{idProduct}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                product = JsonConvert.DeserializeObject<ProductViewModel>(content);
            }
            return View(product);
        }

        public async Task<IActionResult> LoadMoreProducts(int page, int pageSize)
        {
            List<IndexModel> lstProduct = new List<IndexModel>();

            url = $"https://localhost:7049/Store/IndexView/{page}/{pageSize}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                lstProduct = JsonConvert.DeserializeObject<List<IndexModel>>(content);
            }

            return Json(lstProduct);
        }

       

    }
}
