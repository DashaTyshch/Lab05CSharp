using Lab05_Tyshchenko.Models;
using Lab05_Tyshchenko.ViewModels;
using System.Windows;

namespace Lab05_Tyshchenko.Windows
{
    /// <summary>
    /// Логика взаимодействия для ThreadsWindow.xaml
    /// </summary>
    public partial class ThreadsWindow : Window
    {
        public ThreadsWindow(MyProcess myProcess)
        {
            InitializeComponent();

            DataContext = new ThreadsViewModel(myProcess);
        }
    }
}
