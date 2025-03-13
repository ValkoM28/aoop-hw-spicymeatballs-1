using System.Collections.Generic;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class LoginModel
{
   private List<IAccount> _accounts; 
   
   public LoginModel()
   {
      _accounts = AccountLoader.LoadAccounts(); 
   }

   public bool ValidateUser(string username, string password)
   {
      return _accounts.Exists(user =>
         user.Username == username && user.DefinitelyNotPasswordHash == Hasher.Hashed(password)); 
   }
}