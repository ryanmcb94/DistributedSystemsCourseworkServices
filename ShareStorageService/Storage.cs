using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using ShareLibrary;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using sss;
using sss.crypto.data;
using Newtonsoft.Json;


namespace ShareStorageService
{
    public class Storage
    {
        //Define Variables
        private readonly string AccessKey = "DefaultEndpointsProtocol=https;AccountName=shareservicestorage;AccountKey=lfc30lbvCQpMZFBWbPQuGXv9QTIz8aTRyaw6DkBM7pi9yNggQ7924rGtQYhY1ZikUTpMdUS7KGMEB7INWjZafA==;BlobEndpoint=https://shareservicestorage.blob.core.windows.net/;TableEndpoint=https://shareservicestorage.table.core.windows.net/;QueueEndpoint=https://shareservicestorage.queue.core.windows.net/;FileEndpoint=https://shareservicestorage.file.core.windows.net/";
        private CloudBlobClient storageClient;
        private CloudBlobContainer container;
        private static Storage storage;

        /// <summary>
        /// 
        /// </summary>
        private Storage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.AccessKey);
            this.storageClient = storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FileID"></param>
        /// <param name="shares"></param>
        public void uploadShare(int UserID, int FileID, byte[] shares)
        {
            //Azure
            CloudBlobContainer con = this.getUsersContainer(UserID);
            CloudBlockBlob blob = con.GetBlockBlobReference(FileID.ToString());
            blob.UploadFromByteArray(shares, 0, 1, null, null, null);
            //Local
            string content = JsonConvert.SerializeObject(shares);
            string saveLoc = String.Format("{0}\\Storage\\{1}\\{2}.share", Environment.CurrentDirectory, UserID, FileID);
            System.IO.Directory.CreateDirectory(string.Format("{0}\\Storage\\{1}\\", Environment.CurrentDirectory, UserID));
            if (!File.Exists(saveLoc))
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(saveLoc, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    writer.WriteLine(content);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<byte[]> downloadShares(ShareFile file,int UserID)
        {
            //Local
            string content="";
            string saveLoc = String.Format("{0}\\Storage\\{1}\\{2}.share", Environment.CurrentDirectory, UserID, file.ID);
            using (StreamReader reader = new StreamReader(@saveLoc))
            {
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if(line !="")
                    {
                        content = content+reader.ReadLine();
                    }
                }
            }
            
            List<byte[]> bShares = (List<byte[]>)JsonConvert.DeserializeObject(content);
            if(bShares == null)
            {
                //Azure
                List<byte[]> shares = new List<byte[]>();
                CloudBlobContainer container = this.getUsersContainer(UserID);
                CloudBlockBlob blob = container.GetBlockBlobReference(file.ID.ToString());
                long arraySize = blob.Properties.Length;
                byte[] data = new byte[50];
                blob.DownloadToByteArray(data, 0);
                shares.Add(data);
                return shares;
            }
            foreach(byte[] b in bShares)
            {
                //Local
                bShares.Add(b);
            }
            return bShares;
        } 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        protected CloudBlobContainer getUsersContainer(int ID)
        {
            CloudBlobContainer container= this.storageClient.GetContainerReference("secretshareservice" + ID.ToString().ToLower());
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            return container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        protected byte[] ShareToByte(List<Share>shares)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            List<byte[]> shareSerialized = new List<byte[]>();
            foreach(Share s in shares)
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
        protected List<Share> ByteToShare(byte[] byteArray)
        {
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            mStream.Write(byteArray, 0, byteArray.Length);
            mStream.Seek(0, SeekOrigin.Begin);
            return (List<Share>)formatter.Deserialize(mStream);
        }

        public static Storage getService()
        {
            if(storage == null)
            {
                storage = new Storage();
            }
            return storage;
        }



    }
}