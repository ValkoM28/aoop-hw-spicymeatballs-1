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
