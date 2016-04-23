using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareLibrary;
using System.ServiceModel;
using System.Net;

namespace Client
{
    public class Controller
    {
        private static Controller control;
        public User user { get; set; }
        public ShareServiceClient service;
        public BasicHttpBinding binding;
        public EndpointAddress endpoint { get; set; }


        private Controller()
        {
            this.binding = this.CreateHttpBinding();
            this.endpoint = new EndpointAddress("http://40.115.38.250/Service/ShareService.svc");
            service = new ShareServiceClient(binding, endpoint);
        }
        public static Controller getController()
        {
            if(control ==null)
            {
                control = new Controller();
            }
            return control;
        }


        protected BasicHttpBinding CreateHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "BasicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 5000);
            binding.SendTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        public string getIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Address Error";
        }
    
    }
}
