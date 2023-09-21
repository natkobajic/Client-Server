using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class ServerErrorDialogViewModel
    {
        private string errorMessage;

        public ServerErrorDialogViewModel(string errorMessage)
        {
            this.errorMessage = errorMessage;
            RunServerCommand = new RelayCommand(ExecuteRunServerCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public ICommand RunServerCommand { get; }

        public ICommand CancelCommand { get; }

        private void ExecuteRunServerCommand(object parameter)
        {
            try
            {
                Process.Start("Server.exe");
                ((Window)parameter).Close();
                //if (parameter is Window window)
                //{
                //    Process.Start("Server.exe");
                //    window.Close();
                //}
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error starting the server: {ex.Message}";
            }
            
            //Close it here or in the try above, whatever
        }

        private void ExecuteCancelCommand(object parameter)
        {
            ((Window)parameter).Close();
        }
    }
}
