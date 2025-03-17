using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace homework_2_spicymeatballs.AccountLogic;

public class AccountLoader
{
    private const string TeacherAccountsPath = "teacher_users.json";
    private const string StudentAccountsPath = "student_users.json";


    public List<IAccount> LoadAccounts()
    {
        List<IAccount> accounts = new List<IAccount>();
        accounts.AddRange(LoadTeacherAccounts());
        accounts.AddRange(LoadStudentAccounts());
        return accounts;
    }

    private List<StudentAccount> LoadStudentAccounts()
    {
        List<StudentAccount> studentAccounts = [];
        if (File.Exists(StudentAccountsPath))
        {
            string json = File.ReadAllText(StudentAccountsPath);
            studentAccounts = JsonSerializer.Deserialize<List<StudentAccount>>(json);
        }
        return studentAccounts; 
    }
    
    private List<TeacherAccount> LoadTeacherAccounts()
    {
        List<TeacherAccount> teacherAccounts = [];
        if (File.Exists(TeacherAccountsPath))
        {
            string json = File.ReadAllText(TeacherAccountsPath);
            teacherAccounts = JsonSerializer.Deserialize<List<TeacherAccount>>(json);
        }
        return teacherAccounts; 
    }


}