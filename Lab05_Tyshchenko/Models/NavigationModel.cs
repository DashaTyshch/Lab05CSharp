using Lab05_Tyshchenko.Views;
using Lab05_Tyshchenko.Windows;
using System;

namespace Lab05_Tyshchenko.Models
{
    public enum ModesEnum
    {
        Main,
    }

    class NavigationModel
    {
        private ContentWindow _contentWindow;
        private MainView _mainView;

        public NavigationModel(ContentWindow contentWindow)
        {
            _contentWindow = contentWindow;
            _mainView = new MainView();
        }

        public void Navigate(ModesEnum mode)
        {
            switch (mode)
            {
                case ModesEnum.Main:
                    _contentWindow.ContentControl.Content = _mainView;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
