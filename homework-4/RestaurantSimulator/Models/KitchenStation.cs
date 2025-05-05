using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RestaurantSimulator.Models;

public class KitchenStation : INotifyPropertyChanged
{
    private Recipe? _currentRecipe;
    private bool _isActive;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Recipe? CurrentRecipe
    {
        get => _currentRecipe;
        set
        {
            if (_currentRecipe != value)
            {
                _currentRecipe = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAvailable));
            }
        }
    }
    
    public bool IsAvailable => CurrentRecipe == null;
    
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 