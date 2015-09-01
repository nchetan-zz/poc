namespace DataAccessLayer.Interface
{
    public interface IUserRepository
    {
        User Get(int userId);
        void CreateUser(User userToCreate);
    }
}
