using System.Collections.Generic;

namespace homework_2_spicymeatballs.AccountLogic;

public class TeacherAccount : IAccount
{
    public int Id { get; }
    public string Username { get; set; }
    public string DefinitelyNotPasswordHash { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<int> TeachingSubjects { get; set; }
    
    public override string ToString()
    {
        return Id+" "+Username+" "+ DefinitelyNotPasswordHash+" "+Name+" "+Surname;
    }
}