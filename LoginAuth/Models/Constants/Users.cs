using System.Collections.Generic;

namespace LoginAuth.Models.Constants
{
    public class Users
    {
        public static List<UserModel> UserData = new List<UserModel>
        {
            new UserModel { Username = "user"
                , EmailAddress = "user@gmail.com"
                , GivenName = "user"
                , Password = "user"
                , Surname = "reg"
                , Role = "Regular" },

            new UserModel { Username = "admin"
                , EmailAddress = "admin@gmail.com"
                , GivenName = "admin"
                , Password = "admin"
                , Surname = "admin"
                , Role = "Administrator" }
        };
    }
}
