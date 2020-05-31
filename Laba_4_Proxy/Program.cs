using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Laba_4_Ksis
{
    class Program
    {
        const string ProxyIP = "127.0.0.1";
        const int DefaultPort = 80; 
        const int ProxyPort = 666;
        const int BufferLength = 10000;
        static void Main(string[] args)
        {
            TcpListener ProxyListener = new TcpListener(IPAddress.Parse(ProxyIP), ProxyPort);
            ProxyListener.Start();
            while(true)
            {
                if (ProxyListener.Pending())
                {
                    TcpClient ProxyClient = ProxyListener.AcceptTcpClient();
                    Task.Factory.StartNew(() => ReceiveHTTP(ProxyClient));
                }
            }

        }
        public static byte[] AbsToRel(byte[] data) // из абсолютного пути в относительный
        {
            string buffer = Encoding.ASCII.GetString(data);
            Regex regex = new Regex(@"http:\/\/[a-z0-9а-яё\:\.]*");
            MatchCollection matches = regex.Matches(buffer);
            if (matches.Count != 0)
            {
                string host = matches[0].Value;
                buffer = buffer.Replace(host, "");
                data = Encoding.ASCII.GetBytes(buffer);
            }
            return data;
        }

        public static void ReceiveHTTP(TcpClient ProxyClient)
        {
            NetworkStream MyCatchStream = ProxyClient.GetStream();
            if (MyCatchStream.CanRead)
            {
                byte[] bytes = new byte[BufferLength];
                int LengthReaded = MyCatchStream.Read(bytes, 0, BufferLength);

                //HTTP bytes -> Text
                string returndata = Encoding.UTF8.GetString(bytes);

                HTTPWorking(bytes, MyCatchStream, ProxyClient, LengthReaded);
            }
            else
            {
                Console.WriteLine("You cannot read data from this stream.");
                ProxyClient.Close();
                MyCatchStream.Close();
                return;
            }
            MyCatchStream.Close();
        }

        public static void HTTPWorking(byte[] bytes, NetworkStream MyCatchStream, TcpClient ProxyClient,int LengthReaded)
        {
            try
            {
                string[] ParsedStrings = Encoding.ASCII.GetString(bytes).Trim().Split(new char[] { '\r', '\n' });
                string NamePort = "";
                if (ParsedStrings.Length > 1)
                {
                    NamePort = ParsedStrings.FirstOrDefault(x => x.Contains("Host"));
                    NamePort = NamePort.Substring(NamePort.IndexOf(":") + 2);
                    string[] NameAndPort = NamePort.Trim().Split(new char[] { ':' }); // получаем имя домена и номер порта
                    NamePort = ParsedStrings[2];
                    Console.WriteLine("Browser Request: ");
                    Console.WriteLine(NamePort);
                    Console.Write("Domain name:  ");
                    Console.WriteLine(NameAndPort[0]);
                    if (NameAndPort.Length > 1)
                    {
                        Console.Write("Port: ");
                        Console.WriteLine(NameAndPort[1]);
                    }
                    Console.WriteLine("______________________________________________");
                    TcpClient ToServer;
                    //Если указан порт, то true, если нет, то false и по стандартному порту "80"
                    if (NameAndPort.Length == 2)
                    {
                        ToServer = new TcpClient(NameAndPort[0], int.Parse(NameAndPort[1]));
                    }
                    else
                    {
                        ToServer = new TcpClient(NameAndPort[0], DefaultPort);
                    }

                    NetworkStream ToServerStream = ToServer.GetStream();

                    //Отправление исходного запроса.
                    if (MyCatchStream.CanWrite)
                        ToServerStream.Write(AbsToRel(bytes), 0, LengthReaded);

                    //Прием ответа от сервера 
                    byte[] ServerReply = new byte[BufferLength];
                    int ServerLengthReaded = 0;
                    if (ToServerStream.CanRead)
                        ServerLengthReaded = ToServerStream.Read(ServerReply, 0, BufferLength);

                    string[] ParsedStringsServerReply = Encoding.ASCII.GetString(ServerReply).Trim().Split(new char[] { '\r', '\n' });
                    Console.WriteLine("Server Reply: ");
                    Console.Write(NamePort);
                    Console.Write(" ");
                    Console.WriteLine(ParsedStringsServerReply[0]);
                    Console.WriteLine("______________________________________________");

                    //Отправление ответа сервера браузеру
                    if (MyCatchStream.CanWrite)
                        MyCatchStream.Write(ServerReply, 0, ServerLengthReaded);
                    ToServerStream.CopyTo(MyCatchStream);
                    //https connect 
                }
            }
            catch
            {
                return; 
            }
            finally
            {
                ProxyClient.Dispose();
            }
        }

    }
}
