using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;

namespace MailboxService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //注册EncodingProvider实现对中文编码的支持
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            //SendMail();


            FetchMails();
        }

        private static void SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ravenzz", "zhuyingjunjun@foxmail.com"));
            message.To.Add(new MailboxAddress("ravenzz", "ravenzz@aliyun.com"));
            message.Subject = "How you doin'?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Raven,
I just wanted to let you know that Monica and I were going to go play some paintball, you in?
-- Joey",
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.qq.com", 465, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("zhuyingjunjun@foxmail.com", "nnofhlbubmhbbcib");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        private static void FetchMails()
        {
            using (var client = new ImapClient())
            {
                client.Timeout = 100000;
                client.Connect("imap.qq.com", 993, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate("zhuyingjunjun@foxmail.com", "nnofhlbubmhbbcib");
                //var personal = client.GetFolder(client.PersonalNamespaces[0]);

                //foreach (var folder in personal.GetSubfolders(false))
                //{
                //    var subFolders = folder.GetSubfolders(false);
                //    foreach (var subFolder in subFolders)
                //    {
                //        Console.WriteLine("[subFolder] {0}", subFolder.Name);
                //    }
                //    Console.WriteLine("[folder] {0}", folder.Name);
                //}


                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;

                inbox.Open(FolderAccess.ReadWrite);

                //Console.WriteLine("Total messages: {0}", inbox.Count);
                //Console.WriteLine("Recent messages: {0}", inbox.Recent);

                //for (int i = 0; i < inbox.Count; i++)
                //{
                //    var message = inbox.GetMessage(i);
                //    Console.WriteLine("Subject: {0}", message.Subject);
                //    //Console.WriteLine("Body: {0}",message.Body);
                //    Console.WriteLine("TextBody: {0}", message.TextBody);
                //    //Console.WriteLine("HtmlBody: {0}", message.HtmlBody);
                //}

                foreach (var uid in inbox.Search(SearchQuery.NotSeen))
                {
                    var message = inbox.GetMessage(uid);
                    Console.WriteLine("Subject: {0}", message.Subject);
                    Console.WriteLine("Body: {0}",message.Body);
                    //Console.WriteLine("TextBody: {0}", message.TextBody);
                    //Console.WriteLine("HtmlBody: {0}", message.HtmlBody);

                }
                //var list = inbox.Fetch(100, 2, MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
                //foreach (var summary in list)
                //{
                //    var flag =summary.Flags;
                //    Console.WriteLine($"[flag] "+flag);
                //    Console.WriteLine("[summary] {0:D2}: {1}", summary.Index, summary.Envelope.Subject);
                //}
                //Console.WriteLine($"Count : {list.Count}");
                client.Disconnect(true);
            }
        }
    }


}
