using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareLibrary;
using sss.crypto.data;
using System.Threading;
using sss;
using sss.config;
using System.ServiceModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ShareService
{
    public class System
    {
        private List<StorageConnector> connectors;
        private static System sys;
        Share s;
        
        private System()
        {
            this.addConnectors();
        }
        public static System getSystem()
        {
            if (sys == null)
            {
                sys = new System();
            }
            return sys;
        }
        public void addConnectors()
        {
            this.connectors = new List<StorageConnector>();
            StorageConnector sc1 = new StorageConnector(new EndpointAddress("http://40.115.38.250/StorageService/ShareStorageService.svc"));
            this.connectors.Add(sc1);
        }


        public byte[] uploadFile(ShareFile file, User user, string ip)
        {
            Database.getDB().AddFileToDB(file, user, ip);
            file.shares = this.createShares(file);
            return this.sendShares(file, user);
        }

        public List<Share> createShares(ShareFile file)
        {
            Facade f = new Facade(file.n, file.t, RandomSources.SHA1, Encryptors.AESGCM, Algorithms.CSS);
            return f.split(file.file).ToList<Share>();
        }

        protected byte[] sendShares(ShareFile file, User user)
        {
            int pos = 0;
            //ToReturn
            List<Share> toReturn = new List<Share>();
            if (file.ToReturn >0)
            {
                while (pos < file.ToReturn)
                {
                    toReturn.Add((Share)file.shares.ElementAt(pos));
                    pos++;
                }
            }
            foreach(StorageConnector connector in this.connectors)
            {
                List<Share> shares = new List<Share>();
                while(pos <= file.shares.Count)
                {
                    shares.Add((Share)file.shares.ElementAt(pos));
                }
                ShareFile f = new ShareFile(file.ID, file.fileName, file.UserID, file.n, file.t);
                byte[] bShares = this.shareToByte(shares);
                connector.UploadShares(f, user,bShares);

            }
            if(pos < file.shares.Count)
            {
                List<Share> shares = new List<Share>();
                while (pos <= file.shares.Count)
                {
                    shares.Add((Share)file.shares.ElementAt(pos));
                }
                ShareFile f = new ShareFile(file.ID, file.fileName, file.UserID, file.n, file.t);
                byte[] bShares = this.shareToByte(shares);
                Random rnd = new Random();
                int con = rnd.Next(connectors.Count);
                connectors[con].UploadShares(f, user,bShares);
            }
            //Return Shares
            return this.shareToByte(toReturn);
        }



        public ShareFile downloadFile(User user,ShareFile file)
        {
            ShareFile share = Database.getDB().FileDetails(user, file.ID);
            List<Share> shares = new List<Share>();
            int timeout = 0;
            List<SharesThread> threads = new List<SharesThread>();
            List<StorageConnector> activeConnectors = new List<StorageConnector>();
            foreach(StorageConnector connector in this.connectors)
            {
                SharesThread st = new SharesThread(connector,file,user);
                threads.Add(st);
                Thread t = new Thread(new ThreadStart(st.ThreadRun));
                t.Start();
                t.Join();
            }
            while(shares.Count < file.t)
            {
                foreach (SharesThread st in threads)
                {
                    if(st.found == true)
                    {
                        timeout = 0;
                        foreach (byte[] b in st.shares)
                        {
                            shares.AddRange(this.byteToShare(b));
                        }
                    }
                }
            }
            //Convert.
            Facade f = new Facade(file.n, file.t, RandomSources.SHA1, Encryptors.AESGCM, Algorithms.CSS);
            file.file = f.join(shares.ToArray());
            return file; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        protected byte[] shareToByte(List<Share> shares)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            List<byte[]> shareSerialized = new List<byte[]>();
            foreach (Share s in shares)
            {
                shareSerialized.Add(s.serialize());
            }
            formatter.Serialize(ms, shareSerialized);
            return ms.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        protected List<Share> byteToShare(byte[] byteArray)
        {
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            mStream.Write(byteArray, 0, byteArray.Length);
            mStream.Seek(0, SeekOrigin.Begin);
            List<byte[]> bShares =(List<byte[]>)formatter.Deserialize(mStream);
            List<Share> shares = new List<Share>();
            foreach(byte[] b in bShares)
            {
                shares.Add(SerializableShare.deserialize(b));
            }
            return shares;
        }



    }
}