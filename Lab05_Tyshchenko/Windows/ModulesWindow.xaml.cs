using Lab05_Tyshchenko.Models;
using Lab05_Tyshchenko.ViewModels;
using System.Windows;

namespace Lab05_Tyshchenko.Windows
{
    /// <summary>
    /// Логика взаимодействия для ModulesWindow.xaml
    /// </summary>
    public partial class ModulesWindow : Window
    {
        public ModulesWindow(MyProcess myProcess)
        {
            InitializeComponent();

            DataContext = new ModulesViewModel(myProcess);
        }
    }
}
