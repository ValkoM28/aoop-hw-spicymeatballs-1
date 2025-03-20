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
using homework_2_spicymeatballs.Views;

namespace homework_2_spicymeatballs.ViewModels; 

public partial class TeacherViewModel : ViewModelBase
{
    private readonly TeacherModel _teacherModel;
    private readonly SubjectLoader _subjectLoader;

    public string Username => "Username: " + _teacherModel.AccountManager.CurrentAccount.Username;
    public string Fullname => "Full name: " + _teacherModel.AccountManager.CurrentAccount.Name + " " + _teacherModel.AccountManager.CurrentAccount.Surname;
    
    [ObservableProperty]
    private ObservableCollection<Subject> _teachingSubjects;

    [ObservableProperty] 
    private Subject? _listBoxSelected; 

    
    public ICommand AddSubjectCommand { get; set; }
    public ICommand RemoveSubjectCommand { get; set; }
    public ICommand EditSubjectCommand { get; set; }
    

    public TeacherViewModel(TeacherModel model)
    {
        _teacherModel = model;
        TeachingSubjects = new ObservableCollection<Subject>(_teacherModel.ViewSubjects());
        
        AddSubjectCommand = new RelayCommand(OpenSubjectPopup);
        RemoveSubjectCommand = new RelayCommand(Remove);
        EditSubjectCommand = new RelayCommand(OpenEditPopup);
    }
    
    
    public void OpenSubjectPopup()
    {
        var popup = new CreateSubjectPopupView();
        popup.OnSubjectCreated += AddSubject;
        popup.Show();
    }

    public void OpenEditPopup()
    {
        if (ListBoxSelected == null)
        {
            return;
        }
        var editPopup = new EditSubjectPopupView(ListBoxSelected.Name, ListBoxSelected.Description);
        editPopup.OnSubjectEdited += (newName, newDescription) =>
        {
            Edit(ListBoxSelected.Id, newName, newDescription);
            ListBoxSelected.Name = newName;
            ListBoxSelected.Description = newDescription;
        };
        editPopup.Show();
    }

    
    private void AddSubject(string name, string description)
    {
        _teacherModel.CreateSubject(name, description);
        TeachingSubjects = new ObservableCollection<Subject>(_teacherModel.ViewSubjects());
        ShowPopup("Subject created");
        
    }

    private void Remove()
    {
        if (ListBoxSelected == null) return; 
        
        _teacherModel.DeleteSubject(ListBoxSelected.Id);
        TeachingSubjects = new ObservableCollection<Subject>(_teacherModel.ViewSubjects());
        ShowPopup("Subject deleted");
    }
    
    private void Edit(int subjectId, string newName, string newDescription)
    {
        _teacherModel.EditSubject(subjectId, newName, newDescription);
        TeachingSubjects = new ObservableCollection<Subject>(_teacherModel.ViewSubjects());
        ShowPopup("Subject edited");

    } 
    
    private void ShowPopup(string customMessage)
    {
        var welcomeView = new PopupView(customMessage, interactive: false);

        // Show the popup as a normal window
        welcomeView.Show();
    }


}

