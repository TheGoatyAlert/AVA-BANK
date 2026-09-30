using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class DepositsWindow : Window
{
    public DepositsWindow()
    {
        InitializeComponent();

        Loaded += DepositsWindow_Loaded;
    }

    private async void DepositsWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadDeposits();
    }

    private async System.Threading.Tasks.Task LoadDeposits()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text = "База данных не подключена.";
                return;
            }

            var deposits = await App.DB.GetDepositsAsync();

            DepositsList.ItemsSource = deposits;

            CountText.Text =
                $"Найдено вкладов: {deposits.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
                "Ошибка загрузки вкладов: " + ex.Message;
        }
    }
}