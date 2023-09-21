using Client.ViewModel;
using System.Windows.Controls;


namespace Client.View
{
    /// <summary>
    /// Interaction logic for ClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
        public ClientView()
        {
            InitializeComponent();
            DataContext = new ClientViewModel();
        }
    }
}
