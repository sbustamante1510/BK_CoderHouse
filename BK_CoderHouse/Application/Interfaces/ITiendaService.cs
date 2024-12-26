using BK_CoderHouse.Domain.Entities;

namespace BK_CoderHouse.Application.Interfaces;

public interface ITiendaService
{
    Task<List<TiendaEntity>> GetTienda(string payload);
}
