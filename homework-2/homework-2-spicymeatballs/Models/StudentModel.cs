using System;
using System.Collections.Generic;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class StudentModel
{
    public StudentAccount Account { get; set; }
    
    private readonly SubjectSaver _subjectSaver;
    private readonly SubjectLoader _subjectLoader;
    
    

    public StudentModel(StudentAccount account, SubjectLoader loader, SubjectSaver saver)
    {
        Account = account;
        _subjectSaver = saver;
        _subjectLoader = loader;
    }
    
    public List<Subject> ListAllSubjects()
    {
        return _subjectLoader.LoadSubjects(); 
    }

    public List<Subject> ListEnrolledSubjects()
    {
        return _subjectLoader.LoadSubjectByStudent(this.Account); 
    }

    public void EnrollSubject(int subjectId)
    {
        Account.EnrolledSubjects.Add(subjectId);
        //_subjectSaver.SaveSubjects();
    }

    public void DropSubject(int subjectId)
    {
        Account.EnrolledSubjects.Remove(subjectId);
        //_subjectSaver.SaveSubjects();
    }
    

}
    
    