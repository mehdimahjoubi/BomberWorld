using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace BomberServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(BomberServiceImp.BomberService)))
            {
                host.Open();

                Console.WriteLine("Server is running");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();

                host.Close();
            }
        }
    }
}
