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
    private StudentViewModel _studentViewViewModel;
    private TeacherViewModel _teacherViewViewModel;
    
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
            
            /*
             * Initial window setup.
             */
            desktop.MainWindow = _loginScreenView;
            
            _loginScreenViewModel.LoginSucceeded += () =>
            {
                
                _accountManager = new AccountManager(_loginModel.GetCurrentAccount(_loginScreenViewModel.Username), _loginModel.GetAllAccounts() );
                
                if (_accountManager.CurrentAccount.GetType() == typeof(StudentAccount))
                {
                    _studentModel = new StudentModel(_accountManager, new SubjectLoader(), new SubjectSaver());
                    _studentViewViewModel = new StudentViewModel(_studentModel);
                    _studentView = new StudentView { DataContext = _studentViewViewModel}; 
                    var test = desktop.MainWindow; 
                    desktop.MainWindow = _studentView;
                    desktop.MainWindow.Show();
                    test.Close();
                }
                
                else if (_loginModel.GetCurrentAccount(_loginScreenViewModel.Username).GetType() == typeof(TeacherAccount))
                {
                    _teacherModel = new TeacherModel((TeacherAccount) _loginModel.GetCurrentAccount(_loginScreenViewModel.Username), new SubjectLoader(), new SubjectSaver());
                    
                    _teacherViewViewModel = new TeacherViewModel(_teacherModel);
                    _teacherView = new TeacherView { DataContext = _teacherViewViewModel };

                    var test = desktop.MainWindow; 
                    desktop.MainWindow = _teacherView;
                    
                    desktop.MainWindow.Show();
                    test.Close();
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