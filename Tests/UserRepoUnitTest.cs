using DTOs;
using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Repository;

namespace Tests
{
    public class UserRepoUnitTest : TestBase
    {
        #region happy tests
        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsUser()
        {
            // Arrange
            var users = new List<User> { new User { Id = 1, UserEmail = "test@test.com" } };
            var mockContext = new Mock<WebApiShop216328971Context>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var repo = new UserRepository(mockContext.Object);

            // Act
            var result = await repo.GetUserById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }



        [Fact]
        public async Task RegisterAsync_ValidUser_ReturnsAddedUser()
        {
            // Arrange
            var mockContext = new Mock<WebApiShop216328971Context>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());
            var repo = new UserRepository(mockContext.Object);
            var newUser = new User { UserEmail = "new@test.com", FirstName = "A", LastName = "B", Password = "Ee123!@#WWW" };

            // Act
            var result = await repo.addUser(newUser);

            // Assert
            Assert.Equal("new@test.com", result.UserEmail);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_CorrectCredentials_ReturnsUser()
        {
            // Arrange
            var user = new User { UserEmail = "u@u.com", Password = "123" };
            var users = new List<User> { user };
            var mockContext = new Mock<WebApiShop216328971Context>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var repo = new UserRepository(mockContext.Object);

            // יצירת ה-DTO שהפונקציה מצפה לו
            var loginDto = new LoginDTO("u@u.com", "123");

            // Act
            var result = await repo.login(loginDto); // כאן העדכון

            // Assert
            Assert.NotNull(result);
            Assert.Equal("u@u.com", result.UserEmail);
        }
        [Fact]
        public async Task login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var users = new List<User> { new User { UserEmail = "real@test.com", Password = "1234" } };
            var mockContext = GetMockContext<WebApiShop216328971Context, User>(users, c => c.Users);

            var repo = new UserRepository(mockContext.Object);

            // Act
            // אנחנו יוצרים DTO עם פרטים שלא תואמים את מה שיש ב-Mock (למשל אימייל אחר)
            var loginDto = new LoginDTO("fake@test.com", "0000");

            var result = await repo.login(loginDto);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task UpdateAsync_ValidUpdate_ReturnsUpdatedUser()
        {
            // Arrange
            var user = new User { Id = 1, FirstName = "OldName", UserEmail = "u@u.com", Password = "123" };
            var mockContext = new Mock<WebApiShop216328971Context>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User> { user });
            var repo = new UserRepository(mockContext.Object);

            // יצירת ה-DTO
            var userDto = new UserDto("u@u.com", "NewName", "LastName", "123");

            // Act
            await repo.UpdateUser(1, userDto); // שליחת ID ו-DTO

            // Assert
            Assert.Equal("NewName", user.FirstName);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
        #endregion

        #region unhappy tests
        // אין משתמש עם מזהה כזה
        [Fact]
        public async Task GetUserById_NotExistingId_ReturnsNull()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, UserEmail = "a@a.com" }
            };

            var mockContext =
                GetMockContext<WebApiShop216328971Context, User>(users, c => c.Users);

            var repo = new UserRepository(mockContext.Object);

            // Act
            var result = await repo.GetUserById(999);

            // Assert
            Assert.Null(result);
        }
        // כניסה עם סיסמה שגויה

        [Fact]
        public async Task Login_WrongPassword_ReturnsNull()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserEmail = "test@test.com", Password = "1234" }
            };

            var mockContext =
                GetMockContext<WebApiShop216328971Context, User>(users, c => c.Users);

            var repo = new UserRepository(mockContext.Object);

            // Act
            var loginDto = new LoginDTO("test@test.com", "WRONG");
            var result = await repo.login(loginDto);

            // Assert
            Assert.Null(result);
        }
        //User null
        [Fact]
        public async Task Register_NullUser_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<WebApiShop216328971Context>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());
            var repo = new UserRepository(mockContext.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => repo.addUser(null)
            );
        }

        // עדכון משתמש שלא קיים
        [Fact]
        public async Task UpdateUser_NotExistingUser_DoesNotThrowAndDoesNotSave()
        {
            // Arrange
            var users = new List<User>(); // רשימה ריקה - המשתמש לא קיים

            var mockContext = GetMockContext<WebApiShop216328971Context, User>(users, c => c.Users);
            var repo = new UserRepository(mockContext.Object);

            // יצירת DTO לצורך הבדיקה
            var userDto = new UserDto("test@test.com", "X", "Y", "1234");

            // Act
            // אנחנו קוראים לפונקציה עם ID שלא קיים (99)
            await repo.UpdateUser(99, userDto);

            // Assert
            // מכיוון שהפונקציה לא מחזירה ערך, אנחנו מוודאים שהיא לא ניסתה לשמור שינויים ב-DB
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }


        #endregion
    }
}
