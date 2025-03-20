using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace homework_2_spicymeatballs;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TeacherId { get; set; }
    
    [JsonConstructor]
    public Subject(int id, string name, string description, int teacherid)
    {
        Id = id;
        Name = name;
        Description = description;
        TeacherId = teacherid;
    }


    public override string ToString()
    {
        return Id+" "+Name+" "+Description+" "+TeacherId;
    }
}