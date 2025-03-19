using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using homework_2_spicymeatballs.AccountLogic;
using homework_2_spicymeatballs.Models;

namespace homework_2_spicymeatballs.ViewModels; 

public partial class TeacherViewModel : ViewModelBase
{
    private readonly TeacherModel _teacherModel;
    private readonly SubjectLoader _subjectLoader;

    public string Username => "Username: " + _teacherModel.Account.Username;
    public string Fullname => "Full name: " + _teacherModel.Account.Name + " " + _teacherModel.Account.Surname;
    
    [ObservableProperty]
    private ObservableCollection<Subject> _teachingSubjects;

    [ObservableProperty] private Subject _listBoxSelected; 

    
    public ICommand AddSubjectCommand { get; set; }
    public ICommand RemoveSubjectCommand { get; set; }
    public ICommand EditSubjectCommand { get; set; }
    

    public TeacherViewModel(TeacherModel model)
    {
        _teacherModel = model;
        TeachingSubjects = new ObservableCollection<Subject>(_teacherModel.ViewSubjects());
        
        AddSubjectCommand = new RelayCommand(Add);
        RemoveSubjectCommand = new RelayCommand(Remove);
        //EditSubjectCommand = new RelayCommand<Subject>(Edit);
    }
    
    private void Add()
    {
        //_teacherModel.CreateSubject(ListBoxSelected.Name, ListBoxSelected.Description);
    }

    private void Remove()
    {
        _teacherModel.DeleteSubject(ListBoxSelected.Id);
    }
    /*
    private void Edit(Subject subject, string newName, string newDescription)
    {
        _teacherModel.EditSubject(subject.Id, newName, newDescription);
    } */
}

