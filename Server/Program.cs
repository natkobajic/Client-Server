using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private TcpListener listener;

        private Queue<string> requestQueue;

        private object queueLock = new object();

        public Server()
        {
            string portNumberString = ConfigurationManager.AppSettings["PortNumber"];

            // Handle the case where the port number is not a valid integer
            if (!int.TryParse(portNumberString, out int portNumber))
            {
                Console.WriteLine("Invalid port number specified in App.config.");
                Environment.Exit(1); // Close the Server with Error code
            }
            listener = new TcpListener(IPAddress.Loopback, portNumber);
            requestQueue = new Queue<string>();
        }

        //Starting the program and connecting via TcpClient
        public void Start()
        {
            try
            {
                listener.Start();
                Console.WriteLine("Server is running...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine($"Not enough Memory: {ex.Message}");
                Environment.Exit(1); // Close the Server with Error code
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
                Environment.Exit(1); // Close the Server with Error code
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in server startup: {ex.Message}");

            }
        }

        //Network layer - handles network communication with client, which includes reading incoming data and managing the client connection
        private void HandleClient(object obj)
        {
            try
            {
                TcpClient client = (TcpClient)obj;

                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Received: {message}");
                        HandleReceivedMessage(message);
                    }
                }
                client.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IOException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }

        //Processing the received message, validation, parsing, etc.
        public void HandleReceivedMessage(string message)
        {
            if (!File.Exists(message))
            {
                Console.WriteLine($"File not found: {message}");
            }
            else
            {
                // Proceed
                StartRequestProcessingThread(message);
            }
        }

        //Preprocessing of file pt. 1, enqueuing and starting a separate thread as requested
        public void StartRequestProcessingThread(string message)
        {
            lock (queueLock)
            {
                requestQueue.Enqueue(message);
            }

            Thread requestThread = new Thread(ProcessRequests);

            try
            {
                requestThread.Start();
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine($"Not enough Memory to run new Requests");
            }
        }

        //Preprocessing of file pt. 2, dequeuing 
        public void ProcessRequests()
        {
            string path;
            lock (queueLock)
            {
                while (requestQueue.Count > 0)
                {
                    path = requestQueue.Dequeue();//FIFO
                    ProcessFile(path);
                }
            }
        }

        //Finally, adding Timestamp
        private void ProcessFile(string path)
        {
            try
            {
                string content = File.ReadAllText(path);
                content += $"{Environment.NewLine}Timestamp: {DateTime.Now}";
                File.WriteAllText(path, content);
                Console.WriteLine($"Processed: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            Thread serverThread = new Thread(server.Start);
            serverThread.Start();

            Console.WriteLine("Press 'Q' to close the server...");
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key != ConsoleKey.Q)
                {
                    continue;
                }
                else
                {
                    Environment.Exit(0);//Not a very graceful, rather practical way, I admit...
                }
            }
        }
    }
}