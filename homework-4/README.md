# Restaurant Simulator

A restaurant simulation application that visualizes meal preparation processes using multithreading and Avalonia UI.

## Project Overview

This application simulates a restaurant kitchen where multiple recipes are prepared concurrently. It features:
- Real-time visualization of meal preparation
- Concurrent processing of multiple recipes
- Animated progress tracking
- Interactive UI controls

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or JetBrains Rider (recommended)
- Avalonia UI extension for your IDE

## Project Structure

```
RestaurantSimulator/
├── Models/
│   ├── Ingredient.cs
│   ├── Recipe.cs
│   ├── RecipeStep.cs
│   └── KitchenStation.cs
├── ViewModels/
│   ├── MainWindowViewModel.cs
│   ├── RecipeViewModel.cs
│   └── KitchenStationViewModel.cs
├── Views/
│   ├── MainWindow.axaml
│   ├── RecipeView.axaml
│   └── KitchenStationView.axaml
├── Services/
│   ├── JsonDataService.cs
│   ├── SimulationService.cs
│   └── AnimationService.cs
└── Utilities/
    ├── Constants.cs
    └── Extensions.cs
```

## Setup Instructions

1. Clone the repository
2. Open the solution in your IDE
3. Restore NuGet packages
4. Build and run the application

## Key Features

### Data Management
- JSON file parsing and validation
- Thread-safe data structures
- Real-time data updates

### Simulation
- Concurrent recipe processing
- Multiple kitchen stations
- Progress tracking and timing

### UI Features
- Real-time progress visualization
- Animated progress bars
- Interactive controls
- Responsive layout

## Implementation Details

### Data Models
- `Ingredient`: Represents a single ingredient with quantity and unit
- `Recipe`: Contains recipe details, steps, and required equipment
- `RecipeStep`: Represents a single step in a recipe with duration
- `KitchenStation`: Manages a single cooking station's state

### ViewModels
- `MainWindowViewModel`: Main application logic and state management
- `RecipeViewModel`: Handles individual recipe processing
- `KitchenStationViewModel`: Manages kitchen station operations

### Services
- `JsonDataService`: Handles JSON file parsing and validation
- `SimulationService`: Manages the simulation logic
- `AnimationService`: Handles UI animations

## Extensions

### Role-Based Features
- Multiple chef types with different speeds
- Head chef with priority processing
- Waiter role for order management

### Priority Orders
- VIP customer handling
- Order prioritization system

### Status Reporting
- Order history tracking
- Performance metrics
- Completion statistics

## Development Guidelines

1. Follow MVVM pattern strictly
2. Use async/await for all asynchronous operations
3. Implement proper error handling
4. Maintain thread safety
5. Keep UI responsive
6. Document code with XML comments

## Testing

- Unit tests for data parsing
- Integration tests for simulation logic
- UI tests for responsiveness
- Performance testing for concurrent operations

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License. 