using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using homework_2_spicymeatballs.Models;


namespace homework_2_spicymeatballs.ViewModels; 

public class LoginScreenViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string _username = string.Empty;
    private string _password = string.Empty;
    private LoginModel _loginModel; 

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    public string Password
    {
        get => _password;


        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    
    
    public ICommand LoginCommand { get; }

    public LoginScreenViewModel(LoginModel loginModel)
    {
        _loginModel = loginModel;
        LoginCommand = new RelayCommand(Login);
    }




    private void Login()
    {
        if (_loginModel.ValidateUser(Username, Password))
        {
            return; 
        }
    }
    
}
