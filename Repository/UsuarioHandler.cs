using Microsoft.Extensions.Configuration;
using ProyectoFinalCoderHouse.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ProyectoFinalCoderHouse.Repository
{
    public class UsuarioHandler
    {
        private static readonly string _connectionString;
        static UsuarioHandler()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("connectionDB");
        }

        // Método que recibe como parámetro un NombreUsuario y una Contraseña. Debe buscar en la base de datos si el Usuario existe y si posee la misma Contraseña lo devuelve, caso contrario devuelve error.
        public Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            Usuario usuario = new Usuario();

            // Verifico si los argumentos son validos
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseña)) // Si nombreUsuario o contraseña están vacíos o son null no ejecuto el método y devuelvo usuario vacío.
            {
                return usuario; // Devuelvo un Usuario inicializado por defecto.
            }

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE (NombreUsuario = @nombreUsuario AND Contraseña = @contraseña)", sqlConnection))
                {
                    var sqlParameter1 = new SqlParameter();
                    sqlParameter1.ParameterName = "nombreUsuario";
                    sqlParameter1.SqlDbType = SqlDbType.VarChar;
                    sqlParameter1.Value = nombreUsuario;
                    sqlCommand.Parameters.Add(sqlParameter1);

                    var sqlParameter2 = new SqlParameter();
                    sqlParameter2.ParameterName = "contraseña";
                    sqlParameter2.SqlDbType = SqlDbType.VarChar;
                    sqlParameter2.Value = contraseña;
                    sqlCommand.Parameters.Add(sqlParameter2);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows & dataReader.Read())
                        {
                            usuario.Id = Convert.ToInt32(dataReader["Id"]);
                            usuario.Nombre = dataReader["Nombre"].ToString();
                            usuario.Apellido = dataReader["Apellido"].ToString();
                            usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.Contraseña = dataReader["Contraseña"].ToString();
                            usuario.Mail = dataReader["Mail"].ToString();
                            usuario.EsValido = true;
                            usuario.MensajeLogin = "Ingreso al sistema con Usuario Correcto";
                            Console.WriteLine(usuario.MensajeLogin);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            
            return usuario;
        }

        // Método para modificar un usuario existente en la Base de Datos
        // Recibe un objeto Usuario con la información actualizada del usuario a modificar
        // Devuelve true si la modificación fue exitosa, false si no
        public bool ModificarUsuario(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Definimos la consulta SQL que vamos a ejecutar
                const string query = @"UPDATE Usuario SET Nombre = @Nombre, Apellido = @Apellido, 
                                   NombreUsuario = @NombreUsuario, Contraseña = @Contraseña, Mail = @Mail 
                                   WHERE Id = @Id";
                // Creamos una nueva instancia de SqlCommand con la consulta SQL y la conexión asociada
                using (var command = new SqlCommand(query, connection))
                {
                    // Agregamos los parámetros correspondientes a la consulta SQL utilizando el objeto Usuario recibido como parámetro
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                    command.Parameters.AddWithValue("@Mail", usuario.Mail);
                    command.Parameters.AddWithValue("@Id", usuario.Id);

                    // Ejecutamos la consulta SQL utilizando ExecuteNonQuery() que retorna la cantidad de filas afectadas por la consulta SQL
                    // En este caso, debería ser 1 si se modificó el usuario correctamente, o 0 si no se encontró el usuario con el Id correspondiente
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Usuario> TraerListaUsuarios()
        {

            //string connectionString = @"Server=P533750\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True;";
            var query = "SELECT Id, Nombre, Apellido, NombreUsuario, Contraseña, Mail FROM Usuario";
            List<Usuario> listaUsuarios = new List<Usuario>();

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
                                var usuario = new Usuario();
                                usuario.Id = (int)dr.GetInt64(0);
                                usuario.Nombre = dr.GetString(1);
                                usuario.Apellido = dr.GetString(2);
                                usuario.NombreUsuario = dr.GetString(3);
                                usuario.Contraseña = dr.GetString(4);
                                usuario.Mail = dr.GetString(5);
                                listaUsuarios.Add(usuario);
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
            return listaUsuarios;

        }

        
        // Método que recibe un Id de Usuario y debe buscarlo en la base de datos para devolver todos sus atributos.
        public Usuario TraerUsuario_conId(long id)
        {
            Usuario usuario = new Usuario();

            // Verifico si el argumento pasado es válido
            if (id <= 0)
            {
                return usuario; // El id no puede ser cero o negativo. Devuelvo un objeto Usuario inicializado por defecto
            }

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [SistemaGestion].[dbo].[Usuario] WHERE Id = @id", sqlConnection))
                {
                    var sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "id";
                    sqlParameter.SqlDbType = SqlDbType.VarChar;
                    sqlParameter.Value = id;
                    sqlCommand.Parameters.Add(sqlParameter);

                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows & dataReader.Read())
                        {
                            usuario.Id = Convert.ToInt32(dataReader["Id"]);
                            usuario.Nombre = dataReader["Nombre"].ToString();
                            usuario.Apellido = dataReader["Apellido"].ToString();
                            usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                            usuario.Contraseña = dataReader["Contraseña"].ToString();
                            usuario.Mail = dataReader["Mail"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return usuario;
        }
    }
}
