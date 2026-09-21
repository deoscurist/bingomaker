using System;
using System.Collections.Generic;
using BingoMaker.Models;

namespace BingoMaker.Data.Repositories;

public interface IBingoRepository
{
    IReadOnlyList<Bingo> GetAll();
    Bingo? GetById(Guid id);
    bool Save(Bingo bingo);
    bool Delete(Bingo bingo);
}