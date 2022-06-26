using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using peresvet.Models;
using peresvet.Pages;

namespace peresvet.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddAndEditPage.xaml
    /// </summary>
    public partial class AddAndEditPage : Page
    {
        private  Sotrudniki _currentSotrudniki = new Sotrudniki();
        private string _filePath = null;
        private string _photoName = null;
        private static string _currentDirectory = Directory.GetCurrentDirectory() + @"\Images\";
        public AddAndEditPage(Sotrudniki selectedSotrudniki)
        {
            InitializeComponent();
            if (selectedSotrudniki != null)
                _currentSotrudniki = selectedSotrudniki;

            DataContext = _currentSotrudniki;
            ComboStatus.ItemsSource = predprEntities.GetContext().Status.ToList();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif) |*.gif";
                if (op.ShowDialog() == true)
                {
                    FileInfo fileInfo = new FileInfo(op.FileName);
                    if (fileInfo.Length > (1024 * 1024 * 2))
                    {
                        throw new Exception("Размер файла должен быть меньше 2Мб");
                    }
                    ImagePhoto.Source = new BitmapImage(new Uri(op.FileName));
                    _photoName = op.SafeFileName;
                    _filePath = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _filePath = null;
            }
        }
        string ChangePhotoName()
        {
            string x = _currentDirectory + _photoName;
            string photoname = _photoName;
            int i = 0;
            if (File.Exists(x))
            {
                while (File.Exists(x))
                {
                    i++;
                    x = _currentDirectory + i.ToString() + photoname;
                }
                photoname = i.ToString() + photoname;
            }
            return photoname;
        }
        private StringBuilder CheckFields()
        {
            StringBuilder s = new StringBuilder();
            //if (string.IsNullOrWhiteSpace(_currentSotrudniki.Status_id.ToString()))
            //    s.AppendLine("Поле Статус пустое");
            //if (string.IsNullOrWhiteSpace(_currentSotrudniki.Dolzhnost))
            //    s.AppendLine("Поле Должность пустое");
            if (string.IsNullOrWhiteSpace(_currentSotrudniki.Stazh.ToString()))
                s.AppendLine("Поле Стаж пустое");
            return s;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder _error = CheckFields();
            if (_error.Length > 0)

            {
                MessageBox.Show(_error.ToString());
                return;

            }
            if (_currentSotrudniki.sotrudnik_id == 0)
            {
                predprEntities.GetContext().Sotrudniki.Add(_currentSotrudniki);
            }

            try
            {
                predprEntities.GetContext().SaveChanges();
                MessageBox.Show("Запись изменена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
