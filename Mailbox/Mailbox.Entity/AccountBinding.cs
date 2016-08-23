using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mailbox.Entity
{
    [BsonIgnoreExtraElements]
    public class AccountBinding
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }

        private string accountId;
        [BsonElement("aid")]
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        private string token;

        [BsonElement("token")]
        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        private string mail;
        [BsonElement("mail")]
        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }


        private MailBindingType type;
        [BsonElement("type")]
        public MailBindingType Type
        {
            get { return type; }
            set { type = value; }
        }

        [BsonElement("ssl")]
        public bool EnableSSL { get; set; }

        private string server;
        [BsonElement("server")]
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        private int port;
        [BsonElement("port")]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private DateTime updateTIme;
        [BsonElement("time")]
        public DateTime UpdateTime
        {
            get { return updateTIme; }
            set { updateTIme = value; }
        }


    }
}
