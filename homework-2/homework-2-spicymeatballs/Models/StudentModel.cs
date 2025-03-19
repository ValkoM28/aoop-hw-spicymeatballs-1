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

    public void EnrollSubject(int subjectId)
    {
        AccountManager.AddSubject(subjectId);
        
    }

    public void DropSubject(int subjectId)
    {
        AccountManager.DropSubject(subjectId);
        //_subjectSaver.SaveSubjects();
    }
    

}
    
    