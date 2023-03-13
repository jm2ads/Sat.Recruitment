namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        public List<User> GetUsers();
        public User? GetUser(int UserId);

        public Result PostUser(User user);
        public Result PutUser(User user);
        public Result DeleteUser(int UserId);

    }
}
