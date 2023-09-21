using Client.Model;
using Client.View;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;

namespace Client.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private bool _canSendRequestCommandExecute;

        private ClientModel clientModel;

        public RelayCommand ChooseFileCommand { get; }

        public RelayCommand SendRequestCommand { get; }

        public string SelectedFilePath { get; set; }

        public bool CanSendRequestCommandExecute
        {
            get => _canSendRequestCommandExecute;
            set
            {
                _canSendRequestCommandExecute = value;
                OnPropertyChanged(nameof(CanSendRequestCommandExecute));
            }
        }

        private bool CanSendRequest(object parameter)
        {
            return !string.IsNullOrEmpty(SelectedFilePath) && SelectedFilePath != "Please select a file...";
        }

        //Main method for File Manager
        private void ChooseFile(object parameter)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                SelectedFilePath = openFile.FileName;
                CanSendRequestCommandExecute = true;
                SendRequestCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedFilePath)); // Notify the UI that the property has changed
            }
        }

        private void SendRequest(object parameter)
        {
            try
            {
                clientModel.SendRequest(SelectedFilePath);
                SendRequestCommand.RaiseCanExecuteChanged();
                //MessageBox.Show("Request sent successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //Used this for developing, idea was to get the messages from Server via some self defined protocol
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ClientViewModel()
        {
            clientModel = new ClientModel();
            ChooseFileCommand = new RelayCommand(ChooseFile);
            SelectedFilePath = "Please select a file..."; //Set the initial value for nicer display
            SendRequestCommand = new RelayCommand(SendRequest, CanSendRequest); //Handle the IsEnabled property with the overload
            clientModel.ServerErrorOccurred += HandleServerError;
            CanSendRequestCommandExecute = false;
        }

        public string ServerResponse
        {
            get { return clientModel.ServerResponse; }
            set
            {
                if (clientModel.ServerResponse != value)
                {
                    clientModel.ServerResponse = value;
                    OnPropertyChanged(nameof(ServerResponse));
                }
            }
        }

        private void HandleServerError(object sender, string errorMessage)
        {
            // Display the server error dialog here
            var viewModel = new ServerErrorDialogViewModel(errorMessage);
            var dialogView = new ServerErrorDialogView(viewModel);
            dialogView.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
