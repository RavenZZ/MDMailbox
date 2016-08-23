using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailbox.Entity.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mailbox.Entity
{
    [BsonIgnoreExtraElements]
    public class Notification
    {
        public ObjectId _id { get; set; }

        [BsonElement("mail")]
        public string Email { get; set; }

        [BsonElement("uid")]
        public string UniqueId { get; set; }

        [BsonElement("status")]
        public NotifyStatus Status { get; set; }


    }
}
