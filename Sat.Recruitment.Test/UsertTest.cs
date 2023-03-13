


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;
using System.Configuration;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsertTest
    {


        private readonly UsersController _usersController;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UsertTest()
        {
            _userService = new UserService();
            _usersController = new UsersController(_userService);
        }

        [Fact]
        public void GetUsers()
        {
            var userController = new UsersController(_userService);
            //Arrange
           
            //Act
            var result = _usersController.GetUsers();
            //Assert
            var users = Assert.IsType<List<User>>(result.Result.Value);
            Assert.True(users != null);
            Assert.True(users.Count()> 0);

        }


        [Fact]
        public void GetUsersByUserId()
        {
           var userController = new UsersController(_userService);
            //Arrange
            int userId = 8;
            //Act
            var result =   _usersController.GetUser(userId);
            //Assert
            var user = Assert.IsType<User>(result.Result.Value);
            Assert.True(user!=null);
            Assert.Equal(user?.UserId, userId);
         
        }

        [Fact]
        public void GetUsersByUserId_NotFound()
        {
            var userController = new UsersController(_userService);
            //Arrange
            int userId = 99;
            //Act
            var result = _usersController.GetUser(userId);
            //Assert
            User? user = result.Result.Value;
            Assert.True(user == null);
           

        }

        [Fact]
        public async Task PostUser()
        {
            var userController = new UsersController(_userService);
            //Arrange
            User user = new User ();
            user.Name = "Alejandro Sosa";
            user.Email = "adfs@outlook.com.ar";
            user.Address = "Odisea 22";
            user.Phone = "1234567890";
            user.UserType= Api.Utils.Constants.UserTypes.SuperUser.ToString();
            user.Money = 1500;
            //Act
            var rta = _usersController.PostUser(user).Result;
            var result = (OkObjectResult)rta.Result;
            //Assert


            Assert.True(result.Value == "Ok");
            Assert.True(result.StatusCode == 200);


        }
        [Fact]
        public async Task PostUser_Wrong()
        {
            var userController = new UsersController(_userService);
            //Arrange
            User user = new User();
            user.Name = "";
            user.Email = "";
            user.Address = "";
            user.Phone = "";
            user.UserType = Api.Utils.Constants.UserTypes.SuperUser.ToString();
            user.Money = 1500;
            //Act
            var rta = _usersController.PostUser(user).Result;
            var result = (BadRequestObjectResult)rta.Result;
            //Assert
            Assert.True(result.StatusCode == 400);
        }
        [Fact]
        public async Task PutUser()
        {
            var userController = new UsersController(_userService);
            //Arrange
            User user = new User();
            user.UserId = 7;
            user.Name = "Alejandro Sosa";
            user.Email = "adfs@outlook.com.ar";
            user.Address = "Odisea 22";
            user.Phone = "1234567890";
            user.UserType = Api.Utils.Constants.UserTypes.SuperUser.ToString();
            user.Money = 1500;
            //Act
            var rta = _usersController.PutUser(user).Result;
            var result = (OkObjectResult)rta.Result;
            //Assert


            Assert.True(result.Value == "Ok");
            Assert.True(result.StatusCode == 200);


        }

        [Fact]
        public async Task PutUser_Wrong()
        {
            var userController = new UsersController(_userService);
            //Arrange
            User user = new User();
            user.UserId = 1;
            user.Name = "";
            user.Email = "";
            user.Address = "";
            user.Phone = "";
            user.UserType = "";
            user.Money = 1500;
            //Act
            var rta = _usersController.PutUser(user).Result;
            var result = (BadRequestObjectResult)rta.Result;
            //Assert
            Assert.True(result.StatusCode == 400);
        }
        [Fact]

        public async Task PutUser_NotFound()
        {
            var userController = new UsersController(_userService);
            //Arrange
            User user = new User();
            user.UserId = 99;
            user.Name = "Alejandro Sosa";
            user.Email = "adfs@outlook.com.ar";
            user.Address = "Odisea 22";
            user.Phone = "1234567890";
            user.UserType = Api.Utils.Constants.UserTypes.SuperUser.ToString();
            user.Money = 1500;
            //Act
            var rta = _usersController.PutUser(user).Result;
            var result = (BadRequestObjectResult)rta.Result;
            //Assert
            Assert.True(result.Value == "NotFound");
            Assert.True(result.StatusCode == 400);
        }
        [Fact]
        public async Task DeleteUser()
        {
            var userController = new UsersController(_userService);
            //Arrange
            int UserId = 8;
           
            //Act
            var rta = _usersController.DeleteUser(UserId).Result;
            var result = (OkObjectResult)rta.Result;
            //Assert


            Assert.True(result.Value == "Ok");
            Assert.True(result.StatusCode == 200);


        }

        [Fact]
        public void DeleteUser_NotFound()
        {
            var userController = new UsersController(_userService);
            //Arrange
            int userId = 99;
            //Act
            var rta = _usersController.DeleteUser(userId).Result;
            var result = (BadRequestObjectResult)rta.Result;
            //Assert
            Assert.True(result.Value == "NotFound");
            Assert.True(result.StatusCode == 400);


        }
    }
}