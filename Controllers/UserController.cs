using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P_CStore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P_CStore.Controllers
{
    public class UserController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        public int GetIdUser()
        {
            int idUser = 0;
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return idUser;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            idUser = int.Parse(id.Value);
            return idUser;

        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserModel model)
        {
            string apiUrl = "https://localhost:7049/signin";

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Crear HttpClient y hacer la solicitud POST
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer el token de la respuesta
                    string token = await response.Content.ReadAsStringAsync();

                    Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true, // Esto evita que JavaScript acceda a la cookie
                        /* Secure = true,*/   // Si estás en producción, asegúrate de usar HTTPS
                        SameSite = SameSiteMode.Strict // Para prevenir ataques CSRF
                    });

                    // Redirigir a la página protegida o al inicio
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["messageError"] = "invalid credentials";
                    return RedirectToAction("SignIn", "User");
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserModel model)
        {

            if (model.Password != model.ConfirmPassword)
            {
                TempData["messageError"] = "Password did not match";
                return RedirectToAction("SignUp", "User");
            }


            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                TempData["messageError"] = "the username and password cannot be empty";
                return RedirectToAction("Signup", "User");
            }

            model.Role = "Customer";


            string url = "https://localhost:7049/signupCustomer";

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
               
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SignIn");
                }
                else
                {
                    TempData["messageError"] = "Error: user may exists already";
                    return RedirectToAction("SignUp", "User");
                }

            }

        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("SignIn", "User");
        }
    }
}
