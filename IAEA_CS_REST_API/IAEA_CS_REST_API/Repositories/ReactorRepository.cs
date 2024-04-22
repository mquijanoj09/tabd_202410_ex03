using Dapper;
using IAEA_CS_REST_API.DbContexts;
using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Repositories
{
    public class ReactorRepository(PgsqlDbContext unContexto) : IReactorRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Reactor>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "ORDER BY id DESC";

            var resultadoEstilos = await conexion
                .QueryAsync<Reactor>(sentenciaSQL, new DynamicParameters());

            return resultadoEstilos;
        }

        public async Task<Reactor> GetByIdAsync(int fruta_id)
        {
            Reactor unaReactor = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id, System.Data.DbType.Int32,  System.Data.ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "WHERE id = @fruta_id " +
                                  "ORDER BY nombre";

            var resultado = await conexion.QueryAsync<Reactor>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaReactor = resultado.First();

            return unaReactor;
        }
    }
}