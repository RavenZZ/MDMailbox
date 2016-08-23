using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailbox.Business;
using Mailbox.Dal;
using Mailbox.Entity;
using Mailbox.Entity.Enum;
using MailKit;


namespace Mailbox.Service
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Program Start");
            
            var raven = new Entity.AccountBinding()
            {
                AccountId = "f56d42b3-a75a-4eb0-885f-8b7a738581b9",
                EnableSSL = true,
                Mail = "zhuyingjunjun@foxmail.com",
                Port = 993,
                Server = "imap.qq.com",
         
                Type = MailBindingType.Imap
            };

            Imap imap = new Imap(raven);
            Console.WriteLine("Imap Init Start");
            imap.Init();
            Console.WriteLine("Imap FetchNotSeen Start");
            var unseen = imap.FetchNotSeen();
            Console.WriteLine("Imap FetchNotSeen End");

            foreach (KeyValuePair<UniqueId, string> pair in unseen)
            {
                Console.WriteLine("Mail");
                Console.WriteLine($"uid  {pair.Key}");
                Console.WriteLine($"Subject {pair.Value}");

                var query =
                    from p in MailMongoContext.Instance.Notifications
                    where p.UniqueId == pair.Key.ToString() && p.Email == raven.Mail
                    select p;
                if (!query.Any())
                {
                    Console.WriteLine("Has Not Notice");
                    var notification = new Notification()
                    {
                        Email = raven.Mail,
                        Status = NotifyStatus.Success,
                        UniqueId = pair.Key.ToString()
                    };
                    MailMongoContext.Instance.NotificationCollection.Save(notification);
                    Console.WriteLine(" Notice saved");
                }
                else
                {
                    Console.WriteLine("Has Notice");
                }

                Console.WriteLine(Environment.NewLine);
            }
            


            Console.WriteLine("Press to Exit");
            Console.ReadKey();
        }
    }
}
