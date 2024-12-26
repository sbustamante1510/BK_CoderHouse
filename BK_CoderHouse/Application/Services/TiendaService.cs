using BK_CoderHouse.Application.Interfaces;
using BK_CoderHouse.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Drawing;

namespace BK_CoderHouse.Application.Services;

public class TiendaService : ITiendaService
{

    private readonly string _connectionString;

    public TiendaService(IConfiguration _configuration)
    {
        _connectionString = _configuration.GetConnectionString("PostgresSQLConnection");
    }

    public async Task<List<TiendaEntity>> GetTienda(string payload)
    {
        var result = new List<TiendaEntity>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            string query = @"
                            SELECT 
                                id as id,
                                sku as SKU, 
                                stock as Stock, 
                                description as description, 
                                iddetail as idDetail, 
                                pictureurl as pictureUrl, 
                                prize as price,
                                title as title,
                                category as category
                            FROM coderhouse.tienda";

            if (!string.IsNullOrEmpty(payload))
            {
                query += $@" WHERE category = '{payload}'";
            }

            result = connection.Query<TiendaEntity>(query).ToList();
        }


        return result;
    }
}
