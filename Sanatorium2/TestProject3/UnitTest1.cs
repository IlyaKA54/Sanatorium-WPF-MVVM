using Microsoft.EntityFrameworkCore;
using Moq;
using TestProject3.Data;

namespace TestProject3
{
    public class Tests
    {
        private Mock<IDbRepos> _reposMock;
        private UserService _userService;

        private Mock<IDbRepos> _mockRepos;
        private CustomerService _customerService;


        [SetUp]
        public void Setup()
        {
            _reposMock = new Mock<IDbRepos>();
            _userService = new UserService(_reposMock.Object);

            _mockRepos = new Mock<IDbRepos>();
            _customerService = new CustomerService(_mockRepos.Object);

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

        [Test]
        public void AuthenticateUser_InvalidUserName_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>
        {
            new User { Login = "testuser", Password = "password123" }
        };
            _reposMock.Setup(repo => repo.Users.GetCollection()).Returns(users);

            // Act
            var result = _userService.AuthenticateUser("invaliduser", "password123");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AuthenticateUser_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>
        {
            new User { Login = "testuser", Password = "password123" }
        };
            _reposMock.Setup(repo => repo.Users.GetCollection()).Returns(users);

            // Act
            var result = _userService.AuthenticateUser("testuser", "invalidpassword");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AuthenticateUser_NullOrEmptyCredentials_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>
        {
            new User { Login = "testuser", Password = "password123" }
        };
            _reposMock.Setup(repo => repo.Users.GetCollection()).Returns(users);

            // Act
            var resultEmptyUsername = _userService.AuthenticateUser("", "password123");
            var resultEmptyPassword = _userService.AuthenticateUser("testuser", "");

            // Assert
            Assert.IsFalse(resultEmptyUsername);
            Assert.IsFalse(resultEmptyPassword);
        }

        [Test]
        public void AuthenticateUser_NoUsers_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>();
            _reposMock.Setup(repo => repo.Users.GetCollection()).Returns(users);

            // Act
            var result = _userService.AuthenticateUser("testuser", "password123");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Customer_ValidId_ReturnsCustomer()
        {
            // Arrange
            int customerId = 1;
            var expectedCustomer = new Customer { Id = customerId, FirstName = "John", SecondName = "Doe" };
            _mockRepos.Setup(r => r.Customers.GetItem(customerId)).Returns(expectedCustomer);

            // Act
            var result = _customerService.GetCustomer(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCustomer.Id, result.Id);
            Assert.AreEqual(expectedCustomer.FirstName, result.FirstName);
            Assert.AreEqual(expectedCustomer.SecondName, result.SecondName);
        }

        [Test]
        public void GetFilterList_ValidFilter_ReturnsFilteredCustomers()
        {
            // Arrange
            string filter = "John";
            var customers = new List<Customer>
            {
            new Customer { Id = 1, FirstName = "John", SecondName = "Doe" },
            new Customer { Id = 2, FirstName = "Jane", SecondName = "Smith" },
            new Customer { Id = 3, FirstName = "John", SecondName = "Williams" }
            };
            _mockRepos.Setup(r => r.Customers.GetCollection()).Returns(customers);

            // Act
            var result = _customerService.GetFilterList(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Two customers with FirstName "John"
        }

    }
}