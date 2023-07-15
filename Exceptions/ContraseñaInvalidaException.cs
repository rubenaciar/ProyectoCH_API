using System;

namespace ProyectoFinalCoderHouse.Exceptions
{
    public class ContraseñaInvalidaException : Exception
    {
        public ContraseñaInvalidaException(string message) : base(message)
        {
        }
    }
}
