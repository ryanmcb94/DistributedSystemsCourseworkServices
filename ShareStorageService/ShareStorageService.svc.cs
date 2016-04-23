using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ShareLibrary;
using sss.crypto.data;
using ShareLibrary;

namespace ShareStorageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ShareStorageService : IShareStorageService
    {
        public List<byte[]> getShares(ShareFile file, int UserID)
        {
            return Storage.getService().downloadShares(file, UserID);
        }

        public void uploadShares(ShareFile file)
        {
            Storage.getService().uploadShare(file.UserID, file.ID,file.file);
        }
    }
}
