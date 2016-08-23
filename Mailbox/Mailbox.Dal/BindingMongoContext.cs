using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Mailbox.Configuration;
using  Mailbox.Entity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mailbox.Dal
{
    public class BindingMongoContext : MongoDbDataContext
    {
        public BindingMongoContext()
            : base(Configuration.Configuration.Instance.MongoDB.DbName, Configuration.Configuration.Instance.MongoDB.Uri
                )
        {
        }

        private static  readonly  Lazy<BindingMongoContext>  lazy = new Lazy<BindingMongoContext>(()=>new BindingMongoContext());

        public static BindingMongoContext Instance
        {
            get { return lazy.Value; }
        }


        public MongoCollection<AccountBinding> BindingCollection
        {
            get { return this.GetMongoCollection<AccountBinding>(); }
        }

        public IQueryable<AccountBinding> AccountBindings
        {
            get { return this.BindingCollection.AsQueryable(); }
        }




    }
}
