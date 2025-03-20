using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace homework_2_spicymeatballs.Views;
public partial class EditSubjectPopupView : Window
{
    public event Action<string, string> OnSubjectEdited; // Notify TeacherViewModel

    public EditSubjectPopupView(string currentName, string currentDescription)
    {
        InitializeComponent();
        NameTextBox.Text = currentName;
        DescriptionTextBox.Text = currentDescription;
    }

    private void Confirm(object sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text;
        string description = DescriptionTextBox.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
        {
            return; // Optionally, show an error message
        }

        try
        {
            OnSubjectEdited?.Invoke(name, description);
        }
        catch (Exception exception)
        {
            Console.WriteLine("yay :D ");
        }
        finally
        {
            this.Close();
        }

    }

    private void Deny(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
