namespace homework_2_spicymeatballs.ViewModels;
using System.Collections.ObjectModel;

public class UserViewModel : ViewModelBase
{
    public string Username { get; set; } = "john_doe";
    public string FullName { get; set; } = "John Doe";
    public ObservableCollection<string> EnrolledSubjects { get; set; }

    public UserViewModel()
    {
        EnrolledSubjects = new ObservableCollection<string>
        {
            "Mathematics",
            "Physics",
            "Computer Science",
            "History"
        };
    }
}
