using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Mailbox.Entity
{
    public interface IMongoEntity
    {
        ObjectId _id { get; }
        string _accessId { get; set; }
    }
}
