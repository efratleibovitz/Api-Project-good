using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Repository;
namespace Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordEntity Level(string password)
        {
            var _result = Zxcvbn.Core.EvaluatePassword(password);
            int _levelPass = _result.Score;
            PasswordEntity _passRes = new PasswordEntity();
            _passRes.Password = password;
            _passRes.Strength = _levelPass;
            return _passRes;

        }
    }
}
