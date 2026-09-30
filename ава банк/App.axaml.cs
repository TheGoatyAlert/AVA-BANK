using Avalonia; 
using Avalonia.Controls.ApplicationLifetimes; 
using Avalonia.Markup.Xaml; 
using rabota.Data; 
using rabota.Models; 
using rabota.Services;
namespace rabota;
public partial class App:Application { 
    public static DatabaseService? DB{get;private set;} 
    public static User? CurrentUser{get;set;} 
    public override void Initialize()=>AvaloniaXamlLoader.Load(this); 
    public override void OnFrameworkInitializationCompleted(){
        DB=new DatabaseService(Database.ConnectionString);
        if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime d)
            d.MainWindow=new LoginWindow();base.OnFrameworkInitializationCompleted();
    }
}
