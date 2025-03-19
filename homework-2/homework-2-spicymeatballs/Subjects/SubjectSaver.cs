using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace homework_2_spicymeatballs;

public class SubjectSaver
{
    private const string Path = "subjects.json";

    public SubjectSaver()
    {
        
    }
    
    public void SaveSubjects(List<Subject> subjects)
    {
        try
        {
            string json = JsonSerializer.Serialize(subjects, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path, json); 
            Console.WriteLine("Subjects saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving subjects: " + ex.Message);
        }
    }
}