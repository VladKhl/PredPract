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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<User> users = predprEntities.GetContext().User.ToList();
                User u = users.FirstOrDefault(p => p.Password == TbPass.Password && p.Login == TbLogin.Text);

                if (u == null)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }
                switch (u.Login)
                {
                    case "admin":
                        NavigationService.Navigate(new SotrudnikiPage());
                        break;
                    case "123":
                        NavigationService.Navigate(new ProductsPage());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
