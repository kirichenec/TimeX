﻿using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace TimeXv2.View
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void CloseDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
