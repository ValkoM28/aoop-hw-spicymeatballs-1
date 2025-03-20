# Assignment 2 documentation
## User Accounts with passwords

### Teachers
mrbean@teacher.uni.edu: bestteacher 
ironman@teacher.uni.edu: securepassword
bobbyfischer@teacher.uni.edu: 123456
test: test

### Students

gamerstudent@student.uni.edu: meow
startupstudent@student.uni.edu: password321
layzstudent@student.uni.edu: password123
help: me

## Scenario 


You have been hired by a university to develop a University Management App that helps in academic administration. The university is experiencing difficulties in managing their courses and they need a simple but effective solution. The application should allow students to enroll in and drop subjects and teachers to create and manage subjects, while also providing data persistence through a JSON file. You will also have to conduct testing of the code. Below you can find system requirements and instructions you should follow.


## Requirements

Non-Functional

    The application must be developed using C#, Avalonia and following the MVVM architecture.
    The application must implement Student and Teacher roles with access control (each can only access their functionality).
    Ensure simple and intuitive user interface.

Functional 

System:

    An entry point is a login screen that lets the user login as a certain role with a simple validation logic (comparing provided login and password of student/teacher to valid ones and picking the right account).
    The application should provide system's data persistence using a JSON file. The JSON file should be loaded when starting the application and saved on closing the application or using a save and load button.

Student:

    View available subjects.
    View enrolled subjects.
    View subject's details (Name, teacher, description etc).
    Enroll in available subjects.
    Drop from enrolled subjects.

Teacher:

    View subjects they teach.
    Create a new subjects.
    Edit subject's details.
    Delete their subject.

## Use Cases 

Use Cases

I. Student Enrollment in a Subject

    Actors: Student
    Preconditions: You are logged in as a Student.

Steps:

    The student navigates to the "Available Subjects" page.
    The student selects a subject and clicks "Enroll".
    The system updates the student's list of enrolled subjects in "Enrolled Subjects" page.
    The student receives confirmation of successful enrollment in a form of text or pop-up.

 

II. Student Dropping a Subject

    Actors: Student
    Preconditions: You are logged in as a Student and enrolled in at least one subject.

Steps:

    The student navigates to the "Enrolled Subjects" page.
    The student selects a subject and clicks "Drop".
    The system removes the subject from the student's enrolled list.
    The student receives confirmation of successful removal in a form of text or pop-up.
    The dropped subject appears in "Available Subjects" page. 

 

III. Teacher Creating a Subject

    Actors: Teacher
    Preconditions: You are logged in as a Teacher.

Steps:

    The teacher navigates to "My Subjects" and selects "Create new subject".
    The teacher enters subject details (name, description, etc.).
    The teacher clicks "Save".
    The system adds the subject to "My Subjects" for a teacher and "Available Subjects" for students.
    The teacher receives confirmation of successful creation in a form of text or pop-up.

 

IV. Teacher Deleting a Subject

Actors: Teacher

Preconditions: You are logged in as a Teacher.

Steps:

    The teacher navigates to "My Subjects".
    The teacher selects a subject and clicks "Delete".
    The system removes the subject from "My Subjects" for a teacher and "Available Subjects" for students.
    The teacher receives confirmation of successful deletion in a form of text or pop-up.


## Testing

### Unit tests 

