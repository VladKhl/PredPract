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
using peresvet.Pages;

namespace peresvet.Pages
{
    /// <summary>
    /// Логика взаимодействия для SotrudnikiPage.xaml
    /// </summary>
    public partial class SotrudnikiPage : Page
    {
        predprEntities peresvetEntities = new predprEntities();
        public SotrudnikiPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DataGridSotrudniki.ItemsSource = null;
                predprEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                List<Sotrudniki> sotrudnikis = predprEntities.GetContext().Sotrudniki.OrderBy(p => p.sotrudnik_id).ToList();
                DataGridSotrudniki.ItemsSource = sotrudnikis;
            }
        }

        private void DataGridProducts_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddAndEditPage((sender as Button).DataContext as Sotrudniki));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddAndEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedSotrudnik = DataGridSotrudniki.SelectedItem as Sotrudniki;
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить сотрудника??? ", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    predprEntities.GetContext().Sotrudniki.Remove(selectedSotrudnik);
                    predprEntities.GetContext().SaveChanges();
                    MessageBox.Show("Записи удалены");
                    List<Sotrudniki> sotrudnikis = predprEntities.GetContext().Sotrudniki.OrderBy(p => p.sotrudnik_id).ToList();
                    DataGridSotrudniki.ItemsSource = null;
                    DataGridSotrudniki.ItemsSource = sotrudnikis;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка удаления", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
        }
    }
}
