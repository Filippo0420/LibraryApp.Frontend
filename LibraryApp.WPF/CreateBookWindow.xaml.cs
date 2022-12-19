using LibraryApp.WPF.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using static LibraryApp.WPF.Configuration;

namespace LibraryApp.WPF
{
    /// <summary>
    /// Interaction logic for CreateBookWindow.xaml
    /// </summary>
    public partial class CreateBookWindow : Window
    {
        private HttpClient _httpClient;
        private Book formBook;
        private string Mode;

        public CreateBookWindow(Book book)
        {
            _httpClient= new HttpClient();
            formBook = book;
            InitializeComponent();

            Mode = formBook.Id == 0 ? "Create" : "Update";
            ActionTitle.Content = Mode + " Book Form";
            SubmitButton.Content = Mode;
            TitleInput.Text = book.Title;
            AuthorInput.Text = book.Author;
            GenreInput.Text = book.Genre;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = new Book
            {
                Id = formBook.Id,
                Title = TitleInput.Text,
                Author = AuthorInput.Text,
                Genre = GenreInput.Text,
            };
            if(Mode == "Create")
            {
                await _httpClient.PostAsJsonAsync($"{BaseUri}{AddBook}", book);
            }
            else
            {
                await _httpClient.PutAsJsonAsync($"{BaseUri}{UpdateBook}", book);
            }
            
            
            this.Close();
        }
    }
}
