using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client.Model
{
    public class ClientModel
    {
        private string serverResponse;

        public string ServerResponse
        {
            get { return serverResponse; }
            set
            {
                if (serverResponse != value)
                {
                    serverResponse = value;
                }
            }
        }

        public event EventHandler<string> ServerErrorOccurred;

        //ModelLayer of 
        public void SendRequest(string selectedPath)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 1234); // Connect to the server, ToDo bind it with App.config
                NetworkStream stream = client.GetStream();

                if (!File.Exists(selectedPath))
                {
                    throw new FileNotFoundException("The selected file does not exist.");
                }

                byte[] data = Encoding.ASCII.GetBytes(selectedPath);
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            catch (SocketException)
            {
                HandleServerError("Server is not running!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending request: {ex.Message}");
            }
        }

        // Raise the ServerErrorOccurred event with the error message
        private void HandleServerError(string errorMessage)
        {
            ServerErrorOccurred?.Invoke(this, errorMessage);
        }
    }
}
