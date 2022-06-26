using System;
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

namespace peresvet.Pages
{
    /// <summary>
    /// Логика взаимодействия для SkladPage.xaml
    /// </summary>
    public partial class SkladPage : Page
    {
        public SkladPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataGridSklad.ItemsSource = null;
                predprEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                List<Sklad> sklads = predprEntities.GetContext().Sklad.OrderBy(p => p.sklad_id).ToList();
                DataGridSklad.ItemsSource = sklads;
            }
        }

        private void DataGridSklad_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedSklad = DataGridSklad.SelectedItem as Sklad;
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить товар со склада??? ", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    predprEntities.GetContext().Sklad.Remove(selectedSklad);
                    predprEntities.GetContext().SaveChanges();
                    MessageBox.Show("Записи удалены");
                    List<Sklad> sklads = predprEntities.GetContext().Sklad.OrderBy(p => p.sklad_id).ToList();
                    DataGridSklad.ItemsSource = null;
                    DataGridSklad.ItemsSource = sklads;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка удаления", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
        }

        private void redacttovar_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SkladEditPage(DataGridSklad.SelectedItem as Sklad));
        }

        private void addtovar_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SkladEditPage(null));
        }

        private void DataGridSklad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDelete.IsEnabled = true;
            redacttovar.IsEnabled = true;
        }
    }
}
