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
        private string _filePath = null;
        private string _photoName = null;
        private static string _currentDirectory = Directory.GetCurrentDirectory() + @"\Images\";
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
                predprEntities.GetContext().Products.Add(_currentProduct);
            }

            try
            {
                if (_filePath != null)
                {

                    string photo = ChangePhotoName();
                    string dest = _currentDirectory + photo;
                    File.Copy(_filePath, dest);
                }
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
