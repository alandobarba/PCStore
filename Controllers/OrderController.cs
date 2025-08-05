using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P_CStore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace P_CStore.Controllers
{
    public class OrderController : Controller
    {
        private string url;
        private HttpClient client;

        public OrderController(HttpClient client)
        {
            this.client = client;
        }
        public async Task<IActionResult> MyOrders()
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Please sign in First" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int idUser = int.Parse(id.Value);

            List<OrderListDTO> lstOrderList = new List<OrderListDTO>();
            url = "https://localhost:7049/AllOrders/" + idUser;
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                lstOrderList = JsonConvert.DeserializeObject<List<OrderListDTO>>(content);
            }
            return View(lstOrderList);
        }

        public async Task<IActionResult> OrderDetails(int idTransaction)
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Please sign in First" });
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var idUser = int.Parse(id.Value);

            List<OrderDetailsDTO> lstOrderDetails = new List<OrderDetailsDTO>();
            url = $"https://localhost:7049/OrderDetail/{idUser}/{idTransaction}";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                lstOrderDetails = JsonConvert.DeserializeObject<List<OrderDetailsDTO>>(content);
            }
            return View(lstOrderDetails);
        }

    }
}
