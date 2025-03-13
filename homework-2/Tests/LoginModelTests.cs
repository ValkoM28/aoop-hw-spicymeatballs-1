using Xunit;
using homework_2_spicymeatballs.Models;
using homework_2_spicymeatballs.AccountLogic;
using Moq;
using System.Collections.Generic;
using homework_2_spicymeatballs;

public class LoginModelTests
{
    [Fact]
    public void ValidateUser_ValidCredentials_ReturnsTrue()
    {
        // Arrange: Create a fake account list
        var mockAccount = new Mock<IAccount>();
        mockAccount.Setup(a => a.Username).Returns("gamerstudent@student.uni.edu");
        mockAccount.Setup(a => a.DefinitelyNotPasswordHash).Returns(Hasher.Hashed("templatepasswordhash"));

        var loginModel = new LoginModel();
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
        var loginModel = new LoginModel();

        // Act
        bool result = loginModel.ValidateUser("wronguser", "wrongpass");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LoadUsers_UsersLoadedCorrectly_ReturnsListOfAllUsers()
    {
        var loginModel = new LoginModel();
        List<string> expectedStrings =
        [
            "0 mrbean@teacher.uni.edu templatepasswordhash Mr. Bean",
            "1 ironman@teacher.uni.edu templatepasswordhash iron man",
            "2 robertkalinak@teacher.uni.edu templatepasswordhash Robert Kalinak",
            "0 gamerstudent@student.uni.edu templatepasswordhash Gamer Student",
            "1 startupstudent@student.uni.edu templatepasswordhash Startup Student",
            "2 layzstudent@student.uni.edu templatepasswordhash Lazy Student",
        ];
        
        List<string> actualStrings = new List<string>();
        foreach (var account in loginModel.Accounts)
        {
            actualStrings.Add(account.ToString());
            Console.WriteLine(account);
        }
       
        Assert.Equal(expectedStrings, actualStrings);
    }

}