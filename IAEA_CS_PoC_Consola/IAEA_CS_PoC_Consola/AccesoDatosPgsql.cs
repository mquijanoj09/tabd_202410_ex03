using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
namespace IAEA_CS_PoC_Consola
{
    public class AccesoDatosPgsql
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();
            return miConfiguracion["ConnectionString:ReactoresPL"];
        }

        #region Reactores
        /// <summary>
        /// Obtiene la lista con los nombres de las reactores
        /// </summary>
        /// <returns>Lista con los nombres de las reactores</returns>
        public static List<string> ObtieneNombresReactores()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            string sentenciaSQL = "SELECT nombre FROM reactores ORDER BY nombre";
            var resultadoReactores = cxnDB.Query<string>(sentenciaSQL, new DynamicParameters());

            return resultadoReactores.AsList();
        }

        /// <summary>
        /// Obtiene la lista con las reactores del atlas
        /// </summary>
        /// <returns></returns>
        public static List<Reactor> ObtieneListaReactores()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen FROM reactores ORDER BY nombre";
            var resultadoReactores = cxnDB.Query<Reactor>(sentenciaSQL, new DynamicParameters());

            return resultadoReactores.AsList();
        }

        /// <summary>
        /// Obtiene una reactor según el Id
        /// </summary>
        /// <param name="idReactor">ID de la reactor a buscar</param>
        /// <returns>La reactor identificada según el parámetro</returns>
        public static Reactor ObtieneReactor(int idReactor)
        {
            Reactor reactorResultado = new();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos la reactor asociada al nombre
            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@id_reactor", idReactor,
                                    DbType.Int32, ParameterDirection.Input);

            string? sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                    "FROM reactores " +
                                    "WHERE id = @id_reactor";

            var salida = cxnDB.Query<Reactor>(sentenciaSQL, parametrosSentencia);

            if (salida.Any())
                reactorResultado = salida.First();

            return reactorResultado;
        }

        /// <summary>
        /// Obtiene una reactor según el nombre
        /// </summary>
        /// <param name="nombreReactor">Nombre de la reactor a buscar</param>
        /// <returns>La reactor identificada según el parámetro</returns>
        public static Reactor ObtieneReactor(string nombreReactor)
        {
            Reactor reactorResultado = new();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos la reactor asociada al nombre
            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre_reactor", nombreReactor,
                                    DbType.String, ParameterDirection.Input);

            string? sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                    "FROM reactores " +
                                    "WHERE nombre = @nombre_reactor";

            var salida = cxnDB.Query<Reactor>(sentenciaSQL, parametrosSentencia);

            if (salida.Any())
                reactorResultado = salida.First();

            return reactorResultado;
        }

        /// <summary>
        /// Inserta una reactor
        /// </summary>
        /// <param name="unaReactor">La reactor a insertar</param>
        /// <returns>Verdadero si la inserción se hizo correctamente</returns>
        public static bool InsertaReactor(Reactor unaReactor)
        {
            //Validaciones previas: 
            // - Que la reactor no exista previamente

            int cantidadFilas;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@nombre_reactor", unaReactor.Nombre,
                    DbType.String, ParameterDirection.Input);

                //Preguntamos si ya existe una reactor con ese nombre
                string consultaSQL = "SELECT COUNT(id) total " +
                                           "FROM reactores " +
                                           "WHERE LOWER(nombre) = LOWER(@nombre_reactor)";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                // Si hay filas, ya existe una reactor con ese nombre
                if (cantidadFilas != 0)
                    return false;

                try
                {
                    string insertaReactorSQL = "INSERT INTO reactores (nombre, url_wikipedia, url_imagen) " +
                                               "VALUES (@Nombre, @Url_Wikipedia, @Url_Imagen)";

                    cantidadFilas = cxnDB.Execute(insertaReactorSQL, unaReactor);
                }
                catch (NpgsqlException)
                {
                    resultado = false;
                    cantidadFilas = 0;
                }

                //Si la inserción fue correcta, se afectaron filas y podemos retornar true.
                if (cantidadFilas > 0)
                    resultado = true;

            }

            return resultado;
        }

        /// <summary>
        /// Actualiza la información básica de una reactor
        /// </summary>
        /// <param name="reactorActualizada">El objeto reactor para actualizar</param>
        /// <returns>Verdadero si la actualización se hizo correctamente</returns>
        public static bool ActualizaReactor(Reactor reactorActualizada)
        {
            //Validaciones previas: 
            // - Que la a actualizar exista - Busqueda por ID
            // - Que el nombre nuevo no exista previamente

            int cantidadFilas;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();


            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                //Aqui validamos primero que la reactor previamente existe

                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@reactor_id", reactorActualizada.Id,
                                        DbType.Int32, ParameterDirection.Input);

                string consultaSQL = "SELECT COUNT(id) total " +
                                     "FROM reactores " +
                                     "WHERE id = @reactor_id";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe reactor que actualizar
                if (cantidadFilas == 0)
                    return false;

                //Aqui validamos que no exista reactores con el nuevo nombre
                parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@reactor_nombre", reactorActualizada.Nombre,
                                        DbType.String, ParameterDirection.Input);

                //Validamos si el nuevo nombre no exista
                consultaSQL = "SELECT COUNT(id) total " +
                              "FROM reactores " +
                              "WHERE nombre = @reactor_nombre";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si hay filas, el nuevo nombre a utilizar ya existe!
                if (cantidadFilas != 0)
                    return false;

                //Terminadas las validaciones, realizamos el update
                try
                {
                    string actualizaReactoresSql = "UPDATE reactores SET nombre = @Nombre, url_wikipedia = @Url_Wikipedia, url_imagen = @Url_Imagen " +
                        "WHERE id = @Id"; ;

                    //Aqui no usamos parámetros dinámicos, pasamos el objeto!!!
                    cantidadFilas = cxnDB.Execute(actualizaReactoresSql, reactorActualizada);
                }
                catch (NpgsqlException)
                {
                    resultado = false;
                    cantidadFilas = 0;
                }

                //Si la actualización fue correcta, devolvemos true
                if (cantidadFilas > 0)
                    resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Elimina una reactor existente
        /// </summary>
        /// <param name="unaReactor">Objeto reactor a eliminar</param>
        /// <returns>Verdadero si la eliminación se hizo correctamente</returns>
        public static bool EliminaReactor(Reactor unaReactor, out string mensajeEliminacion)
        {
            //Validaciones previas: 
            // - Que la reactor a actualizar exista - Busqueda por ID

            mensajeEliminacion = string.Empty;
            int cantidadFilas;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            using (NpgsqlConnection cxnDB = new(cadenaConexion))
            {
                //Primero, identificamos si hay una reactor con este nombre y ese Id

                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@reactor_nombre", unaReactor.Nombre,
                                        DbType.String, ParameterDirection.Input);

                parametrosSentencia.Add("@reactor_id", unaReactor.Id,
                                        DbType.Int32, ParameterDirection.Input);

                string consultaSQL = "SELECT COUNT(id) total " +
                                     "FROM reactores " +
                                     "WHERE nombre = @reactor_nombre and id = @reactor_id";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe una reactor con ese nombre y ese Id ... no hay nada que eliminar.
                if (cantidadFilas == 0)
                {
                    mensajeEliminacion = $"Eliminación Fallida. No existe una reactor con el nombre {unaReactor.Nombre}.";
                    return false;
                }

                //Pasadas las validaciones, borramos la reactor
                try
                {
                    string eliminaReactorSQL = "DELETE FROM reactores " +
                                             "WHERE id = @Id";

                    //Aqui no usamos parámetros dinámicos, pasamos el objeto!!!
                    cantidadFilas = cxnDB.Execute(eliminaReactorSQL, unaReactor);
                    resultado = true;
                    mensajeEliminacion = "Eliminación exitosa!";

                }
                catch (NpgsqlException elError)
                {
                    resultado = false;
                    mensajeEliminacion = $"Error de borrado en la DB. {elError.Message}";
                }
            }

            return resultado;
        }
        #endregion Reactores
    }
}