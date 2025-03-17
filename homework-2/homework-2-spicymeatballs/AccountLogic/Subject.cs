using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace homework_2_spicymeatballs.AccountLogic;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TeacherId { get; set; }
    public List<int> StudentsEnrolledId { get; set; }
}