using Microsoft.Extensions.Configuration;
using ProyectoFinalCoderHouse.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ProyectoFinalCoderHouse.Repository
{
    public class ProductoHandler
    {
        private static readonly string _connectionString;
        static ProductoHandler()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("connectionDB");
        }



        // Método para insertar un nuevo Producto en la Base de Datos
        // Recibe un objeto Producto con la información del Producto a crear
        // Devuelve el Id asignado al nuevo registro
        public int CrearProducto(Producto producto)
        {
            // Creamos una nueva conexión a la base de datos utilizando el string de conexión que se recibió en el constructor
            using (var connection = new SqlConnection(_connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Definimos la consulta SQL que vamos a ejecutar
                const string query = @"INSERT INTO Producto (Descripciones,Costo,PrecioVenta,Stock,IdUsuario) 
                                   VALUES (@Descripciones,@Costo,@PrecioVenta,@Stock,@IdUsuario);
                                   SELECT SCOPE_IDENTITY();";
                // Creamos una nueva instancia de SqlCommand con la consulta SQL y la conexión asociada
                using (var command = new SqlCommand(query, connection))
                {
                    // Agregamos los parámetros correspondientes a la consulta SQL utilizando el objeto Producto recibido como parámetro
                    command.Parameters.AddWithValue("@Descripciones", producto.Descripciones);
                    command.Parameters.AddWithValue("@Costo", producto.Costo);
                    command.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                    command.Parameters.AddWithValue("@Stock", producto.Stock);
                    command.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);

                    // Ejecutamos la consulta SQL utilizando ExecuteScalar() que retorna el id generado para nuevo registro insertado
                    return (int)(decimal)command.ExecuteScalar();
                }
            }
        }

        // Método para eliminar un producto de la Base de Datos según su Id
        // Recibe el Id del producto que se desea eliminar
        // Devuelve true si la eliminación fue exitosa, false si no
        public bool EliminarProducto(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Definimos la consulta SQL que vamos a ejecutar
                const string query = @"DELETE FROM Producto WHERE Id = @Id";
                // Creamos una nueva instancia de SqlCommand con la consulta SQL y la conexión asociada
                using (var command = new SqlCommand(query, connection))
                {
                    // Agregamos el parámetro correspondiente a la consulta SQL utilizando el Id recibido como parámetro
                    command.Parameters.AddWithValue("@Id", id);

                    // Ejecutamos la consulta SQL utilizando ExecuteNonQuery() que retorna la cantidad de filas afectadas por la consulta SQL
                    // En este caso, debería ser 1 si se eliminó el producto correctamente, o 0 si no se encontró el producto con el Id correspondiente
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // Método para modificar un producto existente en la Base de Datos
        // Recibe un objeto producto con la información actualizada del producto a modificar
        // Devuelve true si la modificación fue exitosa, false si no

        public bool ModificarProducto(Producto producto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Definimos la consulta SQL que vamos a ejecutar
                const string query = @"UPDATE Producto SET Descripciones = @Descripciones,Costo = @Costo, PrecioVenta = @PrecioVenta,
                                   Stock = @Stock, IDUsuario = @IdUsuario
                                   WHERE Id = @Id";
                // Creamos una nueva instancia de SqlCommand con la consulta SQL y la conexión asociada
                using (var command = new SqlCommand(query, connection))
                {
                    // Agregamos los parámetros correspondientes a la consulta SQL utilizando el objeto Producto recibido como parámetro
                    command.Parameters.AddWithValue("@Descripciones", producto.Descripciones);
                    command.Parameters.AddWithValue("@Costo", producto.Costo);
                    command.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                    command.Parameters.AddWithValue("@Stock", producto.Stock);
                    command.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);
                    command.Parameters.AddWithValue("@Id", producto.Id);


                    // Ejecutamos la consulta SQL utilizando ExecuteNonQuery() que retorna la cantidad de filas afectadas por la consulta SQL
                    // En este caso, debería ser 1 si se modificó el producto correctamente, o 0 si no se encontró el producto con el Id correspondiente
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Producto> TraerListaProductos()
        {

        
            var query = "SELECT Id,Descripciones,Costo,PrecioVenta,Stock,IdUsuario FROM Producto";
            List<Producto> listaProductos = new List<Producto>();

            //Creamos una instancia de conexión utilizando el string a nuestra BD, usando using para limpiar los recursos

            using (SqlConnection conect = new SqlConnection(_connectionString))
            {
                // Abrimos nuestra conexion a la BD
                conect.Open();
                // Enviamos el comando que necesitamos ejecutar en nuestra BD para la conexión creada
                using (SqlCommand comando = new SqlCommand(query, conect))
                {
                    // La BD devuelve la información a través de un objeto datareader
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        // Verifico que el la consulta trajo datos
                        if (dr.HasRows)
                        {

                            // Mientras la conexión está abierta, el datareader se puede leer, almacenar el producto y agregarlo a lista
                            while (dr.Read())
                            {
                                var producto = new Producto();
                                producto.Id = dr.GetInt64("Id");
                                producto.Descripciones = dr.GetString("Descripciones");
                                producto.Costo = dr.GetDecimal("Costo");
                                producto.PrecioVenta = dr.GetDecimal("PrecioVenta");
                                producto.Stock = dr.GetInt32("Stock");
                                producto.IdUsuario = dr.GetInt64("IdUsuario");
                                listaProductos.Add(producto);
                            }
                        }
                        else
                        {
                            // Cerramos nuestra conexion a la BD
                            conect.Close();
                            return null;
                        }

                    }
                }
                // Cerramos nuestra conexion a la BD
                conect.Close();
            }
            return listaProductos;

        }

       
        // Método para traer la lista de Productos que fueron ingresador por un IDUsuario

        public List<Producto> TraerProductosPorIdUsuario(long idUsuario)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"SELECT Prod.[Id]
                                                  ,Prod.[Descripciones]
                                                  ,Prod.[Costo]
                                                  ,Prod.[PrecioVenta]
                                                  ,Prod.[Stock]
                                                  ,Prod.[IdUsuario]
	                                              ,Usuario.Apellido
                                                  ,Usuario.Nombre
                                                  FROM [Producto] Prod
                                                  INNER JOIN [Usuario] ON Prod.[IdUsuario] = Usuario.[Id]
                                                  WHERE IdUsuario = @idUsuario;";

                    sqlCommand.Parameters.AddWithValue("@idUsuario", idUsuario);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); //Se ejecuta el Select
                    sqlCommand.Connection.Close();
                    foreach (DataRow row in table.Rows)
                    {
                        Producto producto = new Producto();
                        producto.Id = Convert.ToInt64(row["Id"]);
                        producto.Descripciones = row["Descripciones"].ToString();
                        producto.Costo = Convert.ToDecimal(row["Costo"]);
                        producto.PrecioVenta = Convert.ToDecimal(row["PrecioVenta"]);
                        producto.Stock = Convert.ToInt32(row["Stock"]);
                        producto.IdUsuario = Convert.ToInt64(row["IdUsuario"]);

                        // Asignar el objeto Usuario a través de la propiedad IdUsuarioNavigation
                        producto.IdUsuarioNavigation = new Usuario()
                        {
                            Apellido = row["Apellido"].ToString(),
                            Nombre = row["Nombre"].ToString()
                        };

                        productos.Add(producto);
                    }

                }
            }
            return productos;
        }

        // Método que crea e inicializa un objeto de clase Producto con los valores provistos por un objeto SqlDataReader que previamente accedío a la BD.
        private Producto InicializarProductoDesdeBD(SqlDataReader dataReader)
        {
            Producto nuevoProducto = new Producto(
                                            Convert.ToInt64(dataReader["Id"]),
                                            dataReader["Descripciones"].ToString(),
                                            Convert.ToDecimal(dataReader["Costo"]),
                                            Convert.ToDecimal(dataReader["PrecioVenta"]),
                                            Convert.ToInt32(dataReader["Stock"]),
                                            Convert.ToInt64(dataReader["IdUsuario"])); // Utilizo el constructor de Producto con los atributos provistos por el objeto SqlDataReader para crear e inicializar un nuevo Producto.
            return nuevoProducto;
        }


        // Método que debe traer el producto cargado en la base cuyo Id = id
        public Producto TraerProductoPorID(long id)
        {
            Producto producto = new Producto(); // Creo un objeto de clase Producto. Va a ser lo que devuelva el método.

            // Valido que el argumento pasado al método sea válido
            if (id <= 0) // El Id debe ser mayor a 0 para que apunte a un producto valido.
            {
                return producto;
            }

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                const string queryGet = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id"; // Query que me permite seleccionar todas las columnas de la tabla Prodcuto de la fila cuyo Id = id

                using (SqlCommand sqlCommand = new SqlCommand(queryGet, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "queryGet" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    var sqlParameter = new SqlParameter();          // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en querySelect.
                    sqlParameter.ParameterName = "id";              // Asigno nombre al sqlParameter. Debe ser el mismo nombre utilizado en la query.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;      // Asigno el tipo de dato que tiene la columna correspondiente al parámetro (Id).
                    sqlParameter.Value = id;                        // Asigno el valor a sqlParameter. 
                    sqlCommand.Parameters.Add(sqlParameter);        // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD.

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader. Permite leer un flujo de filas de solo avance desde una base de datos de SQL Server.
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read()) // .HasRows: Me aseguro que haya filas para leer // .Read(): desplazo SqlDataReader al siguiente registro. Se debe llamar antes de acceder a los datos.
                        {
                            producto = InicializarProductoDesdeBD(sqlDataReader); // Creo objeto producto que es inicializado por el método "InicializarProductoDesdeBD" con los atributos obtenidos por "sqlDataReader".
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return producto; // Devuelvo el objeto producto cuya Id = id encontrado en la BD.
        }

        // Método que devuelve el stock de un determinado producto en BD.
        public static Producto ConsultarStock(Producto producto)
        {
            // Valido que el argumento pasado al método sea válido
            if (producto.Id <= 0)
            {
                return producto;
            }

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                const string querySelect = "SELECT * FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @id"; // Query que selecciona todas las columnas de la tabla Producto de las filas con Id = id.

                using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "querySelect" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    var sqlParameter = new SqlParameter();      // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en querySelect.
                    sqlParameter.ParameterName = "id";          // Asigno nombre al sqlParameter. Debe ser el mismo nombre utilizado en la query.
                    sqlParameter.SqlDbType = SqlDbType.BigInt;  // Asigno el tipo de dato que tiene la columna correspondiente al parámetro (Id).
                    sqlParameter.Value = producto.Id;           // Asigno el valor a sqlParameter. 
                    sqlCommand.Parameters.Add(sqlParameter);    // Agrego sqlParameter a la lista de parametros del objeto SqlCommand creado.

                    sqlConnection.Open(); // Abro la conexión con la BD

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) // Creo objeto SqlDataReader. Permite leer un flujo de filas de solo avance desde una base de datos de SQL Server.
                    {
                        if (sqlDataReader.HasRows & sqlDataReader.Read()) // .HasRows: Me aseguro que haya filas para leer // .Read(): desplazo SqlDataReader al siguiente registro. Se debe llamar antes de acceder a los datos.
                        {
                            producto.Stock = Convert.ToInt32(sqlDataReader["Stock"]); // Actualizo el atributo Stock del objeto producto
                        }
                    }
                    sqlConnection.Close(); // Cierro la conexión con la BD.
                }
            }
            return producto; // Devuelvo el objeto de clase Producto con el atributo Stock actualizado.
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que devuelve que actualiza, en BD, el stock de un determinado producto.
        public static bool ActualizarStock(Producto producto)
        {
            // Valido que el argumento pasado al método sea válido
            if (producto.Id <= 0)
            {
                return false;
            }

            bool resultado = false; // Creo una variable tipo bool que va a indicar si se pudo o no actualizar el stock del Producto.
            int rowsAffected = 0;   // Variable que va a indicar la cantidad de filas que fueron afectadas al ejecutar la query en la BD. Se utiliza para validación del método.

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString)) // Creo un objeto de tipo SqlConnection con el connectionString que me permite acceder a mi BD. // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " + // Query que me permite actualizar la columna el Stock de la tabla Producto.
                                        "SET Stock = @stock " +
                                        "WHERE Id = @id";

                var parameterId = new SqlParameter("id", SqlDbType.BigInt); // Creo un nuevo objeto SqlParameter, para especificar "@id", declarado en queryUpdate. Defino el nombre del parámetro (que debe coincidir con el nombre utilizado en la query) y defino el tipo de dato de la columna del parámetro.
                parameterId.Value = producto.Id;                            // Asigno valor al sqlParameter.

                var parameterStock = new SqlParameter("stock", SqlDbType.Int);
                parameterStock.Value = producto.Stock;

                sqlConnection.Open(); // Abro la conexión con la BD.

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection)) // Creo un objeto SqlCommand que va a ejecutar "queryUpdate" en la BD a la que se accede con "connectionString". // El "using" permite liberar los recursos declarados "(...)" para que no permanezcan en memoria más de lo necesario.
                {
                    sqlCommand.Parameters.Add(parameterId); // Agrego sqlParameter "parameterId" a la lista de parametros del objeto SqlCommand creado.
                    sqlCommand.Parameters.Add(parameterStock);
                    rowsAffected = sqlCommand.ExecuteNonQuery(); // Ejecuta una instrucción de Transact-SQL en la conexión y devuelve el número de filas afectadas, que son almacenadas en "rowsAffected"
                }
                sqlConnection.Close(); // Cierro la conexión con la BD.
            }
            if (rowsAffected == 1) // Si se afectó una fila de la BD quiere decir que el método tuvo exito.
            {
                resultado = true; // Devuelvo TRUE.
            }
            return resultado;
        }


    }

}

