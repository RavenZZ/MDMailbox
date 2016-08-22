using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Mailbox.Service
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var a = Configuration.Configuration.Instance.MongoDB.Uri;
            Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
