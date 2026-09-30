using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;

namespace rabota;

public partial class AccountsWindow : Window
{
    private bool _sortByBalance = false;

    public AccountsWindow()
    {
        InitializeComponent();

        SearchButton.Click += SearchButton_Click;
        SortButton.Click += SortButton_Click;

        Loaded += AccountsWindow_Loaded;
    }

    private async void AccountsWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadFilters();
        await LoadAccounts();
    }

    private async System.Threading.Tasks.Task LoadFilters()
    {
        if (App.DB == null)
            return;

        var clients =
            await App.DB.GetClientsAsync();

        ClientCombo.ItemsSource =
            clients.Select(x => x.FullName).ToList();

        ClientCombo.ItemsSource =
            new[] { "Все клиенты" }
            .Concat(clients.Select(x => x.FullName))
            .ToList();

        ClientCombo.SelectedIndex = 0;

        var types =
            await App.DB.GetAccountTypesAsync();

        TypeCombo.ItemsSource =
            new[] { "Все типы" }
            .Concat(types)
            .ToList();

        TypeCombo.SelectedIndex = 0;
    }

    private async void SearchButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadAccounts();
    }

    private async void SortButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _sortByBalance = true;

        await LoadAccounts();
    }

    private async System.Threading.Tasks.Task LoadAccounts()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text =
                    "База данных не подключена.";
                return;
            }

            string? accountType = null;

            if (TypeCombo.SelectedIndex > 0)
            {
                accountType =
                    TypeCombo.SelectedItem?.ToString();
            }

            int? clientId = null;

            if (ClientCombo.SelectedIndex > 0)
            {
                string clientName =
                    ClientCombo.SelectedItem?.ToString() ?? "";

                var clients =
                    await App.DB.GetClientsAsync();

                var client =
                    clients.FirstOrDefault(
                        x => x.FullName == clientName);

                if (client != null)
                    clientId = client.ClientId;
            }

            string search =
                SearchBox.Text?.Trim() ?? "";

            var accounts =
                await App.DB.GetAccountsAsync(
                    clientId,
                    accountType,
                    search,
                    _sortByBalance);

            AccountsList.ItemsSource = accounts;

            CountText.Text =
                $"Найдено счетов: {accounts.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
                "Ошибка: " + ex.Message;
        }
    }
}