`Loading accounts from the jsons test, status Passed`
```csharp
//this test will check if the data is loaded correctly, works only if you actually make the mock records the same as jsons, after they are updated in the app, they have to be updated here for test to work
    [Fact]
    public void LoadUsers_UsersLoadedCorrectly_ReturnsListOfAllUsers()
    {
        

        var mockRecords = new List<IAccount>()
        {
            new TeacherAccount()
            {
                Id = 0,
                Username = "mrbean@teacher.uni.edu",
                DefinitelyNotPasswordHash = "87ab76f7403c8c4510165b6da357e95a16df8a10443a1bc12edf83747790b911",
                Name = "Mr.",
                Surname = "Bean",
            }, 
            
            new TeacherAccount()
            {
                Id = 1,
                Username = "ironman@teacher.uni.edu",
                DefinitelyNotPasswordHash = "e0e6097a6f8af07daf5fc7244336ba37133713a8fc7345c36d667dfa513fabaa",
                Name = "iron",
                Surname = "man",
            }, 
            new TeacherAccount()
            {
                Id = 2,
                Username = "bobbyfischer@teacher.uni.edu",
                DefinitelyNotPasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                Name = "Bobby",
                Surname = "Fischer",
            }, 
            new TeacherAccount()
            {
                Id = 3, 
                Username = "test",
                DefinitelyNotPasswordHash = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08", 
                Name = "Bobby",
                Surname = "Fischer"
            },
            new StudentAccount()
            {
                Id = 0,
                Username = "gamerstudent@student.uni.edu",
                DefinitelyNotPasswordHash = "404cdd7bc109c432f8cc2443b45bcfe95980f5107215c645236e577929ac3e52",
                Name = "Gamer",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {0, 1, 2}
            }, 
            new StudentAccount()
            {
                Id = 1,
                Username = "startupstudent@student.uni.edu",
                DefinitelyNotPasswordHash = "a20aff106fe011d5dd696e3b7105200ff74331eeb8e865bb80ebd82b12665a07",
                Name = "Startup",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {0, 1, 2}
            }, 
            
            new StudentAccount()
            {
                Id = 2,
                Username = "layzstudent@student.uni.edu",
                DefinitelyNotPasswordHash = "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f",
                Name = "Lazy",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {0, 2}
            }, 
            new StudentAccount()
            {
                Id = 3,
                Username = "help",
                DefinitelyNotPasswordHash = "2744ccd10c7533bd736ad890f9dd5cab2adb27b07d500b9493f29cdc420cb2e0",
                Name = "Lazy",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {1}
            }, 
        };
        
        
        var loginModel = new LoginModel(new AccountLoader());
        var actualRecords = loginModel.Accounts;
        
        // Assert - Check if both lists have the same length
        Assert.Equal(mockRecords.Count, actualRecords.Count);
        
        Assert.True(mockRecords.SequenceEqual(actualRecords, new AccountComparer()));
        

    }

```

```csharp
//unit test 2 here
    [Fact]
    public void ValidateUser_ValidCredentials_ReturnsTrue()
    {
        // Arrange: Create a fake account list
        var mockAccount = new Mock<IAccount>();
        mockAccount.Setup(a => a.Username).Returns("gamerstudent@student.uni.edu");
        mockAccount.Setup(a => a.DefinitelyNotPasswordHash).Returns(Hasher.Hashed("templatepasswordhash"));

        var loginModel = new LoginModel(new AccountLoader());
        var accountsField = typeof(LoginModel).GetField("_accounts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        accountsField.SetValue(loginModel, new List<IAccount> { mockAccount.Object });

        // Act
        bool result = loginModel.ValidateUser("gamerstudent@student.uni.edu", "templatepasswordhash");

        // Assert
        Assert.True(result);
    }
```

```csharp
//unit test 3 here
    [Fact]
    public void ValidateUser_InvalidCredentials_ReturnsFalse()
    {
        // Arrange
        var loginModel = new LoginModel(new AccountLoader());

        // Act
        bool result = loginModel.ValidateUser("wronguser", "wrongpass");

        // Assert
        Assert.False(result);
    }

```

