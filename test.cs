using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace StoreInventoryApp
{
    class Program
    {
        static string connectionString; // Declare connectionString as static

        static void Main(string[] args)
        {
          
          // Method to send email
          static void SendEmail()
          {
              string senderEmail = "your_email@example.com";
              string receiverEmail = "recipient_email@example.com";
              string subject = "Store Inventory Updated";
              string body = "The store inventory has been updated. Please check.";
  
              using (MailMessage mail = new MailMessage(senderEmail, receiverEmail))
              {
                  mail.Subject = subject;
                  mail.Body = body;
  
                  using (SmtpClient smtp = new SmtpClient("smtp.example.com"))
                  {
                      smtp.Port = 587;
                      smtp.Credentials = new NetworkCredential("your_smtp_username", "your_smtp_password");
                      smtp.EnableSsl = true;
                      smtp.Send(mail);
                  }
              }
          }
      }
   }
