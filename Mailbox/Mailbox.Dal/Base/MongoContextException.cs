using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Mailbox.Dal
{
    public class MongoContextException : MongoException
    {
        public MongoContextException(string message) : base(message)
        {
        }

        public MongoContextException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
    }
}
