using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class LoansWindow : Window
{
    public LoansWindow()
    {
        InitializeComponent();

        Loaded += LoansWindow_Loaded;
    }

    private async void LoansWindow_Loaded(
    object? sender,
    RoutedEventArgs e)
    {
        await LoadLoans();
    }

    private async System.Threading.Tasks.Task LoadLoans()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text = "База данных не подключена.";
                return;
            }

            var loans = await App.DB.GetLoansAsync();

            LoansList.ItemsSource = loans;

            CountText.Text =
            $"Найдено кредитов: {loans.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
            "Ошибка загрузки кредитов: " + ex.Message;
        }
    }
}