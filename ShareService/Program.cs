using ShareLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShareService
{
    class Program
    {
        static void Main(string[] args)
        {
            User u = Database.getDB().Login("0.0.0.0", "RyanMcb", "Pa$$w0rd");
            ShareFile f = new ShareFile(1);
            System.getSystem().downloadFile(u, f);
           // System.getSystem().uploadFile(f, u, "0.0.0.0");
            Console.WriteLine("Done");
            Console.ReadLine();
        }


    }

}
