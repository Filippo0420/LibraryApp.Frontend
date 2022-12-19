using LibraryApp.WPF.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Windows;
using static LibraryApp.WPF.Configuration;

namespace LibraryApp.WPF
{
    /// <summary>
    /// Interaction logic for CustomerFormWindow.xaml
    /// </summary>
    public partial class CustomerFormWindow : Window
    {
        private HttpClient _httpClient;
        private Customer formCustomer;
        private string Mode;

        public CustomerFormWindow(Customer customer)
        {
            _httpClient = new HttpClient();
            formCustomer = customer;
            Mode = formCustomer.Id == 0 ? "Create" : "Update";
            InitializeComponent();

            ActionTitle.Content = Mode + " Customer Form";
            SubmitButton.Content = Mode;

            NameInput.Text = formCustomer.Name;
            PhoneInput.Text = formCustomer.Phone;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInput(NameInput.Text) && CheckPhone(PhoneInput.Text))
            {
                Customer customer = new Customer
                {
                    Name = NameInput.Text,
                    Phone = PhoneInput.Text
                };
                if (Mode == "Create")
                {
                    await _httpClient.PostAsJsonAsync($"{BaseUri}{AddCustomer}", customer);
                }
                else
                {
                    await _httpClient.PutAsJsonAsync($"{BaseUri}{UpdateCustomer}", customer);
                }
                this.Close();
            }
            else
            {
                ValidateMessage.Content = "Too long Name or Bad Phone Pattern";
            }
            
        }
        private bool CheckInput(string str)
        {
            return str.Length > 25 ? false : true;
        }

        private bool CheckPhone(string str)
        {
            var pattern = @"\\d{7,9}";
            Regex rx = new(pattern);
            
            
            return rx.IsMatch(str);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
