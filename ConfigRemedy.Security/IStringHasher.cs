namespace ConfigRemedy.Security
{
    /// <summary>
    /// Used to create a quick hash that can be used for comparison.
    /// </summary>
    public interface IStringHasher
    {
        string CreateHash(string input);
        bool AreIdentical(string input, string hash);
    }
}