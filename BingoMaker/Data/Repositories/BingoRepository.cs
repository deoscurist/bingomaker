using System;
using System.Collections.Generic;
using System.Linq;
using BingoMaker.Models;
using LiteDB;

namespace BingoMaker.Data.Repositories;

public class BingoRepository(ILiteDatabase database) : IBingoRepository
{
    private readonly ILiteCollection<Bingo> _collection = database.GetCollection<Bingo>("bingos");

    public IReadOnlyList<Bingo> GetAll()
    {
        return _collection.FindAll().ToList();
    }

    public Bingo? GetById(Guid id)
    {
        return _collection.FindById(id);
    }

    public bool Save(Bingo bingo)
    {
        ArgumentNullException.ThrowIfNull(bingo);
        
        _collection.Upsert(bingo);
        
        return true;
    }

    public bool Delete(Bingo bingo)
    {
        ArgumentNullException.ThrowIfNull(bingo);
        
        return _collection.Delete(bingo.Id);
    }
}