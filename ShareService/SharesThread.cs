using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareLibrary;
using System.Threading;
using sss.crypto.data;

namespace ShareService
{
    public class SharesThread
    {
        StorageConnector connector;
        ShareFile file;
        public bool found = false;
        public List<byte[]> shares { get; set; }
        User user;
        public SharesThread(StorageConnector connector,ShareFile file,User user)
        {
            this.connector = connector;
            this.file = file;
            this.user = user;
        }
        public void ThreadRun()
        {
           this.shares = connector.service.getShares(file, user.ID).ToList();
           if(this.shares.Count >0)
           {
               found = true;
           }
        }
        
    }
}