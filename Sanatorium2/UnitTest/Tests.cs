using Moq;
using NUnit.Framework;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;

namespace UnitTest;

public class Tests
{
    private Mock<IDbRepos> _reposMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _reposMock = new Mock<IDbRepos>();
        _userService = new UserService(_reposMock.Object);
    }

    [Test]
    public void AuthenticateUser_ValidCredentials_ReturnsTrue()
    {
        // Arrange
        var users = new List<User>
    {
        new User { Login = "testuser", Password = "password123" }
    };
        _reposMock.Setup(repo => repo.Users.GetCollection()).Returns(users);

        // Act
        var result = _userService.AuthenticateUser("testuser", "password123");

        // Assert
        Assert.IsTrue(result);
    }
}
