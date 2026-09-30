using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class ClientsWindow : Window
{
    public ClientsWindow()
    {
        InitializeComponent();

        SearchButton.Click += SearchButton_Click;
        AddButton.Click += AddButton_Click;

        Loaded += ClientsWindow_Loaded;
    }

    private async void ClientsWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadClients();
    }

    private async void SearchButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadClients();
    }

    private async void AddButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        var window = new AddClientWindow();

        await window.ShowDialog(this);

        await LoadClients();
    }

    private async System.Threading.Tasks.Task LoadClients()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text = "База данных не подключена.";
                return;
            }

            string search = SearchBox.Text?.Trim() ?? "";

            var clients =
                await App.DB.GetClientsAsync(search);

            ClientsList.ItemsSource = clients;

            CountText.Text =
                $"Найдено клиентов: {clients.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
                "Ошибка: " + ex.Message;
        }
    }
}