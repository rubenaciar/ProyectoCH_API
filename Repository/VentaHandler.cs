using ProyectoFinalCoderHouse.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProyectoFinalCoderHouse.Repository
{
    
    public class VentaHandler
    {


        private static readonly string _connectionString;
        static VentaHandler()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("connectionDB");
        }


        // Método que trae todas las ventas de la BD que contienen productos de un determinado Usuario.
        // Método que trae todas los ProductoVendido de la BD que estén asociados a una venta.
        public List<ProductoVendido> TraerVentas()
        {

           
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                const string query = "SELECT pv.Id, pv.Stock, pv.IdProducto, pv.IdVenta " + // Query que me devuelve las ventas que contienen productos con IdUsuario = idUsuario.
                                        "FROM Venta AS v " +
                                        "INNER JOIN ProductoVendido AS pv " +
                                        "ON v.Id = pv.IdVenta ";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["Idventa"]);

                                productosVendidos.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidos;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        // Método que recibe como parámetro una Venta y debe cargarla en BD.
        public static int CrearVenta(Venta venta)
        {
            bool resultado = false;
            int idVenta = 0;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta] (Comentarios) " + // Query que me permite agregar una Venta.
                                        "VALUES (@comentarios) " +
                                        "SELECT @@IDENTITY";

                var parameterComentarios = new SqlParameter("comentarios", SqlDbType.VarChar);
                parameterComentarios.Value = venta.Comentarios;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parameterComentarios);
                    idVenta = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                sqlConnection.Close();
            }
            return idVenta;
        }

    }
}
