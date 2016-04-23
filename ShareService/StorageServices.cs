using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareLibray;
using System.ServiceModel;

namespace ShareService
{
    public abstract class StorageServices
    {
        public BasicHttpBinding binding;
        public EndpointAddress endpoint { get; set; }

        public StorageServices()
        {
            this.binding = this.CreateHttpBinding();
        }

        public abstract void UploadShares();

        public abstract ShareFile DownloadShare();

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