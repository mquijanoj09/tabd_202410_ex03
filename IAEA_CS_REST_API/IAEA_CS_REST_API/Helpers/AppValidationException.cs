﻿/*
AppValidationException:
Excepcion creada para enviar mensajes relacionados 
con la validación en todas las operaciones CRUD de la aplicación
*/

namespace IAEA_CS_REST_API.Helpers
{
    public class AppValidationException : Exception
    {
        public AppValidationException(string message) : base(message) { }
    }
}
