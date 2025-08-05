
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using P_CStore.Models;
using P_CStore.Stripe;
using Stripe;
using Stripe.Issuing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace P_CStore.Controllers
{
    public class StripeController : Controller
    {
        private readonly string publicKey;
        private readonly HttpClient client;
        private string url;

        public StripeController(IOptions<StripeSettings> stripeOptions, HttpClient client)
        {
            publicKey = stripeOptions.Value.PublicKey;
            this.client = client;
        }


        public async Task<IActionResult> CheckOut(int idUser)
        {

            url = $"https://localhost:7049/Store/ViewCart/{idUser}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var checkOutModel = new CheckOutModel();
                checkOutModel.lstCart = JsonConvert.DeserializeObject<List<CartViewModel>>(content);
                ViewBag.StripePublicKey = publicKey;
                decimal total = 0;
                decimal tax;
                foreach (CartViewModel model in checkOutModel.lstCart)
                {
                    tax = Convert.ToDecimal(model.Quantity) * model.Price * 0.13m;
                    total = total + Convert.ToDecimal(model.Quantity) * model.Price + tax;
                }
                checkOutModel.stripeModel = new StripeModel();
                checkOutModel.stripeModel.Amount = total;
                checkOutModel.stripeModel.Currency = "cad";

                return View(checkOutModel);
            }
            else
            {
                return Json(new { success = false, message = "Temporal error, try again later" });
            }
        }

        public IActionResult SuccessPayment(int idTransaction)
        {
            ViewBag.IdTransaction = idTransaction;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CheckOutModel model)
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

            List<CartViewModel> lstCart = new List<CartViewModel>();
            url = $"https://localhost:7049/Store/ViewCart/{idUser}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                lstCart = JsonConvert.DeserializeObject<List<CartViewModel>>(content);
                ViewBag.StripePublicKey = publicKey;
                decimal total = 0;
                decimal tax;
                foreach (CartViewModel cartModel in lstCart)
                {
                    tax = Convert.ToDecimal(cartModel.Quantity) * cartModel.Price * 0.13m;
                    total = total + Convert.ToDecimal(cartModel.Quantity) * cartModel.Price + tax;
                }
                total = Math.Round(total, 2, MidpointRounding.AwayFromZero);
                var checkOutModel = new CheckOutModel
                {
                    stripeModel = new StripeModel
                    {
                        Amount = total,
                        Currency = "cad"
                    }
                };
                if (Math.Abs(checkOutModel.stripeModel.Amount - model.stripeModel.Amount) < 0.01m)
                {
                    CustomerDTO customerDto = new CustomerDTO();
                    StripeModel stripeModel = new StripeModel();
                    customerDto = model.customer;
                    stripeModel = model.stripeModel;
                    CustomerService customerService = new CustomerService();
                    ChargeService service = new ChargeService();

                    var stripeCustomer = await customerService.CreateAsync(new CustomerCreateOptions
                    {
                        Email = customerDto.Email,
                        Name = $"{customerDto.Name} {customerDto.LastName}",
                        Source = stripeModel.StripeToken
                    });

                    var options = new ChargeCreateOptions
                    {
                        Amount = (long)(Math.Round(stripeModel.Amount * 100, 0)),
                        Currency = "cad",
                        Customer = stripeCustomer.Id,
                        Description = "Payment from P&C"
                    };

                    Charge charge = await service.CreateAsync(options);

                    if (charge.Status == "succeeded")
                    {
                        url = "https://localhost:7049/Store/CreateTransaction";
                        var contenido = new StringContent(JsonConvert.SerializeObject(idUser), Encoding.UTF8, "application/json");
                        HttpResponseMessage respuesta = await client.PostAsync(url, contenido);
                        var transactionResponse = await respuesta.Content.ReadAsStringAsync();
                        int idTransaction = int.Parse(transactionResponse);

                        url = "https://localhost:7049/Store/Sale";
                        List<SaleModel> lstSale = new List<SaleModel>();
                        SaleModel sale;
                        foreach (CartViewModel cart in lstCart)
                        {
                            sale = new SaleModel();
                            sale.IdProduct = cart.IdProduct;
                            sale.Quantity = cart.Quantity;
                            sale.IdTransaction = idTransaction;
                            sale.Payment = (cart.Quantity * cart.Price) * 0.13m + (cart.Quantity * cart.Price);
                            lstSale.Add(sale);
                        }
                         contenido = new StringContent(JsonConvert.SerializeObject(lstSale), Encoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                        await client.PostAsync(url, contenido);


                        customerDto.Brand = charge.PaymentMethodDetails.Card.Brand;
                        customerDto.Last4Digits = charge.PaymentMethodDetails.Card.Last4;
                        customerDto.PaymentMethod = charge.PaymentMethod; // ID del método de pago

                        // StripeCustomerId solo está disponible si creas un cliente
                        customerDto.StripeCustomerId = stripeCustomer.Id;


                        customerDto.IdUser = idUser;

                        url = "https://localhost:7049/SaveCustomer";

                        contenido = new StringContent(JsonConvert.SerializeObject(customerDto), Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                        respuesta = await client.PostAsync(url, contenido);

                       
                        if (respuesta.IsSuccessStatusCode)
                        {
                            
                            return RedirectToAction("SuccessPayment", new { idTransaction = idTransaction });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Error Processing Payment" });
                        }

                    }
                    else
                    {
                        return Json(new { success = false, message = "Error Processing Payment. try again later." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid amount. Payment not processed." });
                }

            }
            else
            {
                return Json(new { success = false, message = "Temporal error, try again later" });
            }

        }
    }
}
