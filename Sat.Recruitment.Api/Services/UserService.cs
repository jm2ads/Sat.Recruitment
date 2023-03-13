using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sat.Recruitment.Api.Models;
using System.Configuration;
using System.Net;
using static Sat.Recruitment.Api.Utils.Constants;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly DemoContext _context;
        public UserService( )
        {
        }

        public List<User> GetUsers()
        {
            List<User> userList =  GetUsersMethod();
            return userList;
        }

        public User? GetUser(int UserId)
        {
            User user = GetUsersByUserIdMethod(UserId);
         
            return user;
        }

        public Result PostUser(User user)
        {
            var errors = "";
            ValidateErrors(user, ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };
           
            List<User> userList = GetUsersMethod();
            var maxUserId = 0;
            if (userList.Count > 0)
            {
                maxUserId = userList.Max(x => x.UserId);
            }

            user.UserId = maxUserId + 1;

             user = AddGif(user);


            userList.Add(user);

            bool saved =  SaveJson(userList);

            if (!saved)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "Error"
                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = true,
                    Errors = "User Created"
                };
            }
        }

        public Result PutUser(User user)
        {
            var errors = "";
            ValidateErrors(user, ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };

            User userAux = GetUsersByUserIdMethod(user.UserId);
            if (userAux == null)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "NotFound"
                };
            }
            DeleteUser(user.UserId);

            List<User> userList = GetUsersMethod();
            user = AddGif(user);
            userList.Add(user);
          
            bool saved =  SaveJson(userList);

            if (!saved)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "Error"
                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = true,
                    Errors = "User Created"
                };
            }
        }

        public Result DeleteUser(int UserId)
        {
            List<User> userList = GetUsersMethod();
            User? user = (from u in userList
                          where u.UserId == UserId
                          select u).FirstOrDefault();
            if (user == null)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "NotFound"
                };
            }

            userList.Remove(user);

            bool saved = SaveJson(userList);

            if (!saved)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "Error"
                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = true,
                    Errors = "User Created"
                };
            }
        }










        private List<User> GetUsersMethod()
        {

            string path = Directory.GetCurrentDirectory() +  FilePath ;

            List<User> userList = new List<User>();
            using (var writer = new StreamReader(path))
            {
                string jsonString = writer.ReadToEnd();
                userList = JsonConvert.DeserializeObject<List<User>>(jsonString);
            };
            if (userList == null)
                userList = new List<User>();

            return userList;

        }
        private User GetUsersByUserIdMethod(int UserId)
        {
            string path = Directory.GetCurrentDirectory() + FilePath;
            List<User>? userList = new List<User>();
            User user = new User();
            using (var writer = new StreamReader(path))
            {
                string jsonString = writer.ReadToEnd();
                userList = JsonConvert.DeserializeObject<List<User>>(jsonString);
            };
            if (userList == null)
                userList = new List<User>();

            user = (from u in userList
                    where u.UserId == UserId
                    select u).FirstOrDefault();


            return user;

        }

        private bool SaveJson(List<User> userList)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + FilePath;

                string json = JsonConvert.SerializeObject(userList);

                System.IO.File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //Validate errors
        private void ValidateErrors(User user, ref string errors)
        {
            if (string.IsNullOrEmpty( user.Name))
                //Validate if Name is null
                errors = "The name is required";
            if (string.IsNullOrEmpty(user.Email))
                //Validate if Email is null
                errors = errors + " The email is required";
            if (string.IsNullOrEmpty(user.Address))
                //Validate if Address is null
                errors = errors + " The address is required";
            if (string.IsNullOrEmpty(user.Phone ))
                //Validate if Phone is null
                errors = errors + " The phone is required";


            if (!(new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(user.Email)))
                errors = errors + " The mail is not valid";

            var UserTypesList = Enum.GetValues(typeof(UserTypes))
                           .Cast<UserTypes>()
                           .Select(d =>  d.ToString())
                           .ToList();

            string? UserTypesAux = (from u in UserTypesList
                          where u == user.UserType
                                   select u).FirstOrDefault();
            if (string.IsNullOrEmpty(UserTypesAux))
                errors = errors + " The UserType is not valid";
        }
        private User AddGif(User user)
        {
            if (user.UserType == Utils.Constants.UserTypes.Normal.ToString())
            {
                if (user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = user.Money * percentage;
                    user.Money = user.Money + gif;
                }
                if (user.Money < 100)
                {
                    if (user.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                }
            }
            if (user.UserType == Utils.Constants.UserTypes.SuperUser.ToString())
            {
                if (user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = user.Money * percentage;
                    user.Money = user.Money + gif;
                }
            }
            if (user.UserType == Utils.Constants.UserTypes.Premium.ToString())
            {
                if (user.Money > 100)
                {
                    var gif = user.Money * 2;
                    user.Money = user.Money + gif;
                }
            }


            return user;
        }

    }
}
