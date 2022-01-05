using System;
using System.Linq;
using System.Windows;
using Client.Interfaces;
using Client.Methods;
using Client.EmailServiceReference;
using Microsoft.Win32;

namespace Client
{
    public partial class MainWindow : IClear
    {
        private string[] _attachedFilePath;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            TxtTo.Clear();
            TxtTitle.Clear();
            TxtBody.Clear();
        }

        private void btn_SendMessage(object sender, RoutedEventArgs e)
        {
            if (Check.IsFieldWithData(TxtFrom.Text, TxtPassword.Password))
            {
                try
                {
                    var client = new EmailServiceClient();
                    client.SendMessage(TxtFrom.Text, TxtPassword.Password, TxtTo.Text, TxtTitle.Text, TxtBody.Text, _attachedFilePath);
                    MessageBox.Show(@"Your message has been sent!", @"Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Something went wrong", @"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine(ex);
                }
                finally
                {
                    Clear();
                }
            }
            else
            {
                MessageBox.Show(@"Please fill all fields", @"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_ViewInbox(object sender, RoutedEventArgs e)
        {
            MyListView.Items.Clear();
            var client = new EmailServiceClient();
            var item = client.ViewMail(TxtFrom.Text, TxtPassword.Password);
            var list = item.ToList();

            foreach (var it in list)
            {
                MyListView.Items.Add(it);
            }
        }

        private void btn_MessageDelete(object sender, RoutedEventArgs e)
        {
            var client = new EmailServiceClient();
            var index = MyListView.Items.IndexOf(MyListView.SelectedItem);

            client.DeleteMail(TxtFrom.Text, TxtPassword.Password, index);
        }

        private void MyListView_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var index = MyListView.Items.IndexOf(MyListView.SelectedItem);
            var client = new EmailServiceClient();
            var content = client.ViewMessageBody(TxtFrom.Text, TxtPassword.Password, index);
            
            MessageBox.Show(content, @"Message Body", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btn_AttachFiles(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files (*.*)|*.*"
            };
            
            dialog.ShowDialog();

            var path = dialog.FileNames;
            _attachedFilePath = path;
        }
    }
}
