using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace homework_2_spicymeatballs.AccountLogic;

public static class AccountLoader
{
    static string _teacherAccountsPath = "teacher_users.json";
    static string _studentAccountsPath = "student_users.json";
    
        
    public static List<IAccount> LoadAccounts()
    {
        List<IAccount> accounts = new List<IAccount>();
        accounts.AddRange(LoadTeacherAccounts());
        accounts.AddRange(LoadStudentAccounts());
        return accounts;
    }

    private static List<StudentAccount> LoadStudentAccounts()
    {
        List<StudentAccount> studentAccounts = new List<StudentAccount>();
        if (File.Exists(_studentAccountsPath))
        {
            string json = File.ReadAllText(_studentAccountsPath);
            studentAccounts = JsonSerializer.Deserialize<List<StudentAccount>>(json);
        }
        return studentAccounts; 
    }
    
    private static List<TeacherAccount> LoadTeacherAccounts()
    {
        List<TeacherAccount> teacherAccounts = new List<TeacherAccount>();
        if (File.Exists(_teacherAccountsPath))
        {
            string json = File.ReadAllText(_teacherAccountsPath);
            teacherAccounts = JsonSerializer.Deserialize<List<TeacherAccount>>(json);
        }
        return teacherAccounts; 
    }


}