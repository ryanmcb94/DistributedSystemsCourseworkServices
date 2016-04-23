using System.IO;
using ShareLibrary;
using sss;
using sss.config;
using sss.crypto.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShareStorageService
{
    public class Program
    {
        public static void Main(String[] args)
        {
            User u = new User(1, "RyanMcB", "Pa$$w0rd", "Ryan", "Mcb");
            ShareFile f = new ShareFile(1);
            Console.WriteLine("File: " + Storage.getService().downloadShares(f, 1));
            Console.WriteLine("Done");
            Console.ReadLine();
            /***
            byte[] b = File.ReadAllBytes("C:\\Users\\Ryan McBroom\\Desktop\\File.txt");
            ShareFile f = new ShareFile(b, "File.txt", 50, 20, 20);
            f.shares = CreateShares(f);
                Console.WriteLine("Done");
            Console.ReadLine();
            ***/
        }

        public static List<Share> CreateShares(ShareFile file)
        {
            sss.Facade f = new Facade(file.n, file.t, RandomSources.SHA1, Encryptors.AESGCM, Algorithms.CSS);
            return f.split(file.file).ToList<Share>();
        }
    }
}