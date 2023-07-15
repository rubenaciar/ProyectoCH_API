using ProyectoFinalCoderHouse.Exceptions;
using System;
using System.Collections.Generic;

namespace ProyectoFinalCoderHouse.Models
{
    //CLASE USUARIO
    public class Usuario
    {
        #region Atributos
        private long _id;
        private string _nombre;
        private string _apellido;
        private string _nombreUsuario;
        private string _contraseña;
        private string _mail;
        
        #endregion

        #region Constructores
        public Usuario(long id, string nombre, string apellido, string nombreUsuario, string contraseña, string mail)
        {
            // Validacion de parametros
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede ser nulo o vacio.");
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                throw new ArgumentException("El apellido no puede ser nulo o vacio.");
            }

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacio.");
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                throw new ArgumentException("La contrasena no puede ser nula o vacia.");
            }

            if (!IsValidoEmail(mail))
            {
                throw new ArgumentException("El formato del correo electronico no es valido.");
            }


            // Inicializar los atributos privados con los parámetros ingresados
            _id = id;
            _nombre = nombre;
            _apellido = apellido;
            _nombreUsuario = nombreUsuario;
            _contraseña = contraseña;
            _mail = mail;
            // Mensaje de creacion de la instancia de Usuario
            Console.WriteLine("La instancia de Usuario se ha creado con constuctor con argumentos.");
        }
        // Constructor sin argumentos para inicializar por defecto los atributos privados
        public Usuario()
        {

            _id = 0;
            _nombre = string.Empty;
            _apellido = string.Empty;
            _nombreUsuario = string.Empty;
            _contraseña = string.Empty;
            _mail = string.Empty;
           


            // Mensaje de creacion de la instancia de Usuario
            Console.WriteLine("La instancia de Usuario se ha creado con constuctor sin argumentos.");
        }

        #endregion

        #region Propiedades
        // Propiedades publicas de la clase, para poder acceder a sus Atributos privados

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }

        public string NombreUsuario
        {
            get { return _nombreUsuario; }
            set
            {
                // Validacion del nombre de usuario
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.");
                }

                _nombreUsuario = value;
            }
        }

        public string Contraseña
        {
            get { return _contraseña; }
            set
            {
                const int iLargoContraseña = 8;
                if (!IsValidaContraseña(value, iLargoContraseña))
                {
                    throw new ContraseñaInvalidaException("Contraseña inválida: debe tener un mínimo de 8 caracteres e incluir: 0-9, A-Z, a-z y un carácter especial.");
                }

                _contraseña = value;
            }
        }


      
        public string Mail
        {
            get { return _mail; }
            set
            {
                // Validacion del correo electronico
                if (!IsValidoEmail(value))
                {
                    throw new ArgumentException("El formato del correo electronico no es valido.");
                }

                _mail = value;
            }
        }
        public bool EsValido { get; set; }
        
        public string MensajeLogin { get; set; }


        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

        public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();


        #endregion



        #region Metodos


        ///Método privado para validar una contraseña segura
        public static bool IsValidaContraseña(string sPassword, int iLongitud)
        {
            bool bMayuscula = false, bMinuscula = false, bNumerica = false, bCarEspecial = false;

            for (int i = 0; i < sPassword.Length; i++)
            {
                if (char.IsUpper(sPassword, i))
                {
                    bMayuscula = true;
                }
                else if (char.IsLower(sPassword, i))
                {
                    bMinuscula = true;
                }
                else if (char.IsDigit(sPassword, i))
                {
                    bNumerica = true;
                }
                else if (!char.IsLetter(sPassword, i) && !char.IsWhiteSpace(sPassword, i))
                {
                    bCarEspecial = true;
                }
            }
            if (bMayuscula && bMinuscula && bNumerica && bCarEspecial && sPassword.Length >= iLongitud)
            {
                return true;
            }
            return false;
        }

        // Método privado para validar el formato del correo electrónico.
        // Retorna true si el correo es válido y false si no lo es.
        private static bool IsValidoEmail(string email)
        {
            try
            {
                // Creamos una instancia de la clase MailAddress, pasando el correo electrónico como parámetro en su constructor. 
                // La clase MailAddress se encuentra en el espacio de nombres System.Net.Mail.
                var correo = new System.Net.Mail.MailAddress(email);

                // Comparamos el valor de la propiedad Address de la instancia (correo) y el valor de la variable email para asegurarnos
                // de que no hubo ninguna modificación en la dirección de correo electrónico durante la instancia de MailAddress.
                return correo.Address == email; //retorna booleano
            }
            catch (FormatException)
            {
                // Si se produce una excepción de formato al crear la instancia de MailAddress, significa que el correo electrónico es inválido.
                return false;
            }
            catch (Exception ex)
            {
                // Si se produce cualquier otra excepción al crear la instancia de MailAddress, se lanza una excepción y se imprime un mensaje de error.
                throw new Exception($"Se produjo una excepción al validar el correo electrónico: {ex.Message}");
            }
        }
        #endregion
    }
}