### Functional testing
```
========================================
      SYSTEM MANUAL TESTING REPORT      
========================================
----------------------------------------
  STUDENT FUNCTIONALITY TESTS  
----------------------------------------

TEST CASE 1: Enrollment Test  
----------------------------------------
Steps:
1. Log in as a student.
2. Navigate to the list of available subjects.
3. Select a subject and enroll in it.
4. Check if the subject appears in the "Enrolled Subjects" list.

Expected Result:
- The selected subject should be added to the student's enrolled subjects list.

Actual Result:
- The selected subject was added to the student's enrolled subjects list. 

Status: PASS

----------------------------------------
TEST CASE 2: Drop Subject Test  
----------------------------------------
Steps:
1. Log in as a student.
2. Navigate to "Enrolled Subjects."
3. Select a subject and drop it.
4. Check if the subject is removed from the "Enrolled Subjects" list and reappears in "Available Subjects."

Expected Result:
- The dropped subject should disappear from the student’s enrolled list and return to the available subjects.

Actual Result:
- The dropped subject disappeared from the student’s enrolled list and returned to the available subjects

Status: PASS

----------------------------------------
  TEACHER FUNCTIONALITY TESTS  
----------------------------------------

TEST CASE 3: Create Subject Test  
----------------------------------------
Steps:
1. Log in as a teacher.
2. Navigate to the "My Subjects" section.
3. Click on "Create Subject" and enter subject details.
4. Save the subject and check both "My Subjects" and "Available Subjects" lists.

Expected Result:
- The new subject should be visible in both the teacher’s "My Subjects" list and also in student's available subjects

Actual Result:
- The new subject is created and made visible in the teacher’s "My Subjects" list and also in student's available subjects

Status: PASS

----------------------------------------
TEST CASE 4: Delete Subject Test  
----------------------------------------
Steps:
1. Log in as a teacher.
2. Navigate to "My Subjects."
3. Select a subject and delete it.
4. Check if the subject disappears from both "My Subjects" and "Available Subjects."

Expected Result:
- The deleted subject should be removed from both lists.

Actual Result:
- The deleted subject is removed from both lists

Status: [PASS / FAIL]

----------------------------------------
  SYSTEM-LEVEL TESTS  
----------------------------------------

TEST CASE 5: Login Test  
----------------------------------------
Steps:
1. Attempt to log in with valid student credentials.
2. Attempt to log in with valid teacher credentials.
3. Try logging in with incorrect credentials.

Expected Result:
- Valid credentials should allow access to the respective student/teacher dashboard.

Actual Result:
- Test passed with no remarks

Status: PASS

----------------------------------------
TEST CASE 6: Data Persistence Test  
----------------------------------------
Steps:
1. Perform enrollment, subject creation, and deletion actions.
2. Close the application.
3. Reopen the application and check if the changes persist.

Expected Result:
- All modifications (enrollment, dropping, subject creation, deletion) should be saved and persist after restarting the application.

Actual Result:
- All modifications (enrollment, dropping, subject creation, deletion) are saved and are available after restarting the application.

Status: PASS
```
### UI Testing



## Models

## `TeacherModel`

The `TeacherModel` class allows a teacher to manage subjects. It enables viewing, creating, editing, and deleting subjects.

### Properties
- `AccountManager` - Manages account information.
- `_subjectLoader` - Loads subjects from storage.
- `_subjectSaver` - Saves subjects to storage.

### Methods

#### `List<Subject> ViewSubjects()`
Retrieves all subjects assigned to the currently logged-in teacher.

#### `void CreateSubject(string name, string description)`
Creates a new subject with the specified name and description. The subject ID is determined based on the last subject in the list.

#### `void EditSubject(int id, string name, string description)`
Edits an existing subject by updating its name and description.

#### `void DeleteSubject(int id)`
Deletes a subject by its ID. If the subject is found, it is removed from the list and saved.

---

## `StudentModel`

The `StudentModel` class allows students to view, enroll in, and drop subjects.

### Properties
- `AccountManager` - Manages account information.
- `_subjectSaver` - Saves subjects to storage.
- `_subjectLoader` - Loads subjects from storage.

### Methods

#### `List<Subject> ListAllSubjects()`
Returns a list of all available subjects.

#### `List<Subject> ListEnrolledSubjects()`
Returns a list of subjects that the currently logged-in student is enrolled in.

#### `void AddSubject(int subjectId)`
Adds a subject to the student's enrolled subjects list.

#### `void DropSubject(int subjectId)`
Removes a subject from the student's enrolled subjects list if it exists.

---

## `LoginModel`

The `LoginModel` class handles user authentication and account retrieval.

### Properties
- `_accounts` - A list of all accounts loaded from storage.

### Methods

#### `bool ValidateUser(string username, string password)`
Checks if the provided username and password match an existing account.

