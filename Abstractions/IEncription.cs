namespace webapp
{
    public interface IEncription
    {
        string CreateValueHash(string password);
        bool VerifyValueHash(string value, byte[] valueHash);
    }
}