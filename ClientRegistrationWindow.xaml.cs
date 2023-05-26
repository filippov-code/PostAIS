using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для ClientRegistrationWindow.xaml
    /// </summary>
    public partial class ClientRegistrationWindow : Window
    {
        public ClientRegistrationWindow()
        {
            InitializeComponent();
        }

        private async void OnRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid())
                return;

            string fio = FioTextBox.Text;
            string[] fioSplitted = fio.Split(" ");
            string firstName = fioSplitted[1];
            string middleName = fioSplitted[2];
            string lastName = fioSplitted[0];
            DateTime birthday = birthdayDatePicker.SelectedDate ?? throw new ArgumentNullException("Дата рождения указана некорректно");
            string passportNumber = passportNumberTextBox.Text;
            string passportSeries = passportSeriesTextBox.Text;
            string address = addressTextBox.Text;
            string telephone = telephoneNumberTextBox.Text;
            string password = passwordTextBox.Password;
            string employeeCodeString = employeeCodeTextBox.Text;
            int employeeCode = int.Parse(employeeCodeString);
            
            using (var dbCotext = new PostAisDbContext())
            {
                RegistrationCode? emloyeeCode = await dbCotext.RegistrationCodes
                    .Include(x => x.Employee)
                    .FirstOrDefaultAsync(x => x.Code == employeeCode);
                if (emloyeeCode == null)
                {
                    MessageBox.Show("Код сотрудника недействителен");
                    return;
                }
                User newUser = new User
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Birthday = birthday,
                    PassportNumber = passportNumber,
                    PassportSeries = passportSeries,
                    Address = address,
                    TelephoneNumber = telephone,
                    Password = password,
                    Created = DateTime.Now,
                    RegisteringEmployeeId = emloyeeCode.EmployeeId,
                    Role = UserRole.Client
                };
                await dbCotext.Users.AddAsync(newUser);
                await dbCotext.SaveChangesAsync();
            }
            MessageBox.Show("Успешно");
            Close();
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(FioTextBox.Text))
            {
                MessageBox.Show("Введите ФИО");
                return false;
            }
            if (birthdayDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Введите дату рождения");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addressTextBox.Text))
            {
                MessageBox.Show("Введите адрес");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passportNumberTextBox.Text))
            {
                MessageBox.Show("Введите номер паспорта");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passportSeriesTextBox.Text))
            {
                MessageBox.Show("Введите серию паспорта");
                return false;
            }
            if (string.IsNullOrWhiteSpace(telephoneNumberTextBox.Text))
            {
                MessageBox.Show("Введите номер телефона");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Password))
            {
                MessageBox.Show("Введите пароль");
                return false;
            }
            if (passwordTextBox.Password != passwordRepeatTextBox.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return false;
            }
            if (string.IsNullOrWhiteSpace(employeeCodeTextBox.Text))
            {
                MessageBox.Show("Введите код сотрудника");
                return false;
            }
            return true;
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
