using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAuthDemo.Controllers
{
    public class UserInfoService
    {
        /// <summary>
        /// 取得使用者資訊的服務
        /// </summary>
        public UserInfoService()
        {

        }
        /// <summary>
        /// 取得api的權限
        /// </summary>
        /// <param name="UserId">使用者編號</param>
        /// <returns></returns>
        public string[] GetApiRoles(string UserId)
        {
            if (UserId == "stanley")
            {
                return new string[] { "A", "B", "C", "D", "E", "Admin" };
            }
            else if (UserId == "shingo")
            {
                return new string[] { "A", "C" };
            }
            return new string[] { };
        }
        /// <summary>
        /// 取得使用者資訊
        /// </summary>
        /// <param name="UserId">使用者編號</param>
        /// <returns></returns>
        public UserInfo GetUserInfo(string UserId)
        {
            if (UserId == "stanley")
            {
                return new UserInfo
                {
                    UserName = "系統管理者",
                    UserId = UserId,
                    Company = "A",
                    DataGroups = new List<string> { "1", "2", "3" },
                };
            }
            else if (UserId == "shingo")
            {
                return new UserInfo
                {
                    UserName = "林志興",
                    UserId = UserId,
                    Company = "A",
                    DataGroups = new List<string> { "1" },
                };
            }
            return null;
        }
    }
}
