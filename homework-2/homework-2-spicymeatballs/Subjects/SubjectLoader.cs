using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs;

public class SubjectLoader
{
    private const string SubjectsPath = "subjects.json";
    
    public List<Subject> LoadSubjects()
    {
        List<Subject> subjects = [];
        if (File.Exists(SubjectsPath))
        {
            string json = File.ReadAllText(SubjectsPath);
            subjects = JsonSerializer.Deserialize<List<Subject>>(json);
        }
        return subjects; 
    }

    public List<Subject> LoadSubjectByStudent(StudentAccount student)
    {
        var subjects = LoadSubjects();
        return subjects.FindAll(subject => student.EnrolledSubjects.Contains(subject.Id));
    }
}