using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Client
{
    public class Client_Old
    {
        #region Variables
        private Socket clientSocket;

        private byte[] bufferArray;
        private byte[] dataSubmit;
        private string bufferDecoded;
        #endregion

        public void InitEndPoint(int port = 1111, int bufferSize = 1024) // = Default
        {
            IPHostEntry IPEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPAddr = IPEntry.AddressList[0];
            IPEndPoint EndPoint = new(IPAddr, port);

            clientSocket = new(IPAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(EndPoint);
                bufferArray = new byte[bufferSize];

                Console.WriteLine($"Connected to EndPoint {EndPoint}");

                //TransferToServer();
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

        public void TransferToServer(string message = "Client Req.<EOF>") // = Defualt
        {
            try
            {
                dataSubmit = Encoding.ASCII.GetBytes(message);
                clientSocket.Send(dataSubmit);

                Console.WriteLine($"Message to Server : {message}"); //CW Feedback
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
                RetrieveServerTransfer();
                //CloseConnection();
            }
        }

        public void RetrieveServerTransfer()
        {
            try
            {
                bool isEOF = false;
                do
                {
                    int packedLength = clientSocket.Receive(bufferArray);
                    bufferDecoded += Encoding.ASCII.GetString(bufferArray, 0, packedLength);

                    Console.WriteLine($"Message from Server : {bufferDecoded}"); //CW Feedback

                    if (bufferDecoded.IndexOf("<EOF>") > -1)
                    {
                        isEOF = true;
                    }
                }
                while (!isEOF);
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

        public void CloseConnection()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
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

    public class TestClient
    {
        public void Run()
        {
            try
            {
                IPHostEntry IPEntry = Dns.GetHostEntry(Dns.GetHostName()); // Retrieve a list of HostEntries
                IPAddress IPAddr = IPEntry.AddressList[0]; // Select the first in the list (usually the active)
                IPEndPoint EndPoint = new(IPAddr, 1111); // Set new endpoint obj (IPAddr, #Port)

                Socket sender = new(IPAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(EndPoint);
                    Console.WriteLine($"Socket Connected to {sender.RemoteEndPoint}"); //CW Feedback

                    // Buffer Array for sending data
                    byte[] dataTransfer = Encoding.ASCII.GetBytes("Test Client<EOF>"); // EOF = End of line
                    sender.Send(dataTransfer);

                    // Buffer Array for reciving data
                    byte[] bufferArray = new byte[1024];
                    int packedLength = sender.Receive(bufferArray);
                    string bufferDecoded = Encoding.ASCII.GetString(bufferArray, 0, packedLength);

                    Console.WriteLine($"Data From Server : {bufferDecoded}"); // CW Feedback

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
