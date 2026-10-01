using System;
using System.Collections.Generic;
using System.Linq;
using BingoMaker.Models;
using LiteDB;

namespace BingoMaker.Data.Repositories;

public class GameRepository(ILiteDatabase database) : IGameRepository
{
    private readonly ILiteCollection<Game> _collection = database.GetCollection<Game>("games");
    
    public IReadOnlyList<Game> GetAll()
    {
        return _collection.FindAll().ToList();
    }
    
    public IReadOnlyCollection<Game> GetAllByBingoId(Guid bingoId)
    {
        return _collection.Find(x => x.BingoId == bingoId).ToList();
    }

    public Game? GetById(Guid id)
    {
        return _collection.FindById(id);
    }

    public bool Save(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);
        
        _collection.Upsert(game);
        
        return true;
    }

    public bool Delete(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);
        
        return _collection.Delete(game.Id);
    }
}