using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Diagnostics;


namespace PortScanner
{
    class Program
    {

   
        static void Main(string[] args)
        {
            int input = 0;
          while (true) {
            


            Console.WriteLine("Welcome to the temporary menu");
            Console.WriteLine("Select an option");
            Console.WriteLine("1.: Port scan localhost");
            Console.WriteLine("2: Twitter Search for #tag ");
            Console.WriteLine("3. Find external IP");
            Console.WriteLine("4: Hide the console window");
            
           // input = int.Parse(Console.ReadLine());
            input = 0;

            switch (input) {
                case 1:
                    PortScannerTest.PortScanningClass portScanner = new PortScannerTest.PortScanningClass();
                    portScanner.portScan(args);
                //portScan(args);
          
            break;

                case 2:

            Console.WriteLine("please input a search string\n");
            string searchString;
            searchString = Console.ReadLine();
                    StringBuilder sb = new StringBuilder();
                        byte[] buf = new byte[8192];
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://search.twitter.com/search.json?q=%23" + searchString);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream readStream = response.GetResponseStream();
                        StreamReader streamReader = new StreamReader(readStream, Encoding.UTF8);
                        string responseFromServer = streamReader.ReadToEnd();

                        Console.WriteLine(responseFromServer);
                        string[] resonponses = Regex.Split(responseFromServer, "\"text\":");
                        Console.WriteLine();
                        foreach (string str in resonponses)
                        {
                            Console.WriteLine(str);


                        }

                        if (resonponses.Length != 1)
                        {

                            Console.WriteLine("Tweet found, triggering the port scan");

                        }
                        else
                        {
                            Console.WriteLine("no tweet present, falling back");

                        }
                        streamReader.Close();
                        response.Close();
                        Console.ReadLine();
                    break;
                case 3:
                        HTTPGet req = new HTTPGet();
                        req.Request("http://checkip.dyndns.org");
                        string[] a = req.ResponseBody.Split(':');
                        string a2 = a[1].Substring(1);
                        string[] a3=a2.Split('<');
                        string a4 = a3[0];
                        Console.WriteLine(a4);
                        Console.ReadLine();
                    break;


                case 4:

                    break;
                default:

                    Console.WriteLine(" you choose a non option"); 
            break;
        }
        }
    }
    }
}