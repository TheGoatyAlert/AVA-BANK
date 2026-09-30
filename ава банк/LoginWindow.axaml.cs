using Avalonia.Controls; 
using Avalonia.Interactivity; 

namespace rabota; 
public partial class LoginWindow:Window { 
    public LoginWindow(){
        InitializeComponent();
        LoginButton.Click+=Login;RegisterButton.Click+=Register;
    } 
    async void Login(object? s,RoutedEventArgs e){
        try{if(App.DB==null)throw new Exception("База данных не подключена.");
            var u=await App.DB.AuthenticateAsync(UsernameBox.Text?.Trim()??"",PasswordBox.Text??"");
            if(u==null){
                ErrorText.Text="Неверный логин или пароль.";
            return;
            }
            App.CurrentUser=u;new MainWindow().Show();Close();
        }
        catch(Exception ex){ErrorText.Text=ex.Message;}
    }
    void Register(object? s,RoutedEventArgs e)=>new RegisterWindow().ShowDialog(this);
}
