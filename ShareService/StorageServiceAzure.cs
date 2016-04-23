using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ShareService
{
    public class StorageServiceAzure:StorageServices
    {
        public ShareStorageServiceClient service { get; set; }
        public EndpointAddress Endpoint;
        


        public StorageServiceAzure(EndpointAddress address):base()
        {
            service = new ShareStorageServiceClient(binding,address);
        }

        

    }
}