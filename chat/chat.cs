// CREATE CHAT.CS BY ZEKERİYA - UYSAL
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Do.chat
{
    public class StateObject
    {
        public Socket WorkSocket = null;
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder Sb = new StringBuilder();
    }


    class AsynchronousSocketListener
    {
        // Thread signal.
        public static ManualResetEvent AllDone = new ManualResetEvent(false);
        private static int _port = 9338;
        public AsynchronousSocketListener()
        {
        }

        public static void SetPort(int port)
        {
            _port = port;
        }

        public static void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, _port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(
                        AcceptCallback,
                        listener);

                    // Wait until a connection is made before continuing.
                    AllDone.WaitOne();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            AllDone.Set();
            try
            {
                var listener = (Socket)ar.AsyncState;
                var handler = listener.EndAccept(ar);
                new User(handler);
              

            }
            catch (Exception)
            {
            }
        }
    }
}
