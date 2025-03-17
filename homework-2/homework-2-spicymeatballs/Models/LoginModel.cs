using System;
using System.Collections.Generic;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class LoginModel
{
   private List<IAccount> _accounts;
   public List<IAccount> Accounts { get => _accounts; set => _accounts = value; }
   


   public LoginModel(AccountLoader accountLoader)
   {
      
      Accounts = accountLoader.LoadAccounts();

      PrintAccountsDebug();
   }
   
   

   public bool ValidateUser(string username, string password)
   {
      return Accounts.Exists(user =>
         user.Username == username && user.DefinitelyNotPasswordHash == Hasher.Hashed(password)); 
   }

   private void PrintAccountsDebug()
   {
      foreach (var account in Accounts)
      {
         Console.WriteLine(account.ToString());
      }
      Console.WriteLine();
   }
}