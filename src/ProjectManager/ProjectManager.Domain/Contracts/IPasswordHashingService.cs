namespace ProjectManager.Domain.Contracts
{
    public interface IPasswordHashingService
    {
        string GetHashedPassword(string password);
    }
}
