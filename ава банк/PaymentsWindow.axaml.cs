using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class PaymentsWindow : Window
{
    public PaymentsWindow()
    {
        InitializeComponent();

        SearchButton.Click += SearchButton_Click;

        Loaded += PaymentsWindow_Loaded;
    }

    private async void PaymentsWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadPayments();
    }

    private async void SearchButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadPayments();
    }

    private async System.Threading.Tasks.Task LoadPayments()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text =
                    "База данных не подключена.";
                return;
            }

            string search =
                SearchBox.Text?.Trim() ?? "";

            var payments =
                await App.DB.GetPaymentsAsync(
                    string.IsNullOrWhiteSpace(search)
                        ? null
                        : search);

            PaymentsList.ItemsSource = payments;

            CountText.Text =
                $"Найдено платежей: {payments.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
                "Ошибка: " + ex.Message;
        }
    }
}