using Lab05_Tyshchenko.ViewModels;
using System.Windows.Controls;

namespace Lab05_Tyshchenko.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
