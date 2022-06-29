using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Helpers
{
    public class EmailSender
    {
        public string Email { get; }
        private const string FAW_Email = "jwca.mcl2020@gmail.com";
        private const string FAW_Password = "passwordmcl";

        public EmailSender(string email)
        {
            Email = email;
        }

        public void SendCode(string code)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(FAW_Email, FAW_Password),
                EnableSsl = true,
            };

            smtpClient.Send(FAW_Email, Email, "Code Confirmation for Flame and Wax", code);
        }
        public string RandomString()
        {
            //int size = 5;
            //bool lowerCase = false;
            //var builder = new StringBuilder(size);

            //char offset = lowerCase ? 'a' : 'A';
            //const int lettersOffset = 26;
            //Random _random = new Random();
            //for (var i = 0; i < size; i++)
            //{
            //    var @char = (char)_random.Next(offset, offset + lettersOffset);
            //    builder.Append(@char);
            //}

            //return lowerCase ? builder.ToString().ToLower() : builder.ToString();
            return "Hello";
        }
    }
}
