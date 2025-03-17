using System;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class StudentModel
{
    public StudentAccount Account { get; set; }
    
    private readonly SubjectSaver _subjectSaver;
    
    

    public StudentModel(StudentAccount account, SubjectLoader loader, SubjectSaver saver)
    {
        Account = account;
        _subjectSaver = saver;
    }
    
    public void ListAllSubjects()
    {

    }

    public void ListEnrolledSubjects()
    {
        
    }

    public void ViewSubjectDetails()
    {
        
    }

    public void EnrollSubject()
    {
        
    }

    public void DropSubject()
    {
        
    }
    

}
    
    