using System;
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
    private StudentModel _studentModel; 
    private TeacherModel _teacherModel;
    
    
    //viewmodels
    private LoginScreenViewModel _loginScreenViewModel;
    private ViewModelBase _studentViewViewModel;
    private ViewModelBase _teacherViewViewModel;
    
    //views
    private LoginScreenView _loginScreenView;
    private StudentView _studentView;
    private TeacherView _teacherView;
    
    //services
    private AccountManager _accountManager; 
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
            _loginScreenView = new LoginScreenView { DataContext = _loginScreenViewModel};
            
            //Student view setup
            
            
            
            //Teacher view setup

            
            
            
            /*
             * Initial window setup.
             */
            desktop.MainWindow = _loginScreenView;
            
            _loginScreenViewModel.LoginSucceeded += () =>
            {
                _accountManager = new AccountManager(_loginModel.GetAllAccounts(), _loginModel.GetCurrentAccount(_loginScreenViewModel.Username));
                
                if (_loginModel.GetCurrentAccount(_loginScreenViewModel.Username).GetType() == typeof(StudentAccount))
                {
                    Console.WriteLine("here0");
                    _studentModel = new StudentModel((StudentAccount) _loginModel.GetCurrentAccount(_loginScreenViewModel.Username), new SubjectLoader(), new SubjectSaver());
                    Console.WriteLine("here1");

                    _studentViewViewModel = new StudentViewModel(_studentModel, new SubjectLoader());
                    Console.WriteLine("here2");

                    _studentView = new StudentView { DataContext = _studentViewViewModel}; 
                    Console.WriteLine("here3");
                    
                    desktop.MainWindow = _studentView;
                    desktop.MainWindow.Show();
                    Console.WriteLine("here4");
                }
                else if (_loginModel.GetCurrentAccount(_loginScreenViewModel.Username).GetType() == typeof(TeacherAccount))
                {
                    _teacherModel = new TeacherModel((TeacherAccount) _loginModel.GetCurrentAccount(_loginScreenViewModel.Username));
                    _teacherViewViewModel = new TeacherViewModel(_teacherModel);
                    _teacherView = new TeacherView { DataContext = _teacherViewViewModel }; 
                    
                    desktop.MainWindow = _teacherView;
                }
                else
                {
                    Console.WriteLine("Account type not recognized.");
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