using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using homework_2_spicymeatballs.Models;
using Xunit;

namespace homework_2_spicymeatballs.ViewModels;

public partial class StudentViewModel : ViewModelBase
{
    private readonly StudentModel _studentModel;
    
    public string Username => "Username: " + _studentModel.Account.Username;
    public string Fullname => "Full name: " + _studentModel.Account.Name + " " + _studentModel.Account.Surname;
    
    public List<Subject> Subjects { get; set; }
    [ObservableProperty] 
    private ObservableCollection<Subject> _enrolledSubjects;
    [ObservableProperty]
    private ObservableCollection<Subject> _availableSubjects;
 
    public ICommand DropSubjectCommand { get; set; }
    public ICommand EnrollSubjectCommand { get; set; }
    
    [ObservableProperty]
    private Subject _selectedSubjectDrop;

    [ObservableProperty]
    private Subject _selectedSubjectEnroll;
    
    public StudentViewModel(StudentModel studentModel)
    {
        _studentModel = studentModel;
        
        Subjects = _studentModel.ListAllSubjects();
        
        EnrolledSubjects = new ObservableCollection<Subject>(_studentModel.ListEnrolledSubjects());
        AvailableSubjects = new ObservableCollection<Subject>(Subjects.Except(EnrolledSubjects, new SubjectComparer()));

        //EnrolledSubjects = _subjectLoader.LoadSubjectByStudent(_studentModel.Account);
        //AvailableSubjects = Subjects.Except(EnrolledSubjects, new SubjectComparer()).ToList();

        DropSubjectCommand = new RelayCommand(Drop);
        EnrollSubjectCommand = new RelayCommand(Enroll);

    }
    
    public void Enroll()
    {
        if (SelectedSubjectEnroll == null) return;
        
        _studentModel.Account.EnrolledSubjects.Add(SelectedSubjectEnroll.Id);
        RefreshSubjects();
    }
    
    public void Drop()
    {
        if (SelectedSubjectDrop == null) return;

        _studentModel.Account.EnrolledSubjects.Remove(SelectedSubjectDrop.Id);
        RefreshSubjects();
    }
    
    private void RefreshSubjects()
    {
        EnrolledSubjects.Clear();
        foreach (var subject in _studentModel.ListEnrolledSubjects())
        {
            EnrolledSubjects.Add(subject);
        }

        AvailableSubjects.Clear();
        foreach (var subject in _studentModel.ListAllSubjects().Except(EnrolledSubjects, new SubjectComparer()))
        {
            AvailableSubjects.Add(subject);
        }

        (DropSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
        (EnrollSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private class SubjectComparer : IEqualityComparer<Subject>
    {
        public bool Equals(Subject x, Subject y)
        {
            return x.Id == y.Id; 
        }
        
        public int GetHashCode(Subject obj)
        {
            return obj.Id.GetHashCode();
        } 
        
    }
}