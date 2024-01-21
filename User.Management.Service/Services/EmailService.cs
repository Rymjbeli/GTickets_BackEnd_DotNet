using MimeKit;
using MailKit.Net.Smtp;
using User.Management.Service.Models;
using static System.Net.WebRequestMethods;

namespace User.Management.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _configuration;
        public EmailService(EmailConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(Message message, string emailType)
        {
            if( emailType == "verif")
            {
                var emailMessage = CreateEmailMessageVerification(message);
                Send(emailMessage);
            } else if (emailType == "reset")
            {
                var emailMessage = CreateEmailMessageResetPassword(message);
                Send(emailMessage);

            } else if (emailType == "newTicket")
            {
                var emailMessage = CreateEmailMessageNewTicket(message);
                Send(emailMessage);

            }else if (emailType == "newReply")
            {
                var emailMessage = CreateEmailMessageNewReply(message);
                Send(emailMessage);

            }



        }
        public MimeMessage CreateEmailMessageVerification(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("G_Tickets", _configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder();

            // Add a bold title
            bodyBuilder.HtmlBody = $"<h1>Welcome to G_Tickets</h1>\n\n";

            // Add text before the link
            bodyBuilder.HtmlBody += "<h4>Please click the link below to verify your email address:</h4>\n\n";

            // Add the link as a button
            bodyBuilder.HtmlBody += $"<a href=\"{message.Content}\" style=\"padding:10px; background-color: black; color: white; border-radius: 10px; text-decoration: none\"> Verify Email</a>\n\n";

            // Add information about link validity
            bodyBuilder.HtmlBody += "<h4>The link is valid for 24 hours.</h4>";

            // Attach the body to the email message
            emailMessage.Body = bodyBuilder.ToMessageBody();

            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) {  Text = message.Content };
            return emailMessage;
        }

        public MimeMessage CreateEmailMessageResetPassword(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("G_Tickets", _configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder();


            // Add a bold title
            bodyBuilder.HtmlBody = $"<h1>Forgot your password?</h1>\n\n";

            // Add text before the link
            bodyBuilder.HtmlBody += "<h4>Please Click the link below to reset your password:</h4>\n\n";

            // Add the link as a button
            bodyBuilder.HtmlBody += $"<a href=\"{message.Content}\" style=\"padding:10px; background-color: black; color: white; border-radius: 10px; text-decoration: none\"> Reset Password</a>\n\n";

            // Add information about link validity
            bodyBuilder.HtmlBody += "<h4>The link is valid for 24 hours.</h4>";

            // Attach the body to the email message
            emailMessage.Body = bodyBuilder.ToMessageBody();

            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) {  Text = message.Content };
            return emailMessage;
        }

        public MimeMessage CreateEmailMessageNewTicket(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("G_Tickets", _configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder();

            /*var contentLines = message.Content.Split('\n');
            var ticketCustomer = contentLines.FirstOrDefault(line => line.StartsWith("Ticket details:"))?.Replace("Ticket details:", "").Trim();
            var HelpDeskTableLink = contentLines.FirstOrDefault(line => line.StartsWith("Dashboard Link:"))?.Replace("Dashboard Link:", "").Trim();
            */

            // Add a bold title
            bodyBuilder.HtmlBody = $"<h1>New Ticket Added!</h1>\n\n";
            bodyBuilder.HtmlBody += $"<p>A new ticket from the user <strong>{message.Content}</strong> has been added.</p>\n\n";

            // Add text before the link
            bodyBuilder.HtmlBody += "<h4>Please click the link below to view details in the Help Desk table:</h4>\n\n";

            // Add the link as a button
            bodyBuilder.HtmlBody += $"<a href=\"http://localhost:4200/tickets/all\" style=\"padding:10px; background-color: black; color: white; border-radius: 10px; text-decoration: none\"> View in Help Desk Table</a>\n\n";

            // Attach the body to the email message
            emailMessage.Body = bodyBuilder.ToMessageBody();

            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) {  Text = message.Content };
            return emailMessage;
        }

        public MimeMessage CreateEmailMessageNewReply(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("G_Tickets", _configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder();

            var contentLines = message.Content.Split('\n');
            var userReply = contentLines.FirstOrDefault(line => line.StartsWith("User reply:"))?.Replace("User reply:", "").Trim();
            var ticketTitle = contentLines.FirstOrDefault(line => line.StartsWith("Ticket Title:"))?.Replace("Ticket Title:", "").Trim();
            var ticketId = contentLines.FirstOrDefault(line => line.StartsWith("Ticket Id:"))?.Replace("Ticket Id:", "").Trim();


            // Add a bold title
            bodyBuilder.HtmlBody = $"<h1>New Reply Added!</h1>\n\n";
            bodyBuilder.HtmlBody += $"<p>A new reply from <strong>{userReply}</strong> for the ticket with the title <strong>{ticketTitle}</strong> has been added.</p>\n\n";

            // Add text before the link
            bodyBuilder.HtmlBody += "<h4>Please click the link below to view the details of the ticket:</h4>\n\n";

            // Add the link as a button
            bodyBuilder.HtmlBody += $"<a href=\"http://localhost:4200/ticketDetails/{ticketId}\" style=\"padding:10px; background-color: black; color: white; border-radius: 10px; text-decoration: none\"> View in Ticket details</a>\n\n";

            // Attach the body to the email message
            emailMessage.Body = bodyBuilder.ToMessageBody();

            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) {  Text = message.Content };
            return emailMessage;
        }


        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_configuration.SmtpServer, _configuration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_configuration.UserName, _configuration.Password);
                client.Send(mailMessage);

            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();

            }

        }
    }
}
