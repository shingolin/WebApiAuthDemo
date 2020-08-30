using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAuthDemo.Controllers
{
    /// <summary>
    /// 使用者驗證服務
    /// </summary>
    public class UserValidateService
    {
        public UserValidateService()
        {

        }

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        internal bool ValidateUser(LoginViewModel login)
        {
            if (new string[] { "stanley", "shingo" }.Contains(login.Username.ToLower()))
                return true;
            return false;
        }
    }
}
