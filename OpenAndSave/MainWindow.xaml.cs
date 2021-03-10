using Microsoft.Win32;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OpenAndSave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadCache();
        }
        void loadCache()
        {
            if (File.Exists(Environment.SpecialFolder.InternetCache.ToString()))
            {
                string s = File.ReadAllText(Environment.SpecialFolder.InternetCache.ToString());
                string[] subs = s.Split('-');
                for (int i = 0; i < subs.Length - 1; i++)
                {
                    ListBox_paths.Items.Add(subs[i]);
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string temp = "";
            foreach (var item in ListBox_paths.Items)
            {
                temp += ($"{item}-");
            }
            File.WriteAllText(Environment.SpecialFolder.InternetCache.ToString(), temp);
        }

        private void ListBox_paths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tb_box.Text = "";
                img_box.Source = null;
                if (ListBox_paths.SelectedItem != null)
                {
                    if (ListBox_paths.SelectedItem.ToString().EndsWith(".txt"))
                    {
                        tb_box.Text = File.ReadAllText(ListBox_paths.SelectedItem.ToString());
                        tb_box.Visibility = Visibility.Visible;
                        img_box.Visibility = Visibility.Collapsed;
                    }
                    else if (ListBox_paths.SelectedItem.ToString().EndsWith(".jpg") || ListBox_paths.SelectedItem.ToString().EndsWith(".jepg") || ListBox_paths.SelectedItem.ToString().EndsWith(".png"))
                    {
                        img_box.Source = new BitmapImage(new Uri(ListBox_paths.SelectedItem.ToString()));
                        img_box.Visibility = Visibility.Visible;
                        tb_box.Visibility = Visibility.Collapsed;
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btn_up_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_paths.Items.Count > 0)
            {
                if (ListBox_paths.SelectedIndex == 0 || ListBox_paths.SelectedIndex == -1)
                    ListBox_paths.SelectedIndex = ListBox_paths.Items.Count - 1;
                else
                    ListBox_paths.SelectedIndex = ListBox_paths.SelectedIndex - 1;
            }
        }

        private void btn_down_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_paths.Items.Count > 0)
            {
                if (ListBox_paths.SelectedIndex == ListBox_paths.Items.Count - 1 || ListBox_paths.SelectedIndex == -1)
                    ListBox_paths.SelectedIndex = 0;
                else
                    ListBox_paths.SelectedIndex = ListBox_paths.SelectedIndex + 1;
            }

        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Up)
            {
                btn_up_Click(null, null);
            }
            if (e.Key == System.Windows.Input.Key.Down)
            {
                btn_down_Click(null, null);
            }
        }

        //
        //  Menu Methods
        //

        //File
        private void btn_open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Alle .jpg Dateien | *.jpg; *.jpeg; *.png;" +
                                    "| Alle .txt Dateien | *.txt; " +
                                    "| Alle möglichen Dateiformate | *.jpg; *.jpeg; *.png; *.txt;";
            openFileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var item in openFileDialog.FileNames)
                {
                    ListBox_paths.Items.Add(item);
                }
            }
        }
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (tb_box.Text != "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = " Alle .txt Dateien | *.txt; ";
                saveFileDialog.Title = "MOIN!";
                saveFileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, tb_box.Text);
                }
            }
            else
            {
                MessageBox.Show("Please write some Text to save");
            }
        }

        private void MItem_File_Close_Click(object sender, RoutedEventArgs e)
        {
            var o = MessageBox.Show("Wollen Sie die Anwendung wirklich beenden?", "", MessageBoxButton.YesNo);
            if (o == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        //Edit
        //handled in XAML

        //Font

        private void MItem_Font_Bold_Checked(object sender, RoutedEventArgs e)
        {
            tb_box.FontWeight = FontWeights.Bold;
        }
        private void MItem_Font_Bold_Unchecked(object sender, RoutedEventArgs e)
        {
            tb_box.FontWeight = FontWeights.Normal;
        }

        private void MItem_Font_Italic_Checked(object sender, RoutedEventArgs e)
        {
            tb_box.FontStyle = FontStyles.Italic;
        }
        private void MItem_Font_Italic_Unchecked(object sender, RoutedEventArgs e)
        {
            tb_box.FontStyle = FontStyles.Normal;
        }
        private void MItem_Font_Incr_Click(object sender, RoutedEventArgs e)
        {
            tb_box.FontSize += 4;
        }

        private void MItem_Font_Decr_Click(object sender, RoutedEventArgs e)
        {
            tb_box.FontSize -= 4;
        }

        
    }
}
