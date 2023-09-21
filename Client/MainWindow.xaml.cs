using Client.ViewModel;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClientViewModel ClientViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ClientViewModel = new ClientViewModel();
            DataContext = this;
        }

        //Codebehind initial approach:
        //...
        //private void ClientViewControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.ClientViewModel clientViewModelObject = new ViewModel.ClientViewModel();

        //    clientViewModelObject.LoadClient();

        //    ClientViewControl.DataContext = clientViewModelObject;
        //}


        //private void ChooseFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFile = new OpenFileDialog();
        //    openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        //    if (openFile.ShowDialog() == true)
        //    {
        //        lblFilePath.Content = openFile.FileName;
        //        btnSendFile.IsEnabled = true;
        //    }
        //}

        //private void SendFile_Click(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        TcpClient client = new TcpClient("127.0.0.1", 12345); // Connect to the server
        //        NetworkStream stream = client.GetStream();

        //        try
        //        {
        //            string selectedPath = lblFilePath.Content.ToString();

        //            if (!File.Exists(selectedPath))
        //            {
        //                MessageBox.Show("The selected file does not exist.", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
        //                return; // Do not send the file path to the server
        //            }

        //            byte[] data = Encoding.ASCII.GetBytes(selectedPath);
        //            stream.Write(data, 0, data.Length);
        //            stream.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch (SocketException)
        //    {
        //        ServerErrorDialog errorDialog = new ServerErrorDialog();
        //        errorDialog.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error: {ex.Message}");
        //    }
        //    finally
        //    {

        //        // client.Close(); better to solve this on server side since we need preprocessing
        //    }
        //}

    }
}
