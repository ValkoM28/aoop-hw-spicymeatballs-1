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
        string json = JsonSerializer.Serialize(subjects);
        File.WriteAllText(Path, json);
    }
}