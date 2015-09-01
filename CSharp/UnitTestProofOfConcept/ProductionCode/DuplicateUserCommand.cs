using DataAccessLayer;
using DataAccessLayer.Interface;

namespace ProductionCode
{
    public class DuplicateUserCommand : IDuplicateUserCommand
    {
        private readonly IUserRepository _userRepository;

        public DuplicateUserCommand(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Ideally do this in transactional behavior, however not important for this demo.
        public void Execute(int sourceUserId)
        {
            var sourceUser = _userRepository.Get(sourceUserId);

            // Ideally a copy constructor or a helper would do this. For now, keeping it here to make code readable.
            var copiedUser = new User
            {
                FirstName = sourceUser.FirstName,
                LastName = sourceUser.LastName,
                Id = sourceUser.Id + 1,
                UserName = sourceUser.UserName,
                Password = sourceUser.Password,
            };

            _userRepository.CreateUser(copiedUser);
        }
    }
}
