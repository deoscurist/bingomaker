using System;
using System.IO;
using BingoMaker.Data.Repositories;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace BingoMaker.Data;

public static class DBConnection
{
    public static IServiceCollection AddDBConnection(this IServiceCollection services)
    {
        services.AddSingleton<ILiteDatabase>(_ =>
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "BingoMaker");
            
            Directory.CreateDirectory(directory);

            var connection = new ConnectionString
            {
                Filename = Path.Combine(directory, "bingomaker.db"),
                Connection = ConnectionType.Direct
            };
            
            return new LiteDatabase(connection);
        });
        
        services.AddSingleton<IBingoRepository, BingoRepository>();
        services.AddSingleton<IGameRepository, GameRepository>();
        
        return services;
    }
}