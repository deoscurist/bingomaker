using System;
using System.Collections.Generic;
using BingoMaker.Data.Repositories;
using BingoMaker.Models;

namespace BingoMaker.Design.Data.Repositories;

public class DesignGameRepository : IGameRepository
{
    public List<Game> Games { get; }
    
    public DesignGameRepository(IEnumerable<Game> games)
    {
        Games = new List<Game>(games);
    }
    
    public IReadOnlyList<Game> GetAll() => Games;

    public IReadOnlyCollection<Game> GetAllByBingoId() => Games;

    public Game? GetById(Guid id) => Games[1];

    public bool Save(Game game) => true;

    public bool Delete(Game game) => true;
}