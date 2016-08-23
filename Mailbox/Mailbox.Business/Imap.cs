using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailbox.Entity;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace Mailbox.Business
{
    public class Imap
    {
        public ImapClient Client { get; set; }

        public AccountBinding BindingInfo { get; set; }

        public Imap(Entity.AccountBinding binding)
        {
            BindingInfo = binding;
        }

        public ImapClient Init()
        {
            if (BindingInfo == null)
            {
                throw new MissingFieldException("BindingInfo is missing");
            }

            Client = new ImapClient();

            Client.Timeout = 10000;
            Client.Connect(host: BindingInfo.Server, port: BindingInfo.Port, useSsl: BindingInfo.EnableSSL);
            Client.AuthenticationMechanisms.Remove("XOAUTH2");
            Client.Authenticate(userName: BindingInfo.Mail, password: BindingInfo.Token);
            return Client;
        }

        public Dictionary<UniqueId, string> FetchNotSeen()
        {
            var inbox = Client.Inbox;

            inbox.Open(FolderAccess.ReadOnly);

            var newMails = new Dictionary<UniqueId, string>();

            foreach (var uid in inbox.Search(SearchQuery.NotSeen))
            {
                var message = inbox.GetMessage(uid);
                newMails.Add(uid, message.Subject);
            }


            return newMails;
        }



    }
}
