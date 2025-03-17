using System;
using System.Collections.Generic;

namespace homework_2_spicymeatballs.AccountLogic;

public class AccountManager
{
    public List<IAccount> Accounts { get; set; }
    
    public IAccount CurrentAccount { get; set; }
    
    public AccountManager(List<IAccount> accounts, IAccount currentAccount)
    {
        Accounts = accounts;
        CurrentAccount = currentAccount;
    }


    public void UpdateAccountData()
    {
        Accounts.Remove(CurrentAccount);
        foreach (var account in Accounts)
        {
            Console.WriteLine(account.ToString());
        }
    }
    
    
    
    
    
}