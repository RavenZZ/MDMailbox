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
            var a = new Mailbox.Configuration.Configuration();
            a.getConfig();
        }
    }
}
