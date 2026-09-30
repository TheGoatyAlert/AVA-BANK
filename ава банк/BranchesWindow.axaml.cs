using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class BranchesWindow : Window
{
    public BranchesWindow()
    {
        InitializeComponent();

        Loaded += BranchesWindow_Loaded;
    }

    private async void BranchesWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadBranches();
    }

    private async System.Threading.Tasks.Task LoadBranches()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text = "База данных не подключена.";
                return;
            }

            var branches = await App.DB.GetBranchesAsync();

            BranchesList.ItemsSource = branches;

            CountText.Text =
                $"Найдено отделений: {branches.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
                "Ошибка загрузки отделений: " + ex.Message;
        }
    }
}