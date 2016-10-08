using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //var c = new SIMServer.Config(new SDatabase.MySQL.ConnectionData("127.0.0.1", 3306, "sim", "sim", "1234"), 10000);
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(c));
            //Console.ReadLine();
            var s = new SIMServer.Main();
        }
    }
}
