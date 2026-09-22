using System;
using System.Collections.Generic;
using BingoMaker.Data.Repositories;
using BingoMaker.Models;

namespace BingoMaker.Design.Data.Repositories;

public class DesignBingoRepository : IBingoRepository
{
    public List<Bingo> Bingos { get; }
    
    public DesignBingoRepository(IEnumerable<Bingo> bingos)
    {
        Bingos = new List<Bingo>(bingos);
    }
    
    public IReadOnlyList<Bingo> GetAll() => Bingos;

    public Bingo? GetById(Guid id) => Bingos[1];

    public bool Save(Bingo bingo) => true;

    public bool Delete(Bingo bingo) => true;
}