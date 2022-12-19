using LibraryApp.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using static LibraryApp.WPF.Configuration;

namespace LibraryApp.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _httpClient;
        private List<Customer>? _customers;
        private List<Book>? _books;
        private List<Loan>? _loans;

        public MainWindow()
        {
            _httpClient = new HttpClient();
            _customers = new List<Customer>();
            _books = new List<Book>();
            _loans = new();
            InitializeComponent();

            
        }

        private async void ShowBookDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var books = await _httpClient.GetFromJsonAsync<List<Book>>($"{BaseUri}{GetBooks}");
            IEnumerable<Book>? selectedBooks = books.Where(b => 
                b.Title.Contains(TitleSearch.Text) && 
                b.Author.Contains(AuthorSearch.Text) && 
                b.Genre.Contains(GenreSearch.Text));
            ShowBookData.ItemsSource = selectedBooks;
        }

        private async void ShowCustomerDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var customers = await _httpClient.GetFromJsonAsync<List<Customer>>($"{BaseUri}{GetCustomers}");
            IEnumerable<Customer>? selectedCustomers = customers?.Where(c => 
                c.Name.Contains(NameSearch.Text) && 
                c.Phone.Contains(PhoneSearch.Text));
            ShowCustomerData.ItemsSource = selectedCustomers;
        }

        private void CreateCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerFormWindow customerFormWindow = new CustomerFormWindow(new Customer());
            customerFormWindow.ShowDialog();
        }

        private void CreateBookBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateBookWindow createBookWindow = new CreateBookWindow(new Book());
            createBookWindow.ShowDialog();
        }

        private void DataGridDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShowBookData.SelectedItem != null)
            {
                Book? passedBook;
                if (ShowBookData.SelectedItem != null)
                {
                    passedBook = ShowBookData.SelectedItem as Book;
                }
                else
                {
                    passedBook = new Book();
                }
                CreateBookWindow createBookWindow = new CreateBookWindow(passedBook);
                createBookWindow.ShowDialog();
            }
        }
        private void CustomerDataGridDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShowCustomerData.SelectedItem != null)
            {
                Customer? passedCustomer;
                if (ShowCustomerData.SelectedItem != null)
                {
                    passedCustomer = ShowCustomerData.SelectedItem as Customer;
                }
                else
                {
                    passedCustomer = new Customer();
                }
                CustomerFormWindow createCustomerWindow = new CustomerFormWindow(passedCustomer);
                createCustomerWindow.ShowDialog();
            }
        }

        private async void DeleteCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShowCustomerData.SelectedItem != null)
            {
                var selectedItem = (Customer)ShowCustomerData.SelectedItem;
                int selectedCustomerId = selectedItem.Id;
                await _httpClient.DeleteAsync($"{BaseUri}{DeleteCustomer}{selectedCustomerId}");
            }
            ReloadData();
        }

        private async void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ShowBookData.SelectedItem != null)
            {
                var selectedItem = (Book)ShowBookData.SelectedItem;
                int selectedBookId = selectedItem.Id;
                await _httpClient.DeleteAsync($"{BaseUri}{DeleteBook}{selectedBookId}");
            }
            ReloadData();
        }

        private async void ReloadData()
        {
            _customers = await _httpClient.GetFromJsonAsync<List<Customer>>($"{BaseUri}{GetCustomers}");
            _books = await _httpClient.GetFromJsonAsync<List<Book>>($"{BaseUri}{GetBooks}");
            _loans = await _httpClient.GetFromJsonAsync<List<Loan>>($"{BaseUri}{GetLoans}");

            ShowBookData.ItemsSource = _books;
            ShowCustomerData.ItemsSource = _customers;
            ShowLoanData.ItemsSource = _loans;

            IdSearch.Text = string.Empty;
        }

        private void ShowLoanData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShowLoanData.SelectedItem != null)
            {
                Loan? passedLoan;
                if (ShowLoanData.SelectedItem != null)
                {
                    passedLoan = ShowLoanData.SelectedItem as Loan;
                }
                else
                {
                    passedLoan = new Loan();
                }
                LoanFormWindow createLoanWindow = new LoanFormWindow(passedLoan);
                createLoanWindow.ShowDialog();
            }
        }

        private async void DeleteLoanBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShowLoanData.SelectedItem != null)
            {
                var selectedItem = (Loan)ShowLoanData.SelectedItem;
                int selectedLoanId = selectedItem.Id;
                await _httpClient.DeleteAsync($"{BaseUri}{DeleteLoan}{selectedLoanId}");
            }
            ReloadData();
        }

        private void CreateLoanBtn_Click(object sender, RoutedEventArgs e)
        {
            LoanFormWindow loanFormWindow = new LoanFormWindow(new Loan());
            loanFormWindow.ShowDialog();
        }

        private async void ShowLoanDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if(IdSearch.Text != "")
            {
                ShowLoanData.ItemsSource = _loans.Where(x => x.Id == Int64.Parse(IdSearch.Text));
            }
            else
            {
                ReloadData();
            }
            
            
        }

        private void LoanTabInitialize(object sender, EventArgs e)
        {
            ReloadData();
        }
    }
}
