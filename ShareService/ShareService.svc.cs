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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ShareService : IShareService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User login(string ip,string username, string password)
        {
            return Database.getDB().Login(ip, username, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User register(string ip, string fName,string lName,string username,string password)
        {
            return Database.getDB().Register(ip,fName, lName, username, password);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="user"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public byte[] uploadFile(ShareFile file,User user, string ip)
        {
            return System.getSystem().uploadFile(file, user, ip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public ShareFile downloadFile(User user, ShareFile file)
        {
            return System.getSystem().downloadFile(user,file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public List<ShareFile> getUsersFile(User user,string ip)
        {
            return Database.getDB().getUserFiles(user,ip);
        }



    }
}
