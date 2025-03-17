using System.Collections.Generic;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class LoginModel
{
   private List<IAccount> _accounts;

   public List<IAccount> Accounts
   {
      get => _accounts;
      set { _accounts = value;  }
   }

   
   public LoginModel()
   {
      Accounts = AccountLoader.LoadAccounts();
   }
   
   

   public bool ValidateUser(string username, string password)
   {
      return Accounts.Exists(user =>
         user.Username == username && user.DefinitelyNotPasswordHash == Hasher.Hashed(password)); 
   }
}