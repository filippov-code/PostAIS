using Microsoft.EntityFrameworkCore;
using PostAIS.Database;
using PostAIS.Helpers;
using PostAIS.Models;
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

namespace PostAIS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Product? selectedProduct;
        private List<ShoppingCartItem> shoppingCart = new();
        public MainWindow()
        {
            InitializeComponent();
            using (var dbContext = new PostAisDbContext())
            {
                if (dbContext.Users.Count() == 0)
                {
                    dbContext.Users.Add(new User()
                    {
                        //Id = 1,
                        FirstName = "Employee",
                        MiddleName = " ",
                        LastName = " ",
                        Birthday = new DateTime(),
                        PassportNumber = "0000",
                        PassportSeries = "000000",
                        Address = " ",
                        TelephoneNumber = "123",
                        Password = "123",
                        Created = DateTime.Now,
                        Role = UserRole.Employee
                    });
                }
                dbContext.SaveChanges();

                productsDataGrid.ItemsSource = dbContext.Products.ToArray();
            }
        }

        private async void OnLogInButtonClick(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid())
                return;

            string telephone = telephoneTextBox.Text;
            string password = passwordBox.Password;
            using (var dbContext = new PostAisDbContext())
            {
                User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.TelephoneNumber == telephone && x.Password == password);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден");
                    return;
                }

                if (user.Role == UserRole.Client)
                {
                    var clientWindow = new ClientWindow(user);
                    clientWindow.Closing += (object sender, System.ComponentModel.CancelEventArgs e) => new MainWindow().Show();
                    clientWindow.Show();

                }
                else if (user.Role == UserRole.Employee)
                {
                    var employeeWindow =  new EmployeeWindow(user);
                    employeeWindow.Closing += (object sender, System.ComponentModel.CancelEventArgs e) => new MainWindow().Show();
                    employeeWindow.Show();
                }
                Close();
            }
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(telephoneTextBox.Text))
            {
                MessageBox.Show("Введите номер телефона");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Введите пароль");
                return false;
            }
            return true;
        }

        private void OnGetRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            var clientRegistrationWindow = new ClientRegistrationWindow();
            clientRegistrationWindow.Closing += (object sender, System.ComponentModel.CancelEventArgs e) => new MainWindow().Show();
            clientRegistrationWindow.Show();
            Close();
        }

        private async void BuyButtonClick(object sender, RoutedEventArgs e)
        {
            if (shoppingCart.Count == 0)
            {
                MessageBox.Show("Добавьте товары в корзину");
                return;
            }

            int code = Helper.GetServiceCode();
            ProductPurchaseService purchaseService = new ProductPurchaseService()
            {
                Created = DateTime.Now,
                Code = code
            };
            purchaseService.ShoppingCart.AddRange(shoppingCart.Select(x => new ShoppingCartItem { ProductId = x.ProductId, Count = x.Count}));

            using (var dbContext = new PostAisDbContext())
            {
                dbContext.ProductPurchaseServices.Add(purchaseService);
                await dbContext.SaveChangesAsync();
            }
            MessageBox.Show($"Ваш код: {code}\nПримерное время ожидания: {Helper.GetWaitingTimeForClient()} мин.");
        }

        private void AddToShoppingCartButtonClick(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
                return;

            var existingItem = shoppingCart.FirstOrDefault(x => x.Product == selectedProduct);
            if (existingItem != null)
            {
                existingItem.Count++;
            }
            else
            {
                shoppingCart.Add(new(selectedProduct, 1) { ProductId = selectedProduct.Id});
            }
            UpdateShoppingCartDataGrid();
        }

        private void RemoveFromShoppingCartButtonClick(object sender, RoutedEventArgs e)
        {
            if (shoppingCartDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукт из корзины");
                return;
            }
            ShoppingCartItem item = shoppingCartDataGrid.SelectedItem as ShoppingCartItem;
            var existingItem = shoppingCart.FirstOrDefault(x => x == item);
            if (existingItem.Count > 1)
            {
                existingItem.Count--;
            }
            else
            {
                shoppingCart.Remove(existingItem);
            }
            UpdateShoppingCartDataGrid();
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is not TabControl)
                return;

            if (shoppingTabItem.IsSelected)
                UpdateProductsDataGrid();
        }

        private void UpdateProductsDataGrid()
        {
            //using (var dbContext = new PostAisDbContext())
                //productsDataGrid.ItemsSource = dbContext.Products.ToArray();
        }

        private void ProductsDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsDataGrid.SelectedItem == null)
            {
                selectedProduct = null;
                productTitleLabel.Content = "";
                productDescriptionTextBox.Text = "";
                productPriceLabel.Content = "";
                return;
            }

            selectedProduct = productsDataGrid.SelectedItem as Product;
            productTitleLabel.Content = selectedProduct?.Title;
            productDescriptionTextBox.Text = selectedProduct?.Description;
            productPriceLabel.Content = selectedProduct?.Price;
        }
        private void UpdateShoppingCartDataGrid()
        {
            shoppingCartDataGrid.ItemsSource = shoppingCart;
            shoppingCartDataGrid.Items.Refresh();
            totalPriceLabel.Content = shoppingCart.Sum(x => x.Count * x.Product.Price) + " рублей";
        }

    }
}
