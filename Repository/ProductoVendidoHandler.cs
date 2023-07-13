using Microsoft.Extensions.Configuration;
using ProyectoFinalCoderHouse.Data;
using ProyectoFinalCoderHouse.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace ProyectoFinalCoderHouse.Repository
{
    public class ProductoVendidoHandler
    {
        private static readonly string _connectionString;
        static ProductoVendidoHandler()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("connectionDB");
        }

        // Traer lista de productos vendidos por ID de producto
        public IEnumerable<Producto> TraerProductosPorIdProducto(int idProducto)
        {
          
            using (var dbContext = new SistemaGestionContext())
            {
              
                var listaProductoVendidos = (from pv in dbContext.ProductoVendidos
                                             join p in dbContext.Productos on pv.IdProducto equals p.Id
                                             where pv.IdProducto == idProducto
                                             select new Producto()
                                             {
                                                 Id = pv.Id,
                                                 Stock = pv.Stock,
                                                 Descripciones = p.Descripciones,
                                                 PrecioVenta = p.PrecioVenta
                                             }).ToList();

                // Restaura la validación del certificado SSL al estado predeterminado
                
                return listaProductoVendidos;
            }

        }


        //Metodo para traer la lista de productos vendidos
        public List<ProductoVendido> TraerListaProductoVendidos()
        {

         
            var query = "SELECT Id,Stock,IdProducto,IdVenta FROM ProductoVendido";
            var listaProductoVendidos = new List<ProductoVendido>();

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

                            // Mientras la conexión está abierta, el datareader se puede leer, almacenar el ProductoVendido y agregarlo a lista
                            while (dr.Read())
                            {
                                var productoVendido = new ProductoVendido();
                                productoVendido.Id = (int)dr.GetInt64("Id");
                                productoVendido.Stock = dr.GetInt32("Stock");
                                productoVendido.IdProducto = (int)dr.GetInt64("IdProducto");
                                productoVendido.IdVenta = (int)dr.GetInt64("IdVenta");
                                listaProductoVendidos.Add(productoVendido);
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
            return listaProductoVendidos;

        }

        // Método que recibe una lista de objetos de clase ProductoVendido y debe cargar los mismos en la tabla ProductoVendido en BD. Se utiliza para poder ejecutar el segundo paso de CargarVenta().
        public bool CargarProductosVendidos(List<ProductoVendido> productosVendidos)
        {
            bool resultado = false;
            int idProductoVendido = 0;
            int elementosEnLaLista = 0;
            int idValidoEncontrado = 0;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido] (IdProducto, Stock, IdVenta) " + // Query que me permite agregar un ProductoVendido.
                                        "VALUES (@idProducto, @stock, @idventa) " +
                                        "SELECT @@IDENTITY";

                var parameterIdProducto = new SqlParameter("idProducto", SqlDbType.BigInt) { Value = 0 };
                var parameterStock = new SqlParameter("stock", SqlDbType.Int) { Value = 0 };
                var parameterIdUsuario = new SqlParameter("idVenta", SqlDbType.BigInt) { Value = 0 };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterIdProducto);
                    sqlCommand.Parameters.Add(parameterStock);
                    sqlCommand.Parameters.Add(parameterIdUsuario);

                    foreach (ProductoVendido item in productosVendidos)
                    {
                        // Por cada elemento en la lista productosVendidos se actualizan los parámetros de la query y se ejecuta la misma
                        parameterIdProducto.Value = item.IdProducto;
                        parameterStock.Value = item.Stock;
                        parameterIdUsuario.Value = item.IdVenta;
                        elementosEnLaLista++;
                        idProductoVendido = Convert.ToInt32(sqlCommand.ExecuteScalar());
                        if (idProductoVendido > 0) // Si el Id del objeto ProductoVendido insertado en la tabla es > 0 quiere decir que se inserto correctamente
                        {
                            idValidoEncontrado++;
                        }
                    }
                }
                sqlConnection.Close();
            }
            if (idValidoEncontrado == elementosEnLaLista) // Se valida que se hayan insertado tantas filas en BD como elementos había en la lista recibida por el método
            {
                resultado = true;
            }
            return resultado;
        }

    }

}
