using System.Reflection.Metadata;
using System.Text.Json;
using Entities;
namespace Repository
{
    public class UserRepository : IUserRepository
    {
        string _filePath = "newUsers.txt";
        public User GetUserById(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? _currentUserInFile;
                while ((_currentUserInFile = reader.ReadLine()) != null)
                {
                    User? _user = JsonSerializer.Deserialize<User>(_currentUserInFile);
                    if (_user != null && _user.id == id)
                        return _user;
                }
            }
            return null;
        }

        public User addUser(User user)
        {
            int _numberOfUsers = System.IO.File.ReadLines(_filePath).Count();
            user.id = _numberOfUsers + 1;
            string _userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(_filePath, _userJson + Environment.NewLine);
            return user;

        }
        public void updateUser(int id, User user)
        {
            string _textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? _currentUserInFile;
                while ((_currentUserInFile = reader.ReadLine()) != null)
                {
                    User? _user = JsonSerializer.Deserialize<User>(_currentUserInFile);
                    if (_user != null && _user.id == id)
                        _textToReplace = _currentUserInFile;
                }
            }

            if (_textToReplace != string.Empty)
            {
                string _text = System.IO.File.ReadAllText(_filePath);
                _text = _text.Replace(_textToReplace, JsonSerializer.Serialize(user));
                System.IO.File.WriteAllText(_filePath, _text);
            }
        }
        public User login(User user)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? _currentUserInFile;
                while ((_currentUserInFile = reader.ReadLine()) != null)
                {
                    User? _user = JsonSerializer.Deserialize<User>(_currentUserInFile);
                    if (_user != null && _user.userName == user.userName && _user.passWord == user.passWord)
                        return _user;
                }
            }
            return null;
        }
    }
}
