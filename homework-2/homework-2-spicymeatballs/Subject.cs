using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace homework_2_spicymeatballs;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Teacher { get; set; }
    public List<string> Students { get; set; }
    
    [JsonConstructor]
    public Subject(int id, string name, string description, string teacher, List<string> students)
    {
        Id = id;
        Name = name;
        Description = description;
        Teacher = teacher;
        Students = students;
    }
}