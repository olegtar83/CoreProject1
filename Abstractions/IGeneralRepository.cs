using System.Threading.Tasks;

namespace webapp
{
    public interface IGeneralRepository<T>
    {
        Task<int> GetNextAutoincrementValue();
    }
}