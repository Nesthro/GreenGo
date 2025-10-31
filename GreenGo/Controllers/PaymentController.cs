using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GreenGo.Models;
using System.Net.Mail;
using System.Net;

namespace GreenGo.Controllers
{
    public class PaymentController : Controller
    {
        // Step 1: Show Enter Info page
        [HttpGet]
        public ActionResult EnterInfo(string placeName = "", decimal? totalFee = null, string cartDataJson = "")
        {
            var model = new PaymentViewModel
            {
                PlaceName = placeName,
                TotalFee = totalFee ?? 0,
                CartDataJson = cartDataJson
            };

            return View(model);
        }

        // Step 2: Process info and redirect to payment page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessPayment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EnterInfo", model);
            }

            // Store user info in session for later use
            Session["UserInfo"] = new UserInfoSession
            {
                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                PhoneCountryCode = model.PhoneCountryCode,
                PhoneNumber = model.PhoneNumber
            };

            // Store payment details in session
            Session["PaymentDetails"] = new PaymentDetailsSession
            {
                PlaceName = model.PlaceName,
                TotalFee = model.TotalFee,
                CartDataJson = model.CartDataJson
            };

            // Redirect to payment page
            return RedirectToAction("CompletePayment");
        }

        // Step 3: Show payment page
        [HttpGet]
        public ActionResult CompletePayment()
        {
            var paymentDetails = Session["PaymentDetails"] as PaymentDetailsSession;

            if (paymentDetails == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new PaymentViewModel
            {
                PlaceName = paymentDetails.PlaceName,
                TotalFee = paymentDetails.TotalFee,
                CartDataJson = paymentDetails.CartDataJson
            };

            return View(model);
        }

        // Step 4: Process card payment and send verification code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessCardPayment(CardPaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all payment details correctly.";
                return RedirectToAction("CompletePayment");
            }

            var userInfo = Session["UserInfo"] as UserInfoSession;

            if (userInfo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Generate 4-digit verification code
            Random random = new Random();
            string verificationCode = random.Next(1000, 9999).ToString();

            // Store verification code and payment info in session
            Session["VerificationCode"] = verificationCode;
            Session["VerificationCodeExpiry"] = DateTime.Now.AddMinutes(10);
            Session["CardPaymentInfo"] = model;

            // Send verification email
            try
            {
                SendVerificationEmail(userInfo.Email, verificationCode, userInfo.Name);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email error: " + ex.Message);
            }

            // Redirect to verification page
            return RedirectToAction("EmailVerification");
        }

        // Step 5: Show email verification page
        [HttpGet]
        public ActionResult EmailVerification()
        {
            if (Session["VerificationCode"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Verify the code entered by user
        [HttpPost]
        public JsonResult VerifyCode(string code)
        {
            var storedCode = Session["VerificationCode"] as string;
            var expiry = Session["VerificationCodeExpiry"] as DateTime?;

            if (storedCode == null || expiry == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            if (DateTime.Now > expiry.Value)
            {
                return Json(new { success = false, message = "Verification code expired" });
            }

            if (code == storedCode)
            {
                // Code is correct - process the payment
                bool paymentSuccess = ProcessPaymentTransaction();

                if (paymentSuccess)
                {
                    // Clear sensitive session data
                    Session.Remove("VerificationCode");
                    Session.Remove("VerificationCodeExpiry");
                    Session.Remove("CardPaymentInfo");

                    // Store booking confirmation
                    SaveBookingConfirmation();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Payment processing failed" });
                }
            }

            return Json(new { success = false, message = "Invalid verification code" });
        }

        // Resend verification code
        [HttpPost]
        public JsonResult ResendCode()
        {
            var userInfo = Session["UserInfo"] as UserInfoSession;

            if (userInfo == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            // Generate new code
            Random random = new Random();
            string verificationCode = random.Next(1000, 9999).ToString();

            // Update session
            Session["VerificationCode"] = verificationCode;
            Session["VerificationCodeExpiry"] = DateTime.Now.AddMinutes(10);

            // Send email
            try
            {
                SendVerificationEmail(userInfo.Email, verificationCode, userInfo.Name);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email error: " + ex.Message);
                return Json(new { success = false, message = "Failed to send email" });
            }
        }

        // Helper method to send verification email
        private void SendVerificationEmail(string toEmail, string code, string userName)
        {
            // Configure your SMTP settings here
            string fromEmail = "noreply@greengo.com";
            string fromPassword = "your-email-password";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = "GreenGo Payment Verification Code";
            mail.Body = "<html><body style='font-family: Arial, sans-serif;'><h2>Payment Verification</h2><p>Hello " + userName + ",</p><p>Your verification code is:</p><h1 style='color: #7CB342; font-size: 32px; letter-spacing: 5px;'>" + code + "</h1><p>This code will expire in 10 minutes.</p><p>If you didn't request this code, please ignore this email.</p><br><p>Best regards,<br>GreenGo Team</p></body></html>";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }

        // Helper method to process payment transaction
        private bool ProcessPaymentTransaction()
        {
            var cardInfo = Session["CardPaymentInfo"] as CardPaymentModel;
            var userInfo = Session["UserInfo"] as UserInfoSession;
            var paymentDetails = Session["PaymentDetails"] as PaymentDetailsSession;

            if (cardInfo == null || userInfo == null || paymentDetails == null)
            {
                return false;
            }

            try
            {
                // TODO: Integrate with actual payment gateway
                System.Threading.Thread.Sleep(1000);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Payment error: " + ex.Message);
                return false;
            }
        }

        // Helper method to save booking confirmation
        private void SaveBookingConfirmation()
        {
            var userInfo = Session["UserInfo"] as UserInfoSession;
            var paymentDetails = Session["PaymentDetails"] as PaymentDetailsSession;

            if (userInfo == null || paymentDetails == null)
            {
                return;
            }

            // TODO: Save to database

            try
            {
                SendConfirmationEmail(userInfo.Email, userInfo.Name, paymentDetails.PlaceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Confirmation email error: " + ex.Message);
            }
        }

        // Helper method to send booking confirmation email
        private void SendConfirmationEmail(string toEmail, string userName, string placeName)
        {
            string fromEmail = "noreply@greengo.com";
            string fromPassword = "your-email-password";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = "Booking Confirmation - GreenGo";
            mail.Body = "<html><body style='font-family: Arial, sans-serif;'><h2>Booking Confirmed!</h2><p>Hello " + userName + ",</p><p>Your booking at <strong>" + placeName + "</strong> has been confirmed.</p><p>Thank you for choosing GreenGo!</p><br><p>Best regards,<br>GreenGo Team</p></body></html>";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}