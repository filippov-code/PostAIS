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
using System.Windows.Shapes;

namespace PostAIS
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private readonly User Client;
        private PackageToReceive[] packagesToReceive;
        public ClientWindow(User client)
        {
            InitializeComponent();
            Client = client;
            packageTypeComboBox.ItemsSource = Helper.GetPackageTypeRuNames();
            packageTypeComboBox.SelectedIndex = 0;
            deliveryTypeComboBox.ItemsSource = Helper.GetDeliveryTypeRuNames();
            deliveryTypeComboBox.SelectedIndex = 0;
            fioUserLabel.Content = $"{client.LastName} {client.FirstName} {client.MiddleName}";
        }

        private async void OnSendPackageButtonClick(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid(out PackageType packageType, out double weight, 
                out double width, out double height, out double length, 
                out string address, out DeliveryType deliveryType, true))
                return;

            double cost = Helper.GetDeliveryCost(length, width, height, weight, deliveryType);
            int code = Helper.GetServiceCode();
            using (var dbContext = new PostAisDbContext())
            {
                SendPackageService sendService = new SendPackageService()
                {
                    Created = DateTime.Now,
                    Code = code,
                    ClientId = Client.Id,
                    RecipientFIO = $"{Client.LastName} {Client.FirstName} {Client.MiddleName}",
                    Address = address,
                    DeliveryType = deliveryType,
                    PackageType = packageType,
                    Width = width,
                    Length = length,
                    Height = height,
                    Weight = weight,
                    Cost = cost
                };
                dbContext.SendPackageServices.Add(sendService);
                //dbContext.Services.Add(sendService);
                await dbContext.SaveChangesAsync();
            }
            MessageBox.Show($"Ваш код: {code}\nПримерное время ожидания: {Helper.GetWaitingTimeForClient()} мин.");
        }

        private bool IsFormValid(
            out PackageType packageType, 
            out double weight, 
            out double width,
            out double height, 
            out double length, 
            out string address, 
            out DeliveryType deliveryType,
            bool showMessageBoxes) //ugly method
        {
            packageType = default;
            weight = default;
            width = default;
            height = default;
            length = default;
            address = "";
            deliveryType = default;

            if (packageTypeComboBox.SelectedItem == null)
            {
                if (showMessageBoxes)
                    MessageBox.Show("Выберите тип посылки");
                return false;
            }
            if (string.IsNullOrWhiteSpace(weigthTextBox.Text))
            {
                if (showMessageBoxes)
                    MessageBox.Show("Укажите вес посылки");
                return false;
            }
            if (string.IsNullOrWhiteSpace(widthTextBox.Text))
            {
                if (showMessageBoxes)
                    MessageBox.Show("Укажите ширину посылки");
                return false;
            }
            if (string.IsNullOrWhiteSpace(heightTextBox.Text))
            {
                if (showMessageBoxes)
                    MessageBox.Show("Укажите высоту посылки");
                return false;
            }
            if (string.IsNullOrWhiteSpace(lengthTextBox.Text))
            {
                if (showMessageBoxes)
                    MessageBox.Show("Укажите длину посылки");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addressTextBox.Text))
            {
                if (showMessageBoxes)
                    MessageBox.Show("Укажите адрес посылки");
                return false;
            }
            if (deliveryTypeComboBox.SelectedItem == null)
            {
                if (showMessageBoxes)
                    MessageBox.Show("Выберите тип доставки");
                return false;
            }
            packageType = (PackageType)packageTypeComboBox.SelectedIndex;
            weight = double.Parse(weigthTextBox.Text);
            width = double.Parse(widthTextBox.Text);
            height = double.Parse(heightTextBox.Text);
            length = double.Parse(lengthTextBox.Text);
            address = addressTextBox.Text;
            deliveryType = (DeliveryType)deliveryTypeComboBox.SelectedIndex;
            return true;
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!receivePackageTab.IsSelected || e.Source is not TabControl)
                return;

            PackageToReceive[] packages;
            using (var dbContext = new PostAisDbContext())
            {
                packages = dbContext.PackagesToReceive
                    .Where(x => x.ReceiverTelephoneNumber == Client.TelephoneNumber)
                    .ToArray();
            }
            if (packages.Length == 0)
            {
                // Нет посылок для получения
            }
            else
            {
                packagesToReceiveDataGrid.ItemsSource = packages;
            }
        }

        private async void OnReceivePackageButtonClick(object sender, RoutedEventArgs e)
        {
            if (packagesToReceiveDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите посылку");
                return;
            }

            PackageToReceive packageToReceive = (PackageToReceive)packagesToReceiveDataGrid.SelectedItem;
            int code = Helper.GetServiceCode();
            ReceivePackageService receivePackageService = new ReceivePackageService()
            {
                Created = DateTime.Now,
                Code = code,
                ClientId = Client.Id,
                PackageToReceiveId = packageToReceive.Id
            };
            using (var dbContext = new PostAisDbContext())
            {
                dbContext.ReceivePackageServices.Add(receivePackageService);
                //dbContext.Services.Add(receivePackageService);
                await dbContext.SaveChangesAsync();
            }
            MessageBox.Show($"Ваш код: {code}\nПримерное время ожидания: {Helper.GetWaitingTimeForClient()} мин.");
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PackageTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDeliveryCostLabels();
        }

        private void DeliveryTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDeliveryCostLabels();
        }

        private void UpdateDeliveryCostLabels()
        {
            if (!IsFormValid(out PackageType packageType, out double weight, 
                out double width, out double height, out double length, 
                out string address, out DeliveryType deliveryType, false))
                return;

            double deliveryCost = Helper.GetDeliveryCost(length, width, height, weight, deliveryType);
            deliveryCostLabel.Content = deliveryCost + " руб.";
            string deliveryTime = Helper.GetDeliveryTime(deliveryType);
            deliveryTimeLabel.Content = deliveryTime;

        }
    }
}
