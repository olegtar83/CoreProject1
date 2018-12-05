namespace webapp
{
    public interface IEncription
    {
        void CreateValueHash(string password, out string valueHash);
        bool VerifyValueHash(string value, byte[] valueHash);
    }
}