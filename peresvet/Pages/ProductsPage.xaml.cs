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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        int _itemcount = 0;
        public ProductsPage()
        {
            InitializeComponent();
            var products = predprEntities.GetContext().Products.OrderBy(p => p.naimenivanie).ToList();
            products.Insert(0, new Products
            {
                naimenivanie = "Все типы"
            }
            );

            LViewProducts.ItemsSource = predprEntities.GetContext().Products.OrderBy(p => p.naimenivanie).ToList();
            _itemcount = LViewProducts.Items.Count;
            TextBlockCount.Text = $" Результат запроса: {_itemcount} записей из {_itemcount}";
            ComboSort.ItemsSource = predprEntities.GetContext().Category.ToList();
            ComboSort.DisplayMemberPath = "name";
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gg = ComboSort.SelectedItem as Category;
            var prod = (from p in predprEntities.GetContext().Products
                        join c in predprEntities.GetContext().Category
                        on p.category_id equals c.category_id
                        where c.name == gg.name
                        select p).ToList();
            LViewProducts.ItemsSource = prod;
        }

        private void UpdateData()
        {
            // получаем текущие данные из бд
            var currentTovars = predprEntities.GetContext().Products.OrderBy(p => p.naimenivanie).ToList();
            // выбор тех товаров, в названии которых есть поисковая строка
            currentTovars = currentTovars.Where(p =>
            p.naimenivanie.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            // В качестве источника данных присваиваем список данных
            LViewProducts.ItemsSource = currentTovars;
            // отображение количества записей
            TextBlockCount.Text = $" Результат запроса: {currentTovars.Count} записей из {_itemcount}";
        }

        private void allshow_Click(object sender, RoutedEventArgs e)
        {
            var products = predprEntities.GetContext().Products.OrderBy(p => p.naimenivanie).ToList();
            products.Insert(0, new Products
            {
                naimenivanie = "Все типы"
            }
            );


            LViewProducts.ItemsSource = predprEntities.GetContext().Products.OrderBy(p => p.naimenivanie).ToList();
            _itemcount = LViewProducts.Items.Count;
            TextBlockCount.Text = $" Результат запроса: {_itemcount} записей из {_itemcount}";
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                predprEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProducts.ItemsSource = predprEntities.GetContext().Products.OrderBy(p => p.naimenivanie).ToList();
            }
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void showsklad_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SkladPage());
        }

        private void addtovar_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductAddAndEditPage(null));

        }

        private void deletetovar_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = LViewProducts.SelectedItem as Products;
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить продукт??? ", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    predprEntities.GetContext().Products.Remove(selectedProduct);
                    predprEntities.GetContext().SaveChanges();
                    MessageBox.Show("Записи удалены");
                    List<Products> products = predprEntities.GetContext().Products.OrderBy(p => p.product_id).ToList();
                    LViewProducts.ItemsSource = null;
                    LViewProducts.ItemsSource = products;
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
            Manager.MainFrame.Navigate(new ProductAddAndEditPage(LViewProducts.SelectedItem as Products));

        }
    }
}
