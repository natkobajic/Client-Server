using Client.ViewModel;
using System.Windows;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for ServerErrorView.xaml
    /// </summary>
    public partial class ServerErrorDialogView : Window
    {
        public ServerErrorDialogView(ServerErrorDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
