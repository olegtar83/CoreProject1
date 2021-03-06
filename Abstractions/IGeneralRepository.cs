﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace webapp.Abstractions
{
    public interface IGeneralRepository<T>
    {
        Task<int> GetNextAutoincrementValue();

        Task<bool> RemoveDocument(string id);

        Task<T> GetDocument(int id);

        Task<bool> RemoveAllDocuments();

        Task<IEnumerable<T>> GetAllDocuments();

        Task AddDocument(T item);

        Task<bool> UpdateDocument(T item, int id);

        Task<bool> UpdateDocument(T item, FilterDefinition<T> filter, UpdateDefinition<T> update);

        ObjectId GetInternalId(int id);
    }
}