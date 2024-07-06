using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeWebApp.Models;
using System.Diagnostics;

namespace StripeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult Charge(string stripeEmail,string stripeToken)
        //{
        //    var customers = new CustomerService();
        //    var charges = new ChargeService();

        //    var customer = customers.Create(new CustomerCreateOptions
        //    {
        //        Email = stripeEmail,
        //        Source = stripeToken,
        //    });

        //    var charge = charges.Create(new ChargeCreateOptions
        //    {
        //        Amount = 500,
        //        Description="Test Payment",
        //        Currency = "usd",
        //        Customer = customer.Id,
        //        ReceiptEmail = stripeEmail,
        //        Metadata= new Dictionary<string, string>
        //        {
        //            { "OrderId","111" },
        //            {"PostCode","27010" }
        //        }
                
        //    });


        //    if(charge.Status == "succeeded")
        //    {
        //        string BalanceTransactionId = charge.BalanceTransactionId;
        //        return View();
        //    }
        //    else
        //    {

        //    }


        //    return View();
        //}

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = request.Items.Sum(item => item.Amount),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        public class CreatePaymentIntentRequest
        {
            public List<Item> Items { get; set; }
        }

        public class Item
        {
            public string Id { get; set; }
            public int Amount { get; set; }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
