
namespace Tests
{


    using Xunit;
    using Entities;
    using Repository;
    using Microsoft.EntityFrameworkCore;
    using Tests;
    using DTOs;

    public class UserRepositoryIntegrationTests : IClassFixture<DbFixture>
    {
        private readonly WebApiShop216328971Context _context;
        private readonly UserRepository _repository;

        public UserRepositoryIntegrationTests(DbFixture fixture)
        {
            _context = fixture.Context;
            _repository = new UserRepository(_context);

            // ❌ לא סוגרים ולא עושים Dispose כאן
            // מבודד את ה־ChangeTracker כדי למנוע tracked duplicates
            _context.ChangeTracker.Clear();
        }
        public Task DisposeAsync()
        {
            // לא סוגרים את Context אם Fixture אחראי עליו
            // אפשר לנקות נתונים פה אם רוצים
            return Task.CompletedTask;
        }

        #region HappyTests
        [Fact]
        public async Task RegisterAsync_ShouldSaveUserToRealDatabase()
        {
            var user = new User { UserEmail = "integration@test.com", Password = "123", FirstName = "Test", LastName = "User" };

            var result = await _repository.addUser(user);

            var userInDb = await _context.Users.FindAsync(result.Id);
            Assert.NotNull(userInDb);
            Assert.Equal("integration@test.com", userInDb.UserEmail);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsUserFromDb()
        {
            var user = new User { UserEmail = "login@integration.com", Password = "password123", FirstName = "A", LastName = "B" };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var loginDto = new LoginDTO(user.UserEmail, user.Password);
            var result = await _repository.login(loginDto);

            Assert.NotNull(result);
            Assert.Equal("login@integration.com", result.UserEmail);
        }
        #endregion


        #region unHappy Tests
        // כניסה עם סיסמה שגויה
        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            var user = new User
            {
                UserEmail = "fail@test.com",
                Password = "123",
                FirstName = "A",
                LastName = "B"
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var loginAttempt = new User
            {
                UserEmail = "fail@test.com",
                Password = "wrong"
            };
            var loginDto = new LoginDTO("fail@test.com", "wrong");
            var result = await _repository.login(loginDto);

            Assert.Null(result);
        }
        //רישום משתמש עם אימייל שכבר קיים
        [Fact]
        //לא עובר כי אין דרישת ייחודיות במייל בטבלה

        public async Task RegisterAsync_DuplicateEmail_ThrowsException()
        {
            var user1 = new User { UserEmail = "dup@test.com", Password = "123" };
            var user2 = new User { UserEmail = "dup@test.com", Password = "456" };

            await _repository.addUser(user1);

            await Assert.ThrowsAsync<DbUpdateException>(() =>
                _repository.addUser(user2));
        }
        // כניסה עם אימייל שלא קיים
        [Fact]
        public async Task Login_EmailNotExists_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDTO("notexist@test.com", "123");

            // Act
            var result = await _repository.login(loginDto);

            // Assert
            Assert.Null(result);
        }
        
        

        #endregion
    }
}

