using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using AE.Net.Mail;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;

namespace EService
{
    public class Service : IEmailService
    {
        public void SendMessage(string userName, string password, string sendTo, string title, string body,
            List<string> attachment)
        {
            var smtpClient = new SmtpClient();
            var mailMessage = new MailMessage();
            var basicCredits = new NetworkCredential(userName, password);
            var fromAddress = new MailAddress(userName);

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredits;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            mailMessage.From = fromAddress;
            mailMessage.Subject = title;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;

            if (attachment != null)
            {
                foreach (var fileToAttach in attachment)
                    mailMessage.Attachments.Add(new Attachment(fileToAttach));
                mailMessage.To.Add(sendTo);
            }
            else
            {
                mailMessage.To.Add(sendTo);
            }

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public ObservableCollection<string> ViewMail(string userName, string password)
        {
            var imapClient = new ImapClient("imap.gmail.com", userName, password, AuthMethods.Login, 993, true);
            imapClient.SelectMailbox("INBOX");
            var titleToShow = imapClient.GetMessages(0, 20).Select(x => x.Subject).ToList();
            return new ObservableCollection<string>(titleToShow.ToList());
        }

        public void DeleteMail(string userName, string password, int selectedMessage)
        {
            var imapClient = new ImapClient("imap.gmail.com", userName, password, AuthMethods.Login, 993, true);
            imapClient.SelectMailbox("INBOX");
            var messageToDelete = imapClient.GetMessage(selectedMessage);

            try
            {
                imapClient.DeleteMessage(messageToDelete);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string ViewMessageBody(string userName, string password, int selectedMessage)
        {
            var imapClient = new ImapClient("imap.gmail.com", userName, password, AuthMethods.Login, 993, true);
            imapClient.SelectMailbox("INBOX");
            var bodyToShow = imapClient.GetMessage(selectedMessage).Body;
            return bodyToShow;
        }
    }
}