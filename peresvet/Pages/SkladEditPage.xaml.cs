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
using peresvet.Models;
using peresvet.Pages;
using System.IO;

namespace peresvet.Pages
{
    /// <summary>
    /// Логика взаимодействия для SkladEditPage.xaml
    /// </summary>
    public partial class SkladEditPage : Page
    {
        private Sklad _currentSklad = new Sklad();
        public SkladEditPage(Sklad selectedSklad)
        {
            InitializeComponent();
            if (selectedSklad != null)
                _currentSklad = selectedSklad;

            DataContext = _currentSklad;
            comboname.ItemsSource = predprEntities.GetContext().Products.ToList();
            comboname.DisplayMemberPath = "naimenivanie";
        }
        private StringBuilder CheckFields()
        {
            StringBuilder s = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentSklad.kolichestvo.ToString()))
                s.AppendLine("Поле Количество пустое");
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
            if (_currentSklad.sklad_id == 0)
            {
                predprEntities.GetContext().Sklad.Add(_currentSklad);
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
