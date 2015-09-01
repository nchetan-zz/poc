using System;
using DataAccessLayer;
using DataAccessLayer.Interface;
using FunctionalTests;
using Moq;
using NUnit.Framework;
using ProductionCode;

namespace UnitTestProofOfConcept
{
    public abstract class DuplicateUserCommandTest : TestBase 
    {
        protected Mock<IUserRepository> UserRepository;
        protected int SourceUserId;
        protected User Result;

        protected override void Execute()
        {
            var command = new DuplicateUserCommand(UserRepository.Object);
            command.Execute(SourceUserId);
            Result = UserRepository.Object.Get(SourceUserId + 1);
        }

    }

    public class WhenDuplicatingUser : DuplicateUserCommandTest
    {
        private User _sourceUser;

        protected override void AdjustContext()
        {
            base.AdjustContext();

            var randomGenerator= new Random();
            SourceUserId = randomGenerator.Next();
            var firstName = "FirstName" + randomGenerator.Next();
            var lastName = "LastName" + randomGenerator.Next();
            var userName = "UserName" + randomGenerator.Next();
            var password = "Password" + randomGenerator.Next();

            var id = randomGenerator.Next();
            _sourceUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Password = password,
                Id = id,
            };
        }

        protected override void SetupStubs()
        {
            base.SetupStubs();
            UserRepository = new Mock<IUserRepository>();

            // NOTE: UserRepository.CreateUser is not mocked. So we get a default no-op operation on it.
            UserRepository.Setup(x => x.Get(SourceUserId))
                .Returns(_sourceUser);

            UserRepository.Setup(x => x.Get(SourceUserId+1))
                .Returns(new User
                {
                    FirstName = _sourceUser.FirstName,
                    LastName = _sourceUser.LastName,
                    UserName = _sourceUser.UserName,
                    Password = _sourceUser.Password,
                    Id = _sourceUser.Id+1,
                });
        }

        [Test]
        public void ItShouldGetAUser()
        {
            Assert.NotNull(Result);
        }

        [Test]
        public void ItShouldCopyFirstName()
        {
            Assert.AreEqual(_sourceUser.FirstName, Result.FirstName);
        }
    }
}
