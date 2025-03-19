using System;
using System.Collections.Generic;
using System.Linq;
using homework_2_spicymeatballs.AccountLogic;

namespace homework_2_spicymeatballs.Models;

public class TeacherModel
{
    public TeacherAccount Account { get; set; } 
    private readonly SubjectLoader _subjectLoader;
    private readonly SubjectSaver _subjectSaver;
    public TeacherModel(TeacherAccount account, SubjectLoader loader, SubjectSaver saver)
    {
        Account = account;
        _subjectLoader = loader;
        _subjectSaver = saver; 
    }
    public List<Subject> ViewSubjects()
    {
        return _subjectLoader.LoadSubjectsByTeacher(Account); 
    }

    public void CreateSubject(string name, string description)
    {
        var temp = _subjectLoader.LoadSubjects();
        int id = temp[^1].Id + 1;
        
        var subject = new Subject(id, name, description, Account.Id);
        temp.Add(subject); 
        _subjectSaver.SaveSubjects(temp);
    }
    
    public void EditSubject(int id, string name, string description)
    {
        var temp = _subjectLoader.GetSubjectById(id);
        temp.Name = name; 
        temp.Description = description;
        var subjects = _subjectLoader.LoadSubjects();
        subjects[id] = temp;
        _subjectSaver.SaveSubjects(subjects);
    }
    
    public void DeleteSubject(int id)
    {
        var subjects = _subjectLoader.LoadSubjects();
        var temp = subjects.FirstOrDefault(s => s.Id == id);
        
        if (temp == null)
        {
            Console.WriteLine("Subject not found");
            return;
        }
        
        subjects.Remove(temp);
        
        Console.WriteLine("Deleted subject: " + temp.Name);
        foreach (var subject in subjects)
        {
            Console.WriteLine(subject.Name);
        }
        _subjectSaver.SaveSubjects(subjects);
    }
    
    
}