#### `IAccount GetCurrentAccount(string username)`
Retrieves the account associated with the given username.

#### `List<IAccount> GetAllAccounts()`
Returns a list of all accounts.

#### `void PrintAccountsDebug()`
Prints all loaded accounts for debugging purposes.

---


## ViewModels

## `LoginScreenViewModel`

The `LoginScreenViewModel` class handles user authentication.

### Properties

- `Username` - Stores the user's username.
- `Password` - Stores the user's password.
- `LoginCommand` - Command bound to the login button.
- `LoginSucceeded` - Event triggered upon successful login.

### Methods

#### `void Login()`
Attempts to authenticate the user using the provided credentials.

---

## `StudentViewModel`

The `StudentViewModel` class manages student interactions with subjects.

### Properties

- `Username` - Displays the logged-in student's username.
- `Fullname` - Displays the logged-in student's full name.
- `Subjects` - List of all available subjects.
- `EnrolledSubjects` - List of subjects the student is enrolled in.
- `AvailableSubjects` - List of subjects available for enrollment.
- `SelectedSubjectDrop` - The subject selected for dropping.
- `SelectedSubjectEnroll` - The subject selected for enrollment.
- `DropSubjectCommand` - Command for dropping a subject.
- `EnrollSubjectCommand` - Command for enrolling in a subject.

### Methods

#### `Task Enroll()`
Enrolls the student in a selected subject after confirmation from a popup.

#### `Task Drop()`
Drops the selected subject after confirmation from a popup.

#### `void RefreshSubjects()`
Updates the enrolled and available subjects lists.

#### `void ShowPopup(string customMessage)`
Displays a popup with a message.

#### `bool GetPopupResult()`
Returns the result of the last popup interaction.

---

## `TeacherViewModel`

The `TeacherViewModel` class manages teacher interactions with subjects.

### Properties

- `Username` - Displays the logged-in teacher's username.
- `Fullname` - Displays the logged-in teacher's full name.
- `TeachingSubjects` - List of subjects the teacher is responsible for.
- `ListBoxSelected` - The currently selected subject.

### Commands

- `AddSubjectCommand` - Command for opening the add-subject popup.
- `RemoveSubjectCommand` - Command for deleting a subject.
- `EditSubjectCommand` - Command for opening the edit-subject popup.

### Methods

#### `void OpenSubjectPopup()`
Opens a popup to create a new subject.

#### `void OpenEditPopup()`
Opens a popup to edit an existing subject.

#### `void AddSubject(string name, string description)`
Adds a new subject.

#### `void Remove()`
Deletes the selected subject.

#### `void Edit(int subjectId, string newName, string newDescription)`
Edits an existing subject.

#### `void ShowPopup(string customMessage)`
Displays a popup with a message.

---


## Services/Miscellaneous

## `AccountLoader class:`

The `AccountLoader` class is responsible for loading user account data from JSON files. It supports loading both teacher and student accounts from predefined file paths.  

File Structure  
- **`teacher_users.json`** → Stores teacher account data.  
- **`student_users.json`** → Stores student account data.  

### Usage  
The class provides methods to load both types of accounts separately or together.  

### Methods  

### `public List<IAccount> LoadAccounts()`  
Loads and returns a combined list of both teacher and student accounts.  

**Returns:**  
- `List<IAccount>` → A list containing both `TeacherAccount` and `StudentAccount` objects.  

---

### `public List<StudentAccount> LoadStudentAccounts()`  
Loads student accounts from `student_users.json`.  

**Returns:**  
- `List<StudentAccount>` → A list of `StudentAccount` objects.  

**Behavior:**  
- Reads the `student_users.json` file if it exists.  
- Deserializes the JSON content into a list of `StudentAccount` objects.  
- Returns an empty list if the file does not exist.  

---

### `public List<TeacherAccount> LoadTeacherAccounts()`  
Loads teacher accounts from `teacher_users.json`.  

**Returns:**  
- `List<TeacherAccount>` → A list of `TeacherAccount` objects.  

