using System.Linq;
using DataAccessLayer.Interface;

namespace DataAccessLayer.Implementation
{
    public class UserRepository : IUserRepository
    {
        private const string DatabaseConnectionString = "Data Source=localhost; Initial Catalog=Database; Integrated Security=SSPI;";

        public User Get(int userId)
        {
            using (var context = new DatabaseClassesDataContext(DatabaseConnectionString))
            {
                var user = context.Users
                    .FirstOrDefault(x => x.Id == userId);

                return user;

            }
        }

        public void CreateUser(User userToCreate)
        {
            using (var context = new DatabaseClassesDataContext(DatabaseConnectionString))
            {
                context.Users.InsertOnSubmit(userToCreate);

                context.SubmitChanges();
            }
        }
    }
}
