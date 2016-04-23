using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using sss.crypto.data;
using System.Runtime.Serialization;

namespace ShareLibrary
{
    [DataContract]
    public class ShareFile
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public byte[] file { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int n { get; set; }

        [DataMember]
        public int ToReturn { get; set; }
        [DataMember]
        public string fileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int t { get; set; }
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public List<Share> shares { get; set; }
        
        [DataMember]
        public List<byte[]> sShares { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="n"></param>
        /// <param name="t"></param>
        public ShareFile(string filename,int n,int t,int ret)
        {
            this.file = file;
            this.fileName = filename;
            this.n = n;
            this.t = t;
            this.ToReturn = ret;
            this.shares = new List<Share>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="filename"></param>
        /// <param name="UserID"></param>
        /// <param name="n"></param>
        /// <param name="t"></param>
        public ShareFile(int ID,string filename,int UserID,int n,int t)
        {
            this.ID = ID;
            this.fileName = filename;
            this.UserID = ID;
            this.n = n;
            this.t = t;
            this.shares = new List<Share>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="filename"></param>
        /// <param name="UserID"></param>
        /// <param name="n"></param>
        /// <param name="t"></param>
        /// <param name="r"></param>
        public ShareFile(int ID, string filename,int UserID, int n, int t, int r)
        {
            this.ID = ID;
            this.fileName = filename;
            this.UserID = UserID;
            this.n = n;
            this.t = t;
            this.ToReturn = r;
            this.shares = new List<Share>();

        }
        public ShareFile(int ID)
        {
            this.ID = ID;
            this.shares = new List<Share>();
        }

        public ShareFile(int ID, string fileName)
        {
            this.ID = ID;
            this.fileName = fileName;
            this.shares = new List<Share>();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.fileName;
        }




        
    }
}
