using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RestaurantSimulator.Models;

public class Recipe : INotifyPropertyChanged
{
    private bool _isInProgress;
    private bool _isCompleted;
    private bool _isPaused;
    private int _currentStepIndex;

    public string Name { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public List<string> Equipment { get; set; } = new();
    public List<RecipeStep> Steps { get; set; } = new();

    public bool IsInProgress
    {
        get => _isInProgress;
        set
        {
            if (_isInProgress != value)
            {
                _isInProgress = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (_isCompleted != value)
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPaused 
    {
        get => _isPaused;
        set 
        {
            if (_isPaused != value)
            {
                _isPaused = value;
                OnPropertyChanged();
            }
        }
    }


    public int CurrentStepIndex
    {
        get => _currentStepIndex;
        set
        {
            if (_currentStepIndex != value)
            {
                _currentStepIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProgressPercentage));
            }
        }
    }

    public double ProgressPercentage => Steps.Count > 0 ? (double)CurrentStepIndex / Steps.Count * 100 : 0;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Recipe Clone() { 
        return (Recipe)MemberwiseClone(); 
    }

}