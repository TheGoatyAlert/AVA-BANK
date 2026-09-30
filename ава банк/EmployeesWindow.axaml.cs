using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace rabota;

public partial class EmployeesWindow : Window
{
    public EmployeesWindow()
    {
        InitializeComponent();

        Loaded += EmployeesWindow_Loaded;
    }

    private async void EmployeesWindow_Loaded(
        object? sender,
        RoutedEventArgs e)
    {
        await LoadEmployees();
    }

    private async System.Threading.Tasks.Task LoadEmployees()
    {
        try
        {
            if (App.DB == null)
            {
                CountText.Text = "База данных не подключена.";
                return;
            }

            var employees = await App.DB.GetEmployeesAsync();

            EmployeesList.ItemsSource = employees;

            CountText.Text =
                $"Найдено сотрудников: {employees.Count}";
        }
        catch (Exception ex)
        {
            CountText.Text =
                "Ошибка загрузки сотрудников: " + ex.Message;
        }
    }
}