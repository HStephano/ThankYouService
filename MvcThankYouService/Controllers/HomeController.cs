using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;



namespace MvcTesting.Controllers
{
    public class HomeController : Controller
    {

        //Executes upon page load
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //Executes on Submit button press
        public ActionResult Index(string toAddress, string message)
        {


            //Generate new bodybuilder for email content
            BodyBuilder builder = new();

            //Body builder takes preset email format from files, temporary file location for testing
            using (StreamReader SourceReader = System.IO.File.OpenText("../MvcThankYouService/EmailFormat/email.html"))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            builder.HtmlBody = builder.HtmlBody.Replace("{message}", message);



            //Generate new Mime message and fill body content with email format
            var newMessage = new MimeMessage();
            newMessage.From.Add(MailboxAddress.Parse("hstephanotest12@gmail.com"));
            newMessage.To.Add(MailboxAddress.Parse(toAddress));
            newMessage.Subject = "Thank you!";
            newMessage.Body = builder.ToMessageBody();


            //Connect to gmail and authenticate test account (Bypass OAuth with App Password)
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("hstest718@gmail.com", "wvidjhzwmzwbjudy");
            smtp.Send(newMessage);
            smtp.Disconnect(true);

            //Redirects user to a submission confirmation
            return View("EmailSent");
        }
    }
}