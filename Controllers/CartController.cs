using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P_CStore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

namespace P_CStore.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient client;
        private string url;

        public CartController(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<IActionResult> Cart()
        {
            int idUser;
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Please sign in First" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            idUser = int.Parse(id.Value);
            List<CartViewModel> cart = new List<CartViewModel>();
            url = $"https://localhost:7049/Store/ViewCart/{idUser}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                cart = JsonConvert.DeserializeObject<List<CartViewModel>>(content);
            }
            return View(cart);

        }


        public async Task<IActionResult> AddToCart([FromBody] CartModel cartModel)
        {

            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Please sign in First" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var idUser = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            cartModel.IdUser = int.Parse(idUser.Value);

            url = "https://localhost:7049/Store/AddToCart";

            var content = new StringContent(JsonConvert.SerializeObject(cartModel), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Product Add To Cart Succesfully" });
            }
            else
            {
                return Json(new { success = false, message = "Error Adding to Cart" });
            }



        }

        public async Task<IActionResult> DeleteProductFromCart(int idProduct)
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Please sign in first." });
            }


            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            DeleteCartItemDTO item = new DeleteCartItemDTO();
            item.IdUser = int.Parse(userId.Value);
            item.IdProduct = idProduct;

            url = "https://localhost:7049/Store/DeleteProductFromCart";

            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                return Json(new { success = false, message = "Error Deleting the item" });
            }

        }

        public async Task<IActionResult> UpdateQuantity(int idProduct, int quantity)
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Please sign in first." });
            }


            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            CartModel item = new CartModel();
            item.IdUser = int.Parse(userId.Value);
            item.IdProduct = idProduct;
            item.Quantity = quantity;

            url = "https://localhost:7049/Store/UpdateQuantity";

            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Cart Updated" });
                }
                else
                {
                    return Json(new { success = false, message = "Error updating the item" });
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating the item" });
            }


        }


    }
}
