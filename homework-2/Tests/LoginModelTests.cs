using Xunit;
using homework_2_spicymeatballs.Models;
using homework_2_spicymeatballs.AccountLogic;
using Moq;
using System.Collections.Generic;
using homework_2_spicymeatballs;

public class AccountComparer : IEqualityComparer<IAccount>
{
    public bool Equals(IAccount x, IAccount y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }
        

        return x.Id == y.Id &&
               x.Username == y.Username &&
               x.DefinitelyNotPasswordHash == y.DefinitelyNotPasswordHash &&
               x.Name == y.Name &&
               x.Surname == y.Surname;
    }
    

    public int GetHashCode(IAccount obj)
    {
        return obj.Id.GetHashCode();
    }
    
}
public class LoginModelTests
{
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

    [Fact]
    public void LoadUsers_UsersLoadedCorrectly_ReturnsListOfAllUsers()
    {
        

        var mockRecords = new List<IAccount>()
        {
            new TeacherAccount()
            {
                Id = 0,
                Username = "mrbean@teacher.uni.edu",
                DefinitelyNotPasswordHash = "templatepasswordhash",
                Name = "Mr.",
                Surname = "Bean",
                TeachingSubjects = new List<int>() {0}
            }, 
            
            new TeacherAccount()
            {
                Id = 1,
                Username = "ironman@teacher.uni.edu",
                DefinitelyNotPasswordHash = "templatepasswordhash",
                Name = "iron",
                Surname = "man",
                TeachingSubjects = new List<int>() {1}
            }, 
            new TeacherAccount()
            {
                Id = 2,
                Username = "robertkalinak@teacher.uni.edu",
                DefinitelyNotPasswordHash = "templatepasswordhash",
                Name = "Robert",
                Surname = "Kalinak",
                TeachingSubjects = new List<int>() {0}
            }, 
            new StudentAccount()
            {
                Id = 0,
                Username = "gamerstudent@student.uni.edu",
                DefinitelyNotPasswordHash = "templatepasswordhash",
                Name = "Gamer",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {0, 1, 2}
            }, 
            new StudentAccount()
            {
                Id = 1,
                Username = "startupstudent@student.uni.edu",
                DefinitelyNotPasswordHash = "templatepasswordhash",
                Name = "Startup",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {0, 1, 2}
            }, 
            
            new StudentAccount()
            {
                Id = 2,
                Username = "layzstudent@student.uni.edu",
                DefinitelyNotPasswordHash = "templatepasswordhash",
                Name = "Lazy",
                Surname = "Student",
                EnrolledSubjects = new List<int>() {0, 1, 2}
            }, 
        };
        
        
        var loginModel = new LoginModel(new AccountLoader());
        var actualRecords = loginModel.Accounts;
        
        // Assert - Check if both lists have the same length
        Assert.Equal(mockRecords.Count, actualRecords.Count);
        
        Assert.True(mockRecords.SequenceEqual(actualRecords, new AccountComparer()));
        

    }
}