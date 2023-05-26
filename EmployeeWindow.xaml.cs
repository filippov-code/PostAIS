using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PostAIS.Database;
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
using System.Windows.Shapes;

namespace PostAIS
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private readonly User Employee;
        public EmployeeWindow(User employee)
        {
            InitializeComponent();
            Employee = employee;
            packageTypeComboBox.ItemsSource = Enum.GetValues<PackageType>();
            fioUserLabel.Content = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}";
        }

        private async void GenerateRegistrationCodeButtonClick(object sender, RoutedEventArgs e)
        {
            int code = new Random().Next(10000, 100000);
            RegistrationCode registrationCode = new RegistrationCode()
            {
                Code = code,
                EmployeeId = Employee.Id
            };
            using (var dbContext = new PostAisDbContext())
            {
                RegistrationCode? oldCode = await dbContext.RegistrationCodes
                    .Include(x => x.Employee)
                    .FirstOrDefaultAsync(x => x.EmployeeId == Employee.Id);

                if (oldCode != null)
                {
                    dbContext.RegistrationCodes.Remove(oldCode);
                }
                dbContext.RegistrationCodes.Add(registrationCode);
                await dbContext.SaveChangesAsync();
            }
            registrationCodeLabel.Content = code;
        }

        private async void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is not TabControl)
                return;

            if (servicesTabItem.IsSelected)
                UpdateServiceDataGrid();

            if (productsTabItem.IsSelected)
                UpdateProductsDataGrid();

            if (registrationCodeTabItem.IsSelected)
            {
                using (var dbContext = new PostAisDbContext())
                {
                    RegistrationCode code = await dbContext.RegistrationCodes.FirstOrDefaultAsync(x => x.EmployeeId == Employee.Id);
                    if (code != null)
                        registrationCodeLabel.Content = code.Code;
                }
            }

            if (addEmployeeTabItem.IsSelected)
                UpdateUsersDataGrid();

            if (removeEmployeeTabItem.IsSelected)
                UpdateEmployeesDataGrid();
        }

        private void ServicesDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Service service = servicesDataGrid.SelectedItem as Service;
            if (service != null)
            {
                clientFioLabel.Content = $"{service.Client?.LastName} {service.Client?.FirstName} {service.Client?.MiddleName}";
                clientCodeLabel.Content = service.Code;
                clientPassportLabel.Content = $"{service.Client?.PassportNumber} {service.Client?.PassportSeries}";
                clientTelephoneLabel.Content = service.Client?.TelephoneNumber;
                clientAddressLabel.Content = service.Client?.Address;
                clientServiceTypeLabel.Content = service.OperationType;
                serviceDetailsTextBlock.Text = service.ToString();
            }
            else
            {
                clientFioLabel.Content = "-";
                clientCodeLabel.Content = "-";
                clientPassportLabel.Content = "-";
                clientTelephoneLabel.Content = "-";
                clientAddressLabel.Content = "-";
                clientServiceTypeLabel.Content = "-";
                serviceDetailsTextBlock.Text = "";
            }
        }

        private async void DoneServiceButtonClick(object sender, RoutedEventArgs e)
        {
            Service service = servicesDataGrid.SelectedItem as Service;
            using (var dbContext = new PostAisDbContext())
            {
                if (service is SendPackageService)
                    dbContext.SendPackageServices.Remove((SendPackageService)service);
                else if (service is ReceivePackageService)
                {
                    ReceivePackageService receiveService = (ReceivePackageService)service;
                    dbContext.ReceivePackageServices.Remove(receiveService);
                    dbContext.PackagesToReceive.Remove(receiveService.PackageToReceive);
                }
                else if (service is ProductPurchaseService)
                {
                    var purchaseService = (ProductPurchaseService)service;
                    dbContext.ProductPurchaseServices.Remove(purchaseService);
                }

                await dbContext.SaveChangesAsync();
            }
            UpdateServiceDataGrid();
        }

        private async void UpdateServiceDataGrid()
        {
            List<Service> services = new();
            using var dbContext = new PostAisDbContext();
            {
                services.AddRange(dbContext.SendPackageServices.Include(x => x.Client).ToList());
                services.AddRange(dbContext.ReceivePackageServices.Include(x => x.Client).Include(x => x.PackageToReceive).ToList());
                services.AddRange(dbContext.ProductPurchaseServices.Include(x => x.ShoppingCart).ThenInclude(x => x.Product).ToList());
            }
            servicesDataGrid.ItemsSource = services.OrderBy(x => x.Created);
        }
        private async void AddPackageToReceiveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!AddPackageToReceiveFormValid())
                return;

            string senderFio = senderFioTextBox.Text;
            string senderAddress = senderAddressTextBox.Text;
            PackageType packageType = (PackageType)packageTypeComboBox.SelectedItem;
            string telephone = receiverTelephoneNumberTextBox.Text;
            PackageToReceive package = new PackageToReceive()
            {
                SenderFio = senderFio,
                SenderAddress = senderAddress,
                PackageType = packageType,
                ReceiverTelephoneNumber = telephone
            };
            using (var dbContext = new PostAisDbContext())
            {
                dbContext.PackagesToReceive.Add(package);
                await dbContext.SaveChangesAsync();
            }
            MessageBox.Show("Успешно");
        }

        private bool AddPackageToReceiveFormValid()
        {
            if (string.IsNullOrWhiteSpace(senderFioTextBox.Text))
            {
                MessageBox.Show("Укажите ФИО отправителя");
                return false;
            }
            if (string.IsNullOrWhiteSpace(senderAddressTextBox.Text))
            {
                MessageBox.Show("Укажите адрес отправителя");
                return false;
            }
            if (packageTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите тип посылки");
                return false;
            }
            if (string.IsNullOrWhiteSpace(receiverTelephoneNumberTextBox.Text))
            {
                MessageBox.Show("Укажите телефон получателя");
                return false;
            }
            return true;
        }

        private async void AddProductButtonClick(object sender, RoutedEventArgs e)
        {
            if (!AddProductFormValid())
                return;

            string title = productTitleTextBox.Text;
            string description = productDescriptionTextBox.Text;
            double price = double.Parse(productPriceTextBox.Text);
            Product product = new Product()
            {
                Title = title,
                Description = description,
                Price = price
            };
            using (var dbContext = new PostAisDbContext())
            {
                dbContext.Products.Add(product);
                await dbContext.SaveChangesAsync();
            }
            UpdateProductsDataGrid();
        }

        private bool AddProductFormValid()
        {
            if (string.IsNullOrWhiteSpace(productTitleTextBox.Text))
            {
                MessageBox.Show("Введите название товара");
                return false;
            }
            if (string.IsNullOrWhiteSpace(productDescriptionTextBox.Text))
            {
                MessageBox.Show("Введите описание товара");
                return false;
            }
            if (string.IsNullOrWhiteSpace(productPriceTextBox.Text) || !double.TryParse(productPriceTextBox.Text, out double price))
            {
                MessageBox.Show("Введите корректную цену товара");
                return false;
            }
            return true;
        }

        private async void DeleteProductButtonClick(object sender, RoutedEventArgs e)
        {
            if (productsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар для удаления");
                return;
            }
            Product product = (Product)productsDataGrid.SelectedItem;
            using (var dbContext = new PostAisDbContext())
            {
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
            }
            UpdateProductsDataGrid();
        }

        private void UpdateProductsDataGrid()
        {
            using (var dbContext = new PostAisDbContext())
                productsDataGrid.ItemsSource = dbContext.Products.ToArray();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void AddEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            if (addEmployeeDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента");
                return;
            }

            using (var dbContext = new PostAisDbContext())
            {
                var client = addEmployeeDataGrid.SelectedItem as User;
                client.Role = UserRole.Employee;
                dbContext.Users.Update(client);
                await dbContext.SaveChangesAsync();
            }
            UpdateUsersDataGrid();
        }

        private void UpdateUsersDataGrid()
        {
            using (var dbContext = new PostAisDbContext())
            {
                addEmployeeDataGrid.ItemsSource = dbContext.Users.Where(x => x.Role == UserRole.Client).ToArray();
            }
        }

        private void UpdateEmployeesDataGrid()
        {
            using (var dbContext = new PostAisDbContext())
            {
                removeEmployeeDataGrid.ItemsSource = dbContext.Users.Where(x => x.Role == UserRole.Employee).ToArray();
            }
        }

        private async void RemoveEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            if (removeEmployeeDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника");
                return;
            }

            using (var dbContext = new PostAisDbContext())
            {
                var employee = removeEmployeeDataGrid.SelectedItem as User;
                employee.Role = UserRole.Client;
                dbContext.Users.Update(employee);
                await dbContext.SaveChangesAsync();
            }
            UpdateEmployeesDataGrid();
        }
    }
}
