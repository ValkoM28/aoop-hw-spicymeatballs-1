namespace homework_2_spicymeatballs.AccountLogic;

public interface IAccount
{
    int Id { get;  }
    string Username { get; set; }
    string DefinitelyNotPasswordHash { get; set; }
    string Name { get; set; }
    string Surname { get; set; }
    
}