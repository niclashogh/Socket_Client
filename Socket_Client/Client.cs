using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Client
{
    public class Client
    {
        #region Variables
        private Socket clientSocket;

        private byte[] bufferData { get; set; } = new byte[1024];
        private byte[] dataSubmit { get; set; }
        private string bufferDecoded { get; set; }
        private int Port { get; set; }
        #endregion

        public Client(int port = 1111)
        {
            Port = port;
        }

        public void InitializeConnection()
        {
            IPHostEntry IPEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPAddr = IPEntry.AddressList[0];
            IPEndPoint EndPoint = new(IPAddr, Port);

            clientSocket = new(IPAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(EndPoint);

                if (clientSocket.Connected)
                {
                    Console.WriteLine($"Connected to EndPoint {EndPoint}");
                    SendToHost();
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                CloseConnection();
            }
        }

        private void SendToHost()
        {
            try
            {
                do
                {
                    Console.WriteLine("Write To Server:");
                    string message = Console.ReadLine();

                    dataSubmit = Encoding.ASCII.GetBytes(message);
                    clientSocket.Send(dataSubmit);

                    ReadFromHost();
                }
                while (clientSocket.Connected);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ReadFromHost()
        {
            try
            {
                bufferDecoded = ""; // Aparently need in client but not on the host side ??
                int packedLength = clientSocket.Receive(bufferData);
                bufferDecoded += Encoding.ASCII.GetString(bufferData, 0, packedLength);

                Console.WriteLine($"Message from Server : {bufferDecoded}"); //CW Feedback

                if (bufferDecoded.Contains("<FIN>"))
                {
                    CloseConnection();
                }

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void CloseConnection()
        {
            try
            {
                if (clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    Console.WriteLine("Connection is now closed");
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
