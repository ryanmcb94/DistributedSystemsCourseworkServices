using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ShareLibrary;
using sss.crypto.data;

namespace ShareStorageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IShareStorageService
    {
        [OperationContract]
        List<byte[]> getShares(ShareFile file,int UserID);

        [OperationContract]
        void uploadShares(ShareFile file);

    }
}
