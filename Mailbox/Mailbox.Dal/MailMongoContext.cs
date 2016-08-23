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
    public class MailMongoContext : MongoDbDataContext
    {
        public MailMongoContext()
            : base(Configuration.Configuration.Instance.MongoDB.DbName, Configuration.Configuration.Instance.MongoDB.Uri
                )
        {
        }

        private static  readonly  Lazy<MailMongoContext>  lazy = new Lazy<MailMongoContext>(()=>new MailMongoContext());

        public static MailMongoContext Instance
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


        public MongoCollection<Notification> NotificationCollection
        {
            get { return this.GetMongoCollection<Notification>(); }
        }

        public IQueryable<Notification> Notifications
        {
            get { return this.NotificationCollection.AsQueryable(); }
        }


    }
}
