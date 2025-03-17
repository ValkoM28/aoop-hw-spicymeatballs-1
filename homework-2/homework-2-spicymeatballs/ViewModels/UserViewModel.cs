using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using homework_2_spicymeatballs.AccountLogic;
namespace homework_2_spicymeatballs.ViewModels;

public class UserViewModel : ViewModelBase
{
    private const string StudentsFile = "student_users.json"; // Path updated
    private const string SubjectsFile = "subjects.json";

    public string Username { get; set; }
    public string FullName { get; set; }
    public ObservableCollection<string> EnrolledSubjects { get; set; } = new();

    public UserViewModel(string username)
    {
        LoadUserData(username);
    }

    private void LoadUserData(string username)
    {
        try
        {
            // Load students
            var studentsJson = File.ReadAllText(StudentsFile);
            var students = JsonSerializer.Deserialize<List<StudentAccount>>(studentsJson);

            // Find logged-in user
            var user = students?.FirstOrDefault(s => s.Username == username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Username = user.Username;
            FullName = $"{user.Name} {user.Surname}";

            // Load subjects
            var subjectsJson = File.ReadAllText(SubjectsFile);
            var subjects = JsonSerializer.Deserialize<List<Subject>>(subjectsJson);

            // Get enrolled subjects
            var enrolledSubjects = subjects?
                .Where(sub => user.EnrolledSubjects.Contains(sub.Id))
                .Select(sub => sub.Name)
                .ToList() ?? new List<string>();

            // Update UI
            EnrolledSubjects = new ObservableCollection<string>(enrolledSubjects);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }
}