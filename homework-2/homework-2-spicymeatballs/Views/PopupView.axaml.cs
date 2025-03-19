using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace homework_2_spicymeatballs.Views;
public partial class PopupView : Window
{
    public event Action<bool> OnPopupClosed;
    public string CustomMessage
        {
            get { return MessageTextBlock.Text; }
            set { MessageTextBlock.Text = value; }
        }

    public PopupView(string message)
    {
        InitializeComponent();
        CustomMessage = message;  // Set the custom message
    }

    private void Confirm(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnPopupClosed?.Invoke(true);
        this.Close();
    }

    private void Deny(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnPopupClosed?.Invoke(false);
        this.Close();
    }
}