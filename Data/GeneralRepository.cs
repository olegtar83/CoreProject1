using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Services.Abstractions;

namespace webapp
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T: BaseModel
    {
        private IMongoContext<T> _context;
        public GeneralRepository(IMongoContext<T> context )
        {
            this._context = context;
        }

        public async Task<int> GetNextAutoincrementValue()
        {
            var res = await _context.Documents.Find(x=>true).SortByDescending(s=>s.Id).Limit(1).FirstOrDefaultAsync();
            return res.Id+1;
                
        }

    }
}