**Behavior:**  
- Reads the `teacher_users.json` file if it exists.  
- Deserializes the JSON content into a list of `TeacherAccount` objects.  
- Returns an empty list if the file does not exist.  

---

## Example Usage  
```csharp
var accountLoader = new AccountLoader();

// Load all accounts
List<IAccount> allAccounts = accountLoader.LoadAccounts();

// Load student accounts only
List<StudentAccount> students = accountLoader.LoadStudentAccounts();

// Load teacher accounts only
List<TeacherAccount> teachers = accountLoader.LoadTeacherAccounts();
```
---

## `AccountManager class:`

## Overview  
The `AccountManager` class is responsible for managing and updating user accounts for both students and teachers. It can update account data and save the changes back to the corresponding JSON files.

## File Structure  
- **`student_users.json`** → Stores student account data.  
- **`teacher_users.json`** → Stores teacher account data.  

## Properties  

### `public List<StudentAccount> StudentAccounts { get; set; }`  
A list of `StudentAccount` objects. This stores all student accounts loaded into the system.  

### `public List<TeacherAccount> TeacherAccounts { get; set; }`  
A list of `TeacherAccount` objects. This stores all teacher accounts loaded into the system.  

### `public IAccount CurrentAccount { get; set; }`  
The current account in use. This can be either a `StudentAccount` or a `TeacherAccount`.  

### Constructor  

### `public AccountManager(IAccount currentAccount, List<IAccount> accounts)`  
Constructs a new instance of the `AccountManager` class, initializing the lists of `StudentAccount` and `TeacherAccount` based on the provided `accounts` list.  

**Parameters:**  
- `currentAccount` (`IAccount`) → The account to set as the current account.  
- `accounts` (`List<IAccount>`) → A list containing all accounts (both student and teacher) to be managed by the `AccountManager`.  

---

### Methods  

### `public void UpdateStudentData(StudentAccount updatedAccount)`  
Updates the data of an existing student account and saves the updated list back to `student_users.json`.  

**Parameters:**  
- `updatedAccount` (`StudentAccount`) → The student account containing updated data.  

**Behavior:**  
- Searches for the student account by ID.  
- If the student is found, their existing data is replaced with the updated account.  
- The updated list of student accounts is saved to `student_users.json`.  

---

### `public void UpdateTeacherData(TeacherAccount updatedAccount)`  
Updates the data of an existing teacher account and saves the updated list back to `teacher_users.json`.  

**Parameters:**  
- `updatedAccount` (`TeacherAccount`) → The teacher account containing updated data.  

**Behavior:**  
- Searches for the teacher account by ID.  
- If the teacher is found, their existing data is replaced with the updated account.  
- The updated list of teacher accounts is saved to `teacher_users.json`.  

---

### `private void UpdateAccountData<T>(List<T> accounts, string filePath)`  
Saves the updated account data back to the corresponding JSON file. This method is used internally by `UpdateStudentData` and `UpdateTeacherData` to serialize the account lists and write them to the file system.  

**Parameters:**  
- `accounts` (`List<T>`) → The list of accounts (either `StudentAccount` or `TeacherAccount`) to be saved.  
- `filePath` (`string`) → The file path to save the account data (either `student_users.json` or `teacher_users.json`).  

**Behavior:**  
- Serializes the account list to a JSON format with indentation for readability.  
- Writes the JSON string to the specified file path.  
- If an error occurs during the process, an error message is printed to the console.  

---

## Example Usage  

```csharp
// Initialize AccountManager with a list of accounts
var accountLoader = new AccountLoader();
List<IAccount> allAccounts = accountLoader.LoadAccounts();
IAccount currentAccount = allAccounts[0]; // Set the first account as the current account

var accountManager = new AccountManager(currentAccount, allAccounts);

// Update student data
StudentAccount updatedStudent = new StudentAccount { Id = 1, Name = "John Doe", Grade = "A" };
accountManager.UpdateStudentData(updatedStudent);

// Update teacher data
TeacherAccount updatedTeacher = new TeacherAccount { Id = 101, Name = "Mr. Smith", Subject = "Math" };
accountManager.UpdateTeacherData(updatedTeacher);
```
---
