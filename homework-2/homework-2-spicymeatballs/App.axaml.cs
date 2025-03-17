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
    private LoginModel _loginModel;
    
    
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
            _loginModel = new LoginModel(new AccountLoader()); 
            

        desktop.MainWindow = new LoginScreenView
            {
                DataContext = new LoginScreenViewModel(_loginModel),
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