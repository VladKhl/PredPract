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
        public AddAndEditPage(Sotrudniki selectedSotrudniki)
        {
            InitializeComponent();
            if (selectedSotrudniki != null)
                _currentSotrudniki = selectedSotrudniki;

            DataContext = _currentSotrudniki;
            ComboStatus.ItemsSource = predprEntities.GetContext().Status.ToList();
            ComboDolzh.ItemsSource = predprEntities.GetContext().Dolzhnost.ToList();
            ComboDolzh.DisplayMemberPath = "Name";
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
