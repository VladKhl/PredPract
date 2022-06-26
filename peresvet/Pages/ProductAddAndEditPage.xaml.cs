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
    /// Логика взаимодействия для ProductAddAndEditPage.xaml
    /// </summary>
    public partial class ProductAddAndEditPage : Page
    {

        private Products _currentProduct = new Products();
        public ProductAddAndEditPage(Products selectedProduct)
        {
            InitializeComponent();
            if (selectedProduct != null)
                _currentProduct = selectedProduct;

            DataContext = _currentProduct;
            ComboCategory.ItemsSource = predprEntities.GetContext().Category.ToList();
            ComboCategory.DisplayMemberPath = "name";
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private StringBuilder CheckFields()
        {
            StringBuilder s = new StringBuilder();
             if (string.IsNullOrWhiteSpace(_currentProduct.naimenivanie))
                s.AppendLine("Поле Наименование пустое");
            if (string.IsNullOrWhiteSpace(_currentProduct.articul.ToString()))
                s.AppendLine("Поле Артикул пустое");
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
            if (_currentProduct.product_id == 0)
            {
                var gg = ComboCategory.SelectedItem as Category;
                _currentProduct.category_id = gg.category_id;
                predprEntities.GetContext().Products.Add(_currentProduct);
            }

            try
            {
                var сс = ComboCategory.SelectedItem as Category;
                _currentProduct.category_id = сс.category_id;
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
