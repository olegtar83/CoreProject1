namespace webapp
{
    public interface IEncryption
    {
        string CreateValueHash(string password);
        bool VerifyValueHash(string value, byte[] valueHash);
    }
}