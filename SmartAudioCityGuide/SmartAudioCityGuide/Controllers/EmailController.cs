using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using System.Net.Mail;
using System.Net;
using Amazon.SimpleEmail.Model;

namespace SmartAudioCityGuide.Controllers
{
    public class EmailController : Controller
    {
        public void sendEmailToAuthenticateAUser(Users user)
        {
            try
            {
                System.Collections.Generic.List<string> listColl = new System.Collections.Generic.List<string>();
                ////TODO - Write a simple loop to add the recipents email addresses to the listColl object.
                listColl.Add(user.userName.ToString());

                Amazon.SimpleEmail.AmazonSimpleEmailServiceClient client = new Amazon.SimpleEmail.AmazonSimpleEmailServiceClient("AKIAJUPAMCIGTBC2ODXQ", "s7PkEfwVmhbzWT5PFeN5CV3ZzSPemgaaxnwa32pp");
                SendEmailRequest mailObj = new SendEmailRequest();

                Destination destinationObj = new Destination(listColl);

                mailObj.Source = "smartaudiocg@gmail.com";
                ////The from email address
                mailObj.ReturnPath = "smartaudiocg@gmail.com";
                ////The email address for bounces
                mailObj.Destination = destinationObj;

                string urlLink = "http://ec2-177-71-137-221.sa-east-1.compute.amazonaws.com/smartaudiocityguide/User/authenticateUser/?hash=" + user.hash;
                ////Create Message
                Amazon.SimpleEmail.Model.Content emailSubjectObj = new Amazon.SimpleEmail.Model.Content("Authentication for Smart Audio City Guide");
                Amazon.SimpleEmail.Model.Content emailBodyContentObj = new Amazon.SimpleEmail.Model.Content(@"<htm>
                <head>
                <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                <title>Smart Audio City Guide</title>
                </head>
                <body>
                <div>
                    <a> Welcome " + user.name + "to Smart Audio City Guide </a><br/><a>Click on the link to authenticate your account: <a href='"+urlLink+"'>"+ urlLink+ "</a></a></div></body>");

                Amazon.SimpleEmail.Model.Body emailBodyObj = new Amazon.SimpleEmail.Model.Body();
                emailBodyObj.Html = emailBodyContentObj;
                Message emailMessageObj = new Message(emailSubjectObj, emailBodyObj);
                mailObj.Message = emailMessageObj;

                dynamic response2 = client.SendEmail(mailObj);
            }
            catch (Exception)
            {

            }
        }

        public void sendPasswordToUser(string email, string password)
        {
            var fromAddress = new MailAddress("smartaudiocg@gmail.com", "Smart Audio City Guide");
            var toAddress = new MailAddress(email);
            string fromPassword = "3434g$br!el!";
            string subject = "Password for Smart Audio City Guide site";
            string body = "Your new password for Smart Audio City Guide is " + password ;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress.ToString(), toAddress.ToString(), subject, body);

            smtp.Send(message);

        }

        public void sendEmailToFollowUser(string email,string hash)
        {
            var fromAddress = new MailAddress("smartaudiocg@gmail.com", "Smart Audio City Guide");
            var toAddress = new MailAddress(email);
            string fromPassword = "smartAudio!";
            string subject = "Your friend want a help";
            string body = "Please click on link to follow your friend: http://smartaudiocityguide.azurewebsites.net/HelpMode/follow?hash=" + hash;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress.ToString(), toAddress.ToString(), subject, body);

            smtp.Send(message);

        }

    }
}
