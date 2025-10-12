using System.Web.Mvc;

namespace YourAppName.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment/EnterInfo
        public ActionResult EnterInfo()
        {
            return View();
        }

        // POST: Payment/ProcessInfo (from your 'Enter Info' page)
        [HttpPost]
        public ActionResult ProcessInfo(FormCollection form)
        {
            // In a real app, save contact info, validate, etc.
            // Then redirect to the Payment view.
            return RedirectToAction("Payment");
        }

        // GET: Payment/Payment (This will now also check for a success message)
        public ActionResult Payment(bool paymentSuccess = false)
        {
            ViewBag.PaymentSuccess = paymentSuccess;
            return View();
        }

        // POST: Payment/ProcessPayment (from your 'Complete Payment' form)
        [HttpPost]
        public ActionResult ProcessPayment(FormCollection form)
        {
            // In a real application:
            // 1. Process the payment using a payment gateway (e.g., Stripe, PayPal, GCash API).
            // 2. Handle success or failure.
            // For this example, we'll just simulate success.

            // If payment is successful, redirect back to the Payment view
            // with a parameter to show the success modal.
            return RedirectToAction("Payment", new { paymentSuccess = true });
        }
    }
}