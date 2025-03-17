using System.Collections.Generic;
using homework_2_spicymeatballs.Models;

namespace homework_2_spicymeatballs.ViewModels;

public class StudentViewModel : ViewModelBase
{
    private readonly StudentModel _studentModel;
    
    public string Username => "Username: " + _studentModel.Account.Username;
    public string Fullname => "Full name: " + _studentModel.Account.Name + " " + _studentModel.Account.Surname;
    private readonly SubjectLoader _subjectLoader;
    
    public List<Subject> Subjects { get; init; }
    public List<Subject> EnrolledSubjects { get; init; }
 
    
    
    public StudentViewModel(StudentModel studentModel, SubjectLoader loader)
    {
        _studentModel = studentModel;
        _subjectLoader = loader;
        
        Subjects = _subjectLoader.LoadSubjects();
        EnrolledSubjects = _subjectLoader.LoadSubjectByStudent(_studentModel.Account);
    }
    
    
}