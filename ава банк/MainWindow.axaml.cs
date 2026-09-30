using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class MainWindow : Window
{
    private readonly string _role;

    public MainWindow()
    {
        InitializeComponent();

        _role = App.CurrentUser?.Role ?? "";

        ClientsButton.Click += ClientsButton_Click;
        AccountsButton.Click += AccountsButton_Click;
        TransactionsButton.Click += TransactionsButton_Click;
        LoansButton.Click += LoansButton_Click;
        DepositsButton.Click += DepositsButton_Click;
        EmployeesButton.Click += EmployeesButton_Click;
        BranchesButton.Click += BranchesButton_Click;
        PaymentsButton.Click += PaymentsButton_Click;

        RefreshButton.Click += RefreshButton_Click;
        LogoutButton.Click += LogoutButton_Click;

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(
    object? sender,
    RoutedEventArgs e)
    {
        UserNameText.Text =
        App.CurrentUser?.FullName ?? "Пользователь";

        if (_role == "менеджер")
            RoleText.Text = "Менеджер";
        else
            RoleText.Text = "Операционист";

        if (_role == "менеджер")
            TransactionsButton.IsVisible = false;

        await LoadRecentActions();
    }

    private async void RefreshButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await LoadRecentActions();
    }

    private async System.Threading.Tasks.Task LoadRecentActions()
    {
        try
        {
            if (App.DB == null)
                return;

            var actions =
            await App.DB.GetRecentActionsAsync();

            RecentActionsList.ItemsSource = actions;
        }
        catch (Exception)
        {
            RecentActionsList.ItemsSource = null;
        }
    }

    private async void ClientsButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new ClientsWindow().ShowDialog(this);
    }

    private async void AccountsButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new AccountsWindow().ShowDialog(this);
    }

    private async void TransactionsButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new TransactionsWindow().ShowDialog(this);

        await LoadRecentActions();
    }

    private async void LoansButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new LoansWindow().ShowDialog(this);

        await LoadRecentActions();
    }

    private async void DepositsButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new DepositsWindow().ShowDialog(this);

        await LoadRecentActions();
    }

    private async void EmployeesButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new EmployeesWindow().ShowDialog(this);
    }

    private async void BranchesButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new BranchesWindow().ShowDialog(this);
    }

    private async void PaymentsButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        await new PaymentsWindow().ShowDialog(this);
    }

    private void LogoutButton_Click(
    object? sender,
    RoutedEventArgs e)
    {
        App.CurrentUser = null;

        var loginWindow = new LoginWindow();

        loginWindow.Show();

        Close();
    }
}