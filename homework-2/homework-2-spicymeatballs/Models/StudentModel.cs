using System;
using System.Collections.Generic;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class StudentModel
{
    public AccountManager AccountManager { get; set; }
    
    private readonly SubjectSaver _subjectSaver;
    private readonly SubjectLoader _subjectLoader;
    
    

    public StudentModel(AccountManager accountManager, SubjectLoader loader, SubjectSaver saver)
    {
        AccountManager = accountManager;
        _subjectSaver = saver;
        _subjectLoader = loader;
    }
    
    public List<Subject> ListAllSubjects()
    {
        return _subjectLoader.LoadSubjects(); 
    }

    public List<Subject> ListEnrolledSubjects()
    {
        return _subjectLoader.LoadSubjectByStudent((this.AccountManager.CurrentAccount as StudentAccount)!); 
    }
    
    public void AddSubject(int subjectId)
    {
        if (AccountManager.CurrentAccount is StudentAccount studentAccount)
        {
            studentAccount.EnrolledSubjects.Add(subjectId);
            AccountManager.UpdateStudentData(studentAccount);
        }
    }
    
    public void DropSubject(int subjectId)
    {
        if (AccountManager.CurrentAccount is StudentAccount studentAccount)
        {
            if (studentAccount.EnrolledSubjects.Contains(subjectId))
            {
                studentAccount.EnrolledSubjects.Remove(subjectId);
                AccountManager.UpdateStudentData(studentAccount);
                Console.WriteLine($"Dropped subject {subjectId} successfully.");
            }
            else
            {
                Console.WriteLine($"Subject {subjectId} is not in the enrolled list.");
            }
        }
        else
        {
            Console.WriteLine("Only students can drop subjects.");
        }

    }
    

}
    
    