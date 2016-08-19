using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
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
            var timer = new Timer(timerCallback, null, 2000, Timeout.Infinite);

            Console.ReadLine();



        }

        private static void timerCallback(object state)
        {
            try
            {
                FetchMails();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                InitImap();
            }
            new Timer(timerCallback, null, 2000, Timeout.Infinite);
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

        private static ImapClient client;

        private static void FetchMails()
        {
            if (client == null)
            {
                InitImap();
            }


            client.Timeout = 100000;
            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.

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

            inbox.Open(FolderAccess.ReadOnly);

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
                //Console.WriteLine("Body: {0}",message.Body);
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
            //  client.Disconnect(false);

        }

        private static void InitImap()
        {
            client = new ImapClient();
            client.Connect("imap.qq.com", 993, true);
            client.Disconnected += Client_Disconnected;
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate("zhuyingjunjun@foxmail.com", "nnofhlbubmhbbcib");
        }

        private static void Client_Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine("DisConnected");
            client = null;
            //client.Connect("imap.qq.com", 993, true);
        }

        private static void FetchPop3()
        {
            using (var client = new Pop3Client())
            {
                client.Connect("pop.friends.com", 110, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate("joey", "password");

                for (int i = 0; i < client.Count; i++)
                {
                    
                    var message = client.GetMessage(i);
                    Console.WriteLine("Subject: {0}", message.Subject);
                }

                client.Disconnect(true);
            }
        }


    }


}
