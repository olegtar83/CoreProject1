namespace webapp
{
    public interface IEncription
    {
        void CreateValueHash(string password, out byte[] valueHash);
        bool VerifyValueHash(string value, byte[] valueHash);
    }
}