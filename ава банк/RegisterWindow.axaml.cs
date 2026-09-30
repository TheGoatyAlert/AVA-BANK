using Avalonia.Controls; 
using Avalonia.Interactivity; 
namespace rabota; 
public partial class RegisterWindow:Window { 
    public RegisterWindow(){InitializeComponent();RegisterButton.Click+=Reg;BackButton.Click+=(_,_)=>Close();} 
    async void Reg(object? s,RoutedEventArgs e){
        try{
            if(App.DB==null)
                throw new Exception("База данных не подключена.");
            string role=(RoleCombo.SelectedItem as ComboBoxItem)?.Content?.ToString()=="Менеджер"?"менеджер":"операционист";
            await App.DB.RegisterUserAsync(UsernameBox.Text?.Trim()??"",PasswordBox.Text??"",role,FullNameBox.Text?.Trim()??"");
            ErrorText.Text="Пользователь зарегистрирован.";
        }
        catch(Exception ex){ErrorText.Text=ex.Message;
        }
    }
}
