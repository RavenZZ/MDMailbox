using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
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
                Token = "nnofhlbubmhbbcib",
                Type = MailBindingType.Imap
            };

            var singularity = Singularity.Instance;

            var job = new SimpleParameterizedJob<AccountBinding>((user, scheduledTime) =>
            {
                FetchUnseen(user);
                Console.WriteLine($"scheduled: {scheduledTime.ToString()}");
            });

            var schedule = new EveryXTimeSchedule(TimeSpan.FromMinutes(1));

            var scheduledJob = singularity.ScheduleParameterizedJob(schedule, job, raven, true);

            singularity.Start();




            Console.WriteLine("Press to Exit");
            Console.ReadKey();
        }




        private static void FetchUnseen(AccountBinding binding)
        {
            Imap imap = new Imap(binding);
            Console.WriteLine("Imap Init Start");
            imap.Init();
            Console.WriteLine("Imap FetchNotSeen Start");
            var unseen = imap.FetchNotSeen();
            Console.WriteLine("Imap FetchNotSeen End");

            foreach (KeyValuePair<UniqueId, string> pair in unseen)
            {
              
                var query =
                    from p in MailMongoContext.Instance.Notifications
                    where p.UniqueId == pair.Key.ToString() && p.Email == binding.Mail
                    select p;
               if (!query.Any())
                {
                    Console.WriteLine("New Mail");
                    Console.WriteLine("Mail");
                    Console.WriteLine($"uid  {pair.Key}");
                    Console.WriteLine($"Subject {pair.Value}");
                    SendPush(string.Empty, binding.AccountId, $"新邮件:{pair.Value}");
                    var notification = new Notification()
                    {
                        Email = binding.Mail,
                        Status = NotifyStatus.Success,
                        UniqueId = pair.Key.ToString()
                    };
                    MailMongoContext.Instance.NotificationCollection.Save(notification);
                    Console.WriteLine(" Notice saved");
                }
                else
                {
                    //Console.WriteLine("Has Notice");
                }
            }

            Console.WriteLine($"mail: {binding.Mail} fetching success");
        }



        private static void SendPush(string token, string accountId, string message)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("access_token", "a8b86f3087dd4f52a8c60b191a4ac3d9");
            dic.Add("account_id", accountId);
            dic.Add("message",message);
            HttpContent content = new FormUrlEncodedContent(dic);
            HttpClient client = new HttpClient();
            var uri =
                "https://api.mingdao.com/v1/message/Send_Inbox_System_Message";
            
            var task = client.PostAsync(uri, content);

            var result = task.GetAwaiter().GetResult();

        }








    }
}
