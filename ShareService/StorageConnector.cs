using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareLibrary;
using System.ServiceModel;

namespace ShareService
{
    public class StorageConnector
    {
        public BasicHttpBinding binding;
        public EndpointAddress endpoint { get; set; }
        public ShareStorageServiceClient service { get; set; }
        

        public StorageConnector(EndpointAddress address) : base()
        {
            this.binding = this.CreateHttpBinding();
            service = new ShareStorageServiceClient(binding, address);
        }
        public void UploadShares(ShareFile file, User user,byte[] shares)
        {
            file.UserID = user.ID;
            file.file = shares;
           service.uploadShares(file);
        }
        public List<byte[]> DownloadShare(ShareFile file,User user)
        {
             return service.getShares(file, user.ID).ToList();
        }
        protected BasicHttpBinding CreateHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "BasicHttpBinding",
                MaxBufferSize = 214783647,
                MaxReceivedMessageSize = 214783647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 5000);
            binding.SendTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }
    }
}