using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using GeneticWCF;

namespace GeneticServiceHost
{
    class ServiceHostMain
    {
        static void Main(string[] args)
        {
            //Console.Write("Podaj Ip pod jakim bedzie server:");
           // string ipOfHost = Console.ReadLine();



            string address = "http://localhost:8888/GeneticService/";
            //address = "http://" + ipOfHost + ":8888/GeneticService";
            Uri baseAddres = new Uri(address);

            ServiceHost selfHost = new ServiceHost(typeof(GeneticService), baseAddres);

            try
            {
                WSHttpBinding bindings = new WSHttpBinding(SecurityMode.None);
                bindings.MaxBufferPoolSize = 2147483647;
                bindings.MaxReceivedMessageSize = 2147483647;
               

                selfHost.AddServiceEndpoint(typeof(IGenetic), bindings , "GeneticService");

                

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);



                //S T A R T

                selfHost.Open();
                Console.WriteLine("Serwis dziala.....");
                Console.WriteLine("<ENTER> by zakonczyc");
                Console.WriteLine("HOST:" + baseAddres.Host);
                Console.WriteLine("PORT:" + baseAddres.Port);
                Console.WriteLine("URI:" + baseAddres.OriginalString);
                Console.WriteLine();
                Console.ReadLine();

                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("Wyjatek:\n" + ce.Message);
                selfHost.Abort();
            }

        }
    }
}
