using System;
using System.Collections.Generic;
using BingoMaker.Models;

namespace BingoMaker.Data.Repositories;

public interface IGameRepository
{
    IReadOnlyList<Game> GetAll();
    public IReadOnlyCollection<Game> GetAllByBingoId(Guid bingoId);
    Game? GetById(Guid id);
    bool Save(Game game);
    bool Delete(Game game);
}