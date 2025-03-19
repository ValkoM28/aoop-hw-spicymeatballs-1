using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using homework_2_spicymeatballs.Models;
using homework_2_spicymeatballs.Views;

namespace homework_2_spicymeatballs.ViewModels;

public partial class StudentViewModel : ViewModelBase
{
    private readonly StudentModel _studentModel;
    
    public string Username => "Username: " + _studentModel.Account.Username;
    public string Fullname => "Full name: " + _studentModel.Account.Name + " " + _studentModel.Account.Surname;
    private readonly SubjectLoader _subjectLoader;
    
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
    
    public StudentViewModel(StudentModel studentModel, SubjectLoader loader)
    {
        _studentModel = studentModel;
        _subjectLoader = loader;
        
        Subjects = _subjectLoader.LoadSubjects();
        
        EnrolledSubjects = new ObservableCollection<Subject>(_subjectLoader.LoadSubjectByStudent(_studentModel.Account));
        AvailableSubjects = new ObservableCollection<Subject>(Subjects.Except(EnrolledSubjects, new SubjectComparer()));

        //EnrolledSubjects = _subjectLoader.LoadSubjectByStudent(_studentModel.Account);
        //AvailableSubjects = Subjects.Except(EnrolledSubjects, new SubjectComparer()).ToList();

        DropSubjectCommand = new AsyncRelayCommand(Drop);
        EnrollSubjectCommand = new AsyncRelayCommand(Enroll);

    }
    
    /*
    public void Enroll()
    {
        if (SelectedSubjectEnroll == null) return;
        
        ShowPopup("Do you want to enroll in subject ?");
        if (GetPopupResult() == true){
            _studentModel.Account.EnrolledSubjects.Add(SelectedSubjectEnroll.Id);
            RefreshSubjects();
        }

    }
    
    public void Drop()
    {
        if (SelectedSubjectDrop == null) return;

        ShowPopup("Do you want to drop subject ?");
        if (GetPopupResult() == true){
            _studentModel.Account.EnrolledSubjects.Remove(SelectedSubjectDrop.Id);
            RefreshSubjects();
        }
    }
    */
    private void RefreshSubjects()
    {
        EnrolledSubjects.Clear();
        foreach (var subject in _subjectLoader.LoadSubjectByStudent(_studentModel.Account))
        {
            EnrolledSubjects.Add(subject);
        }

        AvailableSubjects.Clear();
        foreach (var subject in _subjectLoader.LoadSubjects().Except(EnrolledSubjects, new SubjectComparer()))
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

    private bool popupResult;
    private void ShowPopup(string customMessage)
    {
        var welcomeView = new PopupView(customMessage);

        // Subscribe to the event to handle popup closure
        welcomeView.OnPopupClosed += HandlePopupClosed;

        // Show the popup as a normal window
        welcomeView.Show();
    }


    // FOR LOGGING ONLY BASICALLY LEAVE IT HERE
    private void HandlePopupClosed(bool result)
    {
        popupResult = result;
        Console.WriteLine(popupResult);
        if (result == false)
        {
            Console.WriteLine("Popup was closed with a result of 'false'.");
        }
        else
        {
            Console.WriteLine("Popup was closed with another result.");
        }
    }

    public bool GetPopupResult()
    {
        Console.WriteLine("GetPopupResult called");
        return popupResult;
    }

    private async Task<bool> ShowPopupAsync(string customMessage)
    {
        var tcs = new TaskCompletionSource<bool>();
        var popupView = new PopupView(customMessage);

        // Subscribe to the event to handle popup closure
        popupView.OnPopupClosed += result =>
        {
            tcs.SetResult(result);
            popupView.Close();
        };

        // Show the popup as a normal window
        popupView.Show();

        // Wait for the popup to close and return the result
        return await tcs.Task;
    }

    public async Task Enroll()
    {
        if (SelectedSubjectEnroll == null) return;

        var result = await ShowPopupAsync("Do you want to enroll in subject?");
        if (result)
        {
            _studentModel.Account.EnrolledSubjects.Add(SelectedSubjectEnroll.Id);
            RefreshSubjects();
        }
    }

    public async Task Drop()
    {
        if (SelectedSubjectDrop == null) return;

        var result = await ShowPopupAsync("Do you want to drop subject?");
        if (result)
        {
            _studentModel.Account.EnrolledSubjects.Remove(SelectedSubjectDrop.Id);
            RefreshSubjects();
        }
    }
}