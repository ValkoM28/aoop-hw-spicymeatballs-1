using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace homework_2_spicymeatballs.Views;
public partial class CreateSubjectPopupView : Window
{
    public event Action<string, string> OnSubjectCreated;

    public CreateSubjectPopupView()
    {
        InitializeComponent();
    }

    private void Confirm(object sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text;
        string description = DescriptionTextBox.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
        {
            return;
        }

        OnSubjectCreated?.Invoke(name, description);
        this.Close();
    }

    private void Deny(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}