using Avalonia.Controls;
using System;

namespace homework_2_spicymeatballs.Views;

public partial class LoginScreenView : Window
{
    public LoginScreenView()
    {
        InitializeComponent();
        ShowPopup();
    }

    private void ShowPopup()
    {
        string customMessage = "Welcome to the Popup Screen!";  // Your custom message
        var welcomeView = new PopupView(customMessage);

        // Subscribe to the event to handle popup closure
        welcomeView.OnPopupClosed += HandlePopupClosed;

        // Show the popup as a normal window
        welcomeView.Show();
    }

    private void HandlePopupClosed(bool result)
    {
        if (result == false)
        {
            // Handle the case where the result is false
            Console.WriteLine("Popup was closed with a result of 'false'.");
        }
        else
        {
            // Handle other results if necessary
            Console.WriteLine("Popup was closed with another result.");
        }
    }
}