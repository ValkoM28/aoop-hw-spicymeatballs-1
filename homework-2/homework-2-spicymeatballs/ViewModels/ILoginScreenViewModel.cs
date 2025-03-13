using System.Windows.Input; 

namespace homework_2_spicymeatballs.ViewModels;

public interface ILoginScreenViewModel
{
    string Username { get; set; }
    string Password { get; set; }
    bool IsPasswordVisible { get; set; }
    
    string PasswordMask { get; }
    void TogglePasswordVisibilityCommand();
    void LoginCommand(); 
}