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
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("DefaultConnection"); // Initialize connectionString

            Console.WriteLine("Store Items");

            // Display existing items
            DisplayItems();

            // Add a new item
            AddNewItem();

            // Send email
            SendEmail();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Method to display items from the database
        static void DisplayItems()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT itemCode, description, qty, price, amount FROM items";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("Item Code\tDescription\tQuantity\tPrice\tAmount");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["itemCode"]}\t{reader["description"]}\t{reader["qty"]}\t{reader["price"]}\t{reader["amount"]}");
                }

                reader.Close();
            }
        }

        // Method to add a new item to the database
        static void AddNewItem()
        {
            Console.WriteLine("\nAdd New Item");
            Console.Write("Enter Item Code: ");
            string itemCode = Console.ReadLine();
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Quantity: ");
            int qty = int.Parse(Console.ReadLine());
            Console.Write("Enter Price: ");
            double price = double.Parse(Console.ReadLine());
            double amount = qty * price;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO items (itemCode, description, qty, price, amount) " +
                               "VALUES (@itemCode, @description, @qty, @price, @amount)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@itemCode", itemCode);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@qty", qty);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@amount", amount);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) affected.");
            }
        }

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
