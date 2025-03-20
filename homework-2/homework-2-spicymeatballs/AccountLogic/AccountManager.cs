using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace homework_2_spicymeatballs.AccountLogic;

public class AccountManager
{
    private const string StudentDataPath = "student_users.json";
    private const string TeacherDataPath = "teacher_users.json";

    public List<StudentAccount> StudentAccounts { get; set; } = new(); 
    public List<TeacherAccount> TeacherAccounts { get; set; } = new(); 
    
    public IAccount CurrentAccount { get; set; }
    
    public AccountManager(IAccount currentAccount, List<IAccount> accounts)
    {
        foreach (var account in accounts)
        {
            if (account is StudentAccount studentAccount)
            {
                StudentAccounts.Add(studentAccount);
            }
            else if (account is TeacherAccount teacherAccount)
            {
                TeacherAccounts.Add(teacherAccount);
            } 
        }
        CurrentAccount = currentAccount;
    }
    

    // ✅ Updates student data and saves back to JSON
    public void UpdateStudentData(StudentAccount updatedAccount)
    {
        var existingStudent = StudentAccounts.Find(s => s.Id == updatedAccount.Id);
        if (existingStudent != null)
        {
            StudentAccounts.Remove(existingStudent);
        }
        StudentAccounts.Add(updatedAccount);
        UpdateAccountData(StudentAccounts, StudentDataPath);
    }

    // ✅ Updates teacher data and saves back to JSON
    public void UpdateTeacherData(TeacherAccount updatedAccount)
    {
        var existingTeacher = TeacherAccounts.Find(t => t.Id == updatedAccount.Id);
        if (existingTeacher != null)
        {
            TeacherAccounts.Remove(existingTeacher);
        }
        TeacherAccounts.Add(updatedAccount);
        UpdateAccountData(TeacherAccounts, TeacherDataPath);
    }

    // ✅ Saves updated account lists back to the respective JSON file
    private void UpdateAccountData<T>(List<T> accounts, string filePath)
    {
        try
        {
            string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Updated {filePath} successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating {filePath}: {ex.Message}");
        }
    }



}