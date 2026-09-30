using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class TransactionsWindow : Window
{
    public TransactionsWindow()
    {
        InitializeComponent();

        SearchButton.Click += SearchButton_Click;
        TransferButton.Click += TransferButton_Click;

        Loaded += TransactionsWindow_Loaded;
    }

    private async void TransactionsWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadTransactions();
    }

    private async void SearchButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadTransactions();
    }

    private async void TransferButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        try
        {
            if (App.DB == null)
            {
                MessageText.Text =
                    "База данных не подключена.";
                return;
            }

            string senderAccount =
                SenderBox.Text?.Trim() ?? "";

            string receiverAccount =
                ReceiverBox.Text?.Trim() ?? "";

            string description =
                DescriptionBox.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(senderAccount))
                throw new Exception(
                    "Введите счет отправителя.");

            if (string.IsNullOrWhiteSpace(receiverAccount))
                throw new Exception(
                    "Введите счет получателя.");

            if (!decimal.TryParse(
                    AmountBox.Text,
                    out decimal amount))
            {
                throw new Exception(
                    "Введите корректную сумму.");
            }

            if (amount <= 0)
                throw new Exception(
                    "Сумма должна быть больше нуля.");

            await App.DB.CreateTransactionAsync(
                senderAccount,
                receiverAccount,
                amount,
                description);

            MessageText.Text =
                "Перевод успешно выполнен.";

            SenderBox.Text = "";
            ReceiverBox.Text = "";
            AmountBox.Text = "";
            DescriptionBox.Text = "";

            await LoadTransactions();
        }
        catch (Exception ex)
        {
            MessageText.Text =
                ex.Message;
        }
    }

    private async System.Threading.Tasks.Task LoadTransactions()
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

            if (string.IsNullOrWhiteSpace(search))
            {
                var transactions =
                    await App.DB.GetTransactionsAsync();

                TransactionsList.ItemsSource =
                    transactions;

                CountText.Text =
                    $"Операций: {transactions.Count}";

                return;
            }

            var allTransactions =
                await App.DB.GetTransactionsAsync();

            var filtered =
                allTransactions.FindAll(
                    x => x.AccountNumber.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase));

            TransactionsList.ItemsSource =
                filtered;

            CountText.Text =
                $"Найдено операций: {filtered.Count}";
        }
        catch (Exception ex)
        {
            MessageText.Text =
                "Ошибка: " + ex.Message;
        }
    }
}