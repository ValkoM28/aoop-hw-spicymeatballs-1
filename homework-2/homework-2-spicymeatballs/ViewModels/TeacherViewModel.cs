using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using homework_2_spicymeatballs.AccountLogic;
using homework_2_spicymeatballs.Models;

namespace homework_2_spicymeatballs.ViewModels; 

public class TeacherViewModel : ViewModelBase
{
    private readonly TeacherModel _teacherModel;

    public string Username { get; set; }
    public string FullName { get; set; }

    public ObservableCollection<string> TeachingSubjects { get; set; } = new();

    public TeacherViewModel(TeacherModel model)
    {
        _teacherModel = model;
    }
}

