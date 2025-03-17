using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace homework_2_spicymeatballs.AccountLogic;

public class StudentAccount : IAccount
{
    public int Id { get; set;  }
    public string Username { get; set; }
    public string DefinitelyNotPasswordHash { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<int> EnrolledSubjects { get; set; }

    public override string ToString()
    {
        return Id+" "+Username+" "+ DefinitelyNotPasswordHash+" "+Name+" "+Surname+ " " + string.Join(" ", EnrolledSubjects);
    }
}