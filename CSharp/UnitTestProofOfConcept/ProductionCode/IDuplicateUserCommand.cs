namespace ProductionCode
{
    public interface IDuplicateUserCommand
    {
        void Execute(int userId);
    }
}