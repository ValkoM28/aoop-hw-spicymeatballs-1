using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using homework_2_spicymeatballs.AccountLogic;
using homework_2_spicymeatballs.Models;

namespace homework_2_spicymeatballs.ViewModels
{
    public class TeacherViewModel : ViewModelBase
    {
        private const string TeachersFile = "teacher_users.json"; 
        private const string SubjectsFile = "subjects.json";

        public string Username { get; set; }
        public string FullName { get; set; }
        public ObservableCollection<string> TeachingSubjects { get; set; } = new();

        public TeacherViewModel(string username)
        {
            LoadTeacherData(username);
        }

        private void LoadTeacherData(string username)
        {
            try
            {
                // Load teachers
                var teachersJson = File.ReadAllText(TeachersFile);
                var teachers = JsonSerializer.Deserialize<List<TeacherAccount>>(teachersJson);

                // Find logged-in teacher
                var teacher = teachers?.FirstOrDefault(t => t.Username == username);
                if (teacher == null)
                {
                    Console.WriteLine("Teacher not found.");
                    return;
                }

                Username = teacher.Username;
                FullName = $"{teacher.Name} {teacher.Surname}";

                // Load subjects
                var subjectsJson = File.ReadAllText(SubjectsFile);
                var subjects = JsonSerializer.Deserialize<List<Subject>>(subjectsJson);

                // Get teaching subjects
                var teachingSubjects = subjects?
                    .Where(sub => teacher.TeachingSubjects.Contains(sub.Id))
                    .Select(sub => sub.Name)
                    .ToList() ?? new List<string>();

                // Update UI
                TeachingSubjects = new ObservableCollection<string>(teachingSubjects);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading teacher data: {ex.Message}");
            }
        }
    }
}
