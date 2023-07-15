using ProyectoFinalCoderHouse.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using ProyectoFinalCoderHouse.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalCoderHouse.Controllers.DTOS;

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

        public bool CargarVenta(List<PostVenta> productosvendidos,long idUsuario)
        {
            try
            {
                using (var _context = new SistemaGestionContext())
                {
                    if (productosvendidos.Count == 0)
                    {
                        return false; // Manejo de error si la lista de productos está vacía
                    }


                    // Crea una nueva instancia de Venta con los datos necesarios
                    Venta venta = new Venta
                    {
                        Comentarios = "",
                        IdUsuario = idUsuario
                    };

                    // Agrega la venta al contexto
                    _context.Venta.Add(venta);

                    // Guarda la venta en la base de datos para generar su ID automáticamente
                    _context.SaveChanges();

                    foreach (var producto in productosvendidos)
                    {
                        Producto productoExistente = _context.Productos.Find(producto.IdProducto);

                        if (productoExistente != null)
                        {
                            ProductoVendido productoVendido = new ProductoVendido
                            {
                                IdProducto = producto.IdProducto,
                                IdVenta = venta.Id,
                                Stock = producto.Stock
                            };

                            // Agrega el producto vendido al contexto
                            _context.ProductoVendidos.Add(productoVendido);

                            // Actualiza el stock del producto en la tabla "Productos"
                            productoExistente.Stock -= producto.Stock;
                            _context.Entry(productoExistente).State = EntityState.Modified;
                        }
                        else
                        {
                            return false; // Manejo de error si el producto no se encuentra en la base de datos
                        }
                    }

                    // Asigna el valor generado automáticamente del campo Id al campo Comentarios
                    venta.Comentarios = $"Comentarios Venta {venta.Id}";

                    // Guarda los cambios en la base de datos
                    _context.SaveChanges();

                    return true;
                }
            }
            catch (Exception)
            {
                return false; // Manejo de error genérico
            }
        }

        // Método para eliminar un VENTA la Base de Datos según su Id
        // Recibe el Id del VENTA que se desea eliminar
        // Devuelve true si la eliminación fue exitosa, false si no
        public bool EliminarVenta(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Definimos la consulta SQL que vamos a ejecutar
                const string query = @"DELETE FROM Venta WHERE Id = @Id";
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

        // Traer lista de productos vendidos por ID de producto con LinQ
        public IEnumerable<VentaInfo> TraerVentasPorIdUsuario(long idUsuario)
        {
      
            using (var _dbContext = new SistemaGestionContext())
            {

                var listaVentas = (from u in _dbContext.Usuarios
                                    join v in _dbContext.Venta on u.Id equals v.IdUsuario

                                             where u.Id == idUsuario
                                             select new VentaInfo()
                                             {
                                                 Id = v.Id,
                                                 Comentarios = v.Comentarios,
                                                 IdUsuario = v.IdUsuario,
                                                 Usuario = v.IdUsuarioNavigation.Apellido + "," + v.IdUsuarioNavigation.Nombre
                                             }).ToList();         


                return listaVentas;
            }

        }

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

                                productoVendido.Id = Convert.ToInt64(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt64(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt64(dataReader["Idventa"]);

                                productosVendidos.Add(productoVendido);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return productosVendidos;
        }




    }
}
