namespace homework_2_spicymeatballs.AccountLogic;

public interface IAccount
{
    int Id { get; set; }
    string Username { get; set; }
    string DefinitelyNotPasswordHash { get; set; }
    string Name { get; set; }
    string Surname { get; set; }

    string ToString(); 

}