namespace ConfigRemedy.Security
{
    /// <summary>
    /// Used to create safe strings 
    /// </summary>
    public interface IPasswordHasher
    {
        string CreateHash(string password);
        bool ValidatePassword(string password, string correctHash);
    }
}