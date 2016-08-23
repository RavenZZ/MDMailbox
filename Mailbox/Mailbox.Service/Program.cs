using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailbox.Dal;
using Mailbox.Entity;


namespace Mailbox.Service
{
    public class Program
    {

        public static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                var account = new AccountBinding()
                {
                    AccountId = Guid.NewGuid().ToString(),
                    Mail = $"mail{i}@qq.com",
                    Type = MailBindingType.Imap,
                    EnableSSL = true,
                    Server = "imap.qq.com",
                    Port = 465,
                    UpdateTime = DateTime.Now,
                    Token =  "aaa"
                };
                BindingMongoContext.Instance.Save(account);
                Console.WriteLine($"insert {i} mail={account.Mail}");
            }

         


      //      BindingMongoContext.Instance.Save();


            //var a = Configuration.Configuration.Instance.MongoDB.Uri;
            //Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
