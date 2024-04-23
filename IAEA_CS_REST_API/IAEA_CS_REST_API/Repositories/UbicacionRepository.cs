using System.Data;
using Dapper;
using IAEA_CS_REST_API.DbContexts;
using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;
using IAEA_CS_REST_API.Helpers;
using Npgsql;

namespace IAEA_CS_REST_API.Repositories
{
    public class UbicacionRepository(PgsqlDbContext unContexto) : IUbicacionRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Ubicacion>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM core.Ciudades " +
                                  "ORDER BY id DESC";

            var resultadoUbicaciones = await conexion
                .QueryAsync<Ubicacion>(sentenciaSQL, new DynamicParameters());

            return resultadoUbicaciones;
        }

        public async Task<Ubicacion> GetByIdAsync(int ubicacion_id)
        {
            Ubicacion unaUbicacion = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM core.Ciudades " +
                                  "WHERE id = @ubicacion_id " +
                                  "ORDER BY nombre";

            var resultado = await conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaUbicacion = resultado.First();

            return unaUbicacion;
        }

        public async Task<Ubicacion> GetReactoresUbicacionAsync(int ubicacion_id)
        {
            Ubicacion reactoresUbicacion = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@ubicacion_id", ubicacion_id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM core.Reactores " +
                                  "WHERE id = @ubicacion_id " +
                                  "ORDER BY nombre";

            var resultado = await conexion.QueryAsync<Ubicacion>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                reactoresUbicacion = resultado.First();

            return reactoresUbicacion;
        }

    }
}