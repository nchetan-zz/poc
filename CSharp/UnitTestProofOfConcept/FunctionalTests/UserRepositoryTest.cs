using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Implementation;
using NUnit.Framework;

namespace FunctionalTests
{
    public abstract class UserRepositoryTest : TestBase
    {
        private const string InsertScript = "INSERT [User] (Id, UserName, Password, FirstName, LastName) VALUES ({0}, '{1}', '{2}', '{3}', '{4}'); SELECT SCOPE_IDENTITY();";
        private const string DeleteUserSqlScript = "DELETE [User] where Id = {0}";

        // SetupStub is not overridden as no stubs are required as this functional test does go to the database.

        protected virtual void _CreateUser(int id, string userName, string password, string firstName, string lastName)
        {
            var sqlCommandText = string.Format(InsertScript, id, userName, password, firstName, lastName);
            _ExecuteSqlScript(sqlCommandText);
        }

        protected void _RemoveUser(int userId)
        {
            var sqlStatement = string.Format(DeleteUserSqlScript, userId);
            _ExecuteSqlScript(sqlStatement);
        }

        protected static void _ExecuteSqlScript(string sqlStatement)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var sqlCommand = new SqlCommand(sqlStatement, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
        }
    }

    #region GetUser tests

    public class GetUserTests : UserRepositoryTest 
    {
        protected User Result;

        protected override void Execute()
        {
            var userRespository = new UserRepository();
            Result = userRespository.Get(1);
        }
    }

    public class WhenGettingUser : GetUserTests
    {
        // This test does 2 assertions. 
        // #1. The test did not throw an exception.
        // #2. User was created with non zero identity.
        [Test]
        public void ItShouldCreateAUser()
        {
            Assert.AreNotEqual(0, Result);
        }
    }

    #endregion GetUser tests

    #region CreateUser tests

    public class CreateUserTest : UserRepositoryTest
    {
        private User _userToCreate;
        protected List<User> Result;


        protected override void AdjustContext()
        {
            base.AdjustContext();

            _userToCreate = new User
            {
                Id = 1,
                FirstName = "Created",
                LastName = "User",
                UserName = "CreatedUser",
                Password = "Some Created User password",
            };
        }

        protected override void Execute()
        {
            var userRepository = new UserRepository();
            userRepository.CreateUser(_userToCreate);

            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (var context = new DatabaseClassesDataContext(connectionString))
            {
                Result = context.Users.Where(x => x.FirstName == "Created" && x.LastName == "User").ToList();
            }
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _RemoveUser(1);
        }
    }

    [TestFixture]
    public class WhenCreatingUser : CreateUserTest
    {
        [Test]
        public void UserListIsNotEmpty()
        {
            Assert.IsNotEmpty(Result);
            Assert.IsNotEmpty(Result);
        }

        [Test]
        public void UserNameIsSaved()
        {
            var createdUser = Result.First();
            Assert.AreEqual("CreatedUser", createdUser.UserName);
        }

        [Test]
        public void UserIdIsGenerated()
        {
            var createdUser = Result.First();
            Assert.AreNotEqual(0, createdUser.Id);
        }

    }

    #endregion CreateUser tests
}
