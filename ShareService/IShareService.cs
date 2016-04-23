using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ShareLibrary;

namespace ShareService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IShareService
    {
        [OperationContract]
        User login(string ip,string username, string password);

        [OperationContract]
        User register(string ip,string fName, string lName, string username, string password);

        [OperationContract]
        byte[] uploadFile(ShareFile file, User user, string ip);

        [OperationContract]
        ShareFile downloadFile(User user, ShareFile file);

        [OperationContract]
        List<ShareFile> getUsersFile(User user, string ip);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
}
