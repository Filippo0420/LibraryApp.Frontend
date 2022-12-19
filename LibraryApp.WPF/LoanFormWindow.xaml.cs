using LibraryApp.WPF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using static LibraryApp.WPF.Configuration;

namespace LibraryApp.WPF
{
    /// <summary>
    /// Interaction logic for LoanFormWindow.xaml
    /// </summary>
    public partial class LoanFormWindow : Window
    {
        private HttpClient _httpClient;
        private List<Customer> _customers;
        private List<Book> _books;
        private Loan formLoan;
        private string Mode;

        public LoanFormWindow(Loan formloan)
        {
            _httpClient = new HttpClient();
            _books = new();
            _customers = new();
            formLoan = formloan;
            InitializeComponent();
            
            Mode = formLoan.Id == 0 ? "Create" : "Update";
            ActionTitle.Content = Mode + " Loan Form";
            SubmitButton.Content = Mode;


            CustomerInput.SelectedItem = formLoan.CustomerId.ToString();
            BookInput.Text = formLoan.BookId.ToString();
            LoanDateInput.SelectedDate = formLoan.LoanDate;
            ReturnDateInput.SelectedDate = formLoan.ReturnDate;
            
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            
            Loan loan = new Loan
            {
                CustomerId = _customers.Where(c => c.Name == CustomerInput.Text).First().Id,
                BookId = _books.Where(b => b.Title == BookInput.Text).First().Id,
                LoanDate = LoanDateInput.SelectedDate,
                ReturnDate = ReturnDateInput.SelectedDate
            };
            await _httpClient.PostAsJsonAsync($"{BaseUri}{AddLoan}", loan);

            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        private async void LoanFormInitialized(object sender, System.EventArgs e)
        {
            _customers = await _httpClient.GetFromJsonAsync<List<Customer>>($"{BaseUri}{GetCustomers}");
            _books = await _httpClient.GetFromJsonAsync<List<Book>>($"{BaseUri}{GetBooks}");
            BookInput.ItemsSource = _books?.Select(b => b.Title);
            CustomerInput.ItemsSource = _customers?.Select(c => c.Name);
        }
    }
}
