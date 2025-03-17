using Avalonia.Controls;
using homework_2_spicymeatballs.ViewModels;
namespace homework_2_spicymeatballs.Views;

public partial class UserView : Window
{
    public UserView(string username)
    {
        InitializeComponent();
        DataContext = new UserViewModel(username);

    }
}