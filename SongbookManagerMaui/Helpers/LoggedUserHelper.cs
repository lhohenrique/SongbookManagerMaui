using SongbookManagerMaui.Models;
using SongbookManagerMaui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Helpers
{
    public class LoggedUserHelper
    {
        private static User loggedUser;
        public static User LoggedUser
        {
            get
            {
                if (loggedUser == null)
                {
                    loggedUser = new User();
                }

                return loggedUser;
            }
        }

        private static UserService userService = new UserService();

        public static async Task UpdateLoggedUserAsync()
        {
            var userEmail = Preferences.Get("Email", string.Empty);

            var user = await userService.GetUser(userEmail);

            loggedUser = user;
        }

        public static bool HasSharedList()
        {
            return !string.IsNullOrEmpty(LoggedUser.SharedList);
        }

        public static string GetEmail()
        {
            if (HasSharedList())
            {
                return LoggedUser.SharedList;
            }
            else
            {
                return LoggedUser.Email;
            }
        }

        public static void ResetLoggedUser()
        {
            loggedUser = null;
        }
    }
}
