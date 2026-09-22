using System;
using System.Collections.Generic;
using BingoMaker.Models;

namespace BingoMaker.Data.Repositories;

public interface IGameRepository
{
    IReadOnlyList<Game> GetAll();
    Game? GetById(Guid id);
    bool Save(Game game);
    bool Delete(Game game);
}