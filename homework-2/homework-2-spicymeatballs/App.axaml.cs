using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using homework_2_spicymeatballs.AccountLogic;
using homework_2_spicymeatballs.Models;
using homework_2_spicymeatballs.ViewModels;
using homework_2_spicymeatballs.Views;

namespace homework_2_spicymeatballs;

public partial class App : Application
{
    //models
    private LoginModel _loginModel;
    
    
    //viewmodels
    private LoginScreenViewModel _loginScreenViewModel;
    private ViewModelBase _studentViewViewModel;
    private ViewModelBase _teacherViewViewModel;
    
    //views
    private LoginScreenView _loginScreenView;
    private IViewPlaceholder _studentView;
    private IViewPlaceholder _teacherView;
    
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            //Login screen setup
            _loginModel = new LoginModel(new AccountLoader());
            _loginScreenViewModel = new LoginScreenViewModel(_loginModel); 
            _loginScreenView = new LoginScreenView { DataContext = new LoginScreenViewModel(_loginModel) };
            
            //Student view setup
            
            //_studentModel = ...
            //_studentViewViewModel = ...
            //_studentView = ...
            
            //Teacher view setup
            
            //_teacherModel = ...
            //_teacherViewViewModel = ...
            //_teacherView = ...
            
            
            
            /*
             * Initial window setup.
             */
            desktop.MainWindow = _loginScreenView;
            
            _loginScreenViewModel.LoginSucceeded += () =>
            {
                if (_loginModel.GetAccount(_loginScreenViewModel.Username).GetType() == typeof(StudentAccount))
                {
                    //desktop.MainWindow = _studentView;
                }
                else
                {
                    //desktop.MainWindow = _teacherView;
                }
            };

        }

        base.OnFrameworkInitializationCompleted();
    }   

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    
}