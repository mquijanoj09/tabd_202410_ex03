using Dapper;
using IAEA_CS_REST_API.DbContexts;
using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Repositories
{
    public class ResumenRepository(PgsqlDbContext unContexto) : IResumenRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<Resumen> GetAllAsync()
        {
            Resumen unResumen = new();

            var conexion = contextoDB.CreateConnection();

            //Total Ubicaciones
            string sentenciaSQL = "SELECT COUNT(id) total FROM core.frutas";
            unResumen.Frutas = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL = "SELECT COUNT(id) total FROM core.departamentos";
            unResumen.Departamentos = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return unResumen;
        }
    }
}