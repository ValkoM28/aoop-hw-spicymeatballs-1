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

```csharp
//unit test 1 here
```

```csharp
//unit test 2 here
```

```csharp
//unit test 3 here
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

## ViewModels

## Views

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
