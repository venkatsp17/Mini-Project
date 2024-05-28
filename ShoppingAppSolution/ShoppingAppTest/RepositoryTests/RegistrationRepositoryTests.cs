using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.RepositoryTests
{
    [TestFixture]
    public class RegistrationRepositoryTests
    {
        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Test]
        public async Task AddCustomer_UserTransaction_ShouldAddCustomerAndUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new UserRegisterDTO
            {
                Username = "testuser",
                Password = Encoding.UTF8.GetBytes("password"),
                Password_Hashkey = Encoding.UTF8.GetBytes("hashkey"),
                IsAdmin = false,
                Role = Enums.UserRole.Customer,
                Email = "customer@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };

            // Act
            var result = await repository.AddCustomer_UserTransaction(userRegisterDTO);

            // Assert
            Assert.NotNull(result.customer);
            Assert.NotNull(result.user);
            Assert.That(result.user.Username, Is.EqualTo(userRegisterDTO.Username));
            Assert.That(result.customer.Email, Is.EqualTo(userRegisterDTO.Email));
        }

        [Test]
        public async Task AddSeller_UserTransaction_ShouldAddSellerAndUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new UserRegisterDTO
            {
                Username = "testseller",
                Password = Encoding.UTF8.GetBytes("password"),
                Password_Hashkey = Encoding.UTF8.GetBytes("hashkey"),
                IsAdmin = false,
                Role = Enums.UserRole.Seller,
                Email = "seller@example.com",
                Name = "Jane Doe",
                Address = "456 Elm St",
                Phone_Number = "987-654-3210",
                Date_of_Birth = new DateTime(1985, 5, 15),
                Gender = "Female",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };

            // Act
            var result = await repository.AddSeller_UserTransaction(userRegisterDTO);

            // Assert
            Assert.NotNull(result.seller);
            Assert.NotNull(result.user);
            Assert.That(result.user.Username, Is.EqualTo(userRegisterDTO.Username));
            Assert.That(result.seller.Email, Is.EqualTo(userRegisterDTO.Email));
        }

        [Test]
        public void AddCustomer_UserTransaction_ShouldRollbackOnError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new UserRegisterDTO
            {
                Username = "testuser",
                Password = Encoding.UTF8.GetBytes("password"),
                Password_Hashkey = Encoding.UTF8.GetBytes("hashkey"),
                IsAdmin = false,
                Role = Enums.UserRole.Customer,
                Email = "customer@example.com",
                Name = "John Doe",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Date_of_Birth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };

            // Act & Assert
            Assert.ThrowsAsync<UnableToRegisterException>(async () =>
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var result = await repository.AddCustomer_UserTransaction(userRegisterDTO);

                    // Simulate an error by throwing an exception after user is added
                    context.Users.Add(new User { Username = null }); // This will cause a validation error
                    await context.SaveChangesAsync();
                }
            });
        }

        [Test]
        public void AddSeller_UserTransaction_ShouldRollbackOnError()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new RegistrationRepository(context);
            var userRegisterDTO = new UserRegisterDTO
            {
                Username = "testseller",
                Password = Encoding.UTF8.GetBytes("password"),
                Password_Hashkey = Encoding.UTF8.GetBytes("hashkey"),
                IsAdmin = false,
                Role = Enums.UserRole.Seller,
                Email = "seller@example.com",
                Name = "Jane Doe",
                Address = "456 Elm St",
                Phone_Number = "987-654-3210",
                Date_of_Birth = new DateTime(1985, 5, 15),
                Gender = "Female",
                Profile_Picture_URL = "http://example.com/profile.jpg"
            };

            // Act & Assert
            Assert.ThrowsAsync<UnableToRegisterException>(async () =>
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var result = await repository.AddSeller_UserTransaction(userRegisterDTO);

                    // Simulate an error by throwing an exception after user is added
                    context.Users.Add(new User { Username = null }); // This will cause a validation error
                    await context.SaveChangesAsync();
                }
            });
        }
    }
}
