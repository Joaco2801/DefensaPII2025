using CrmUcu.Core;
using CrmUcu.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar reuniones con clientes.
    /// Permite programar reuniones con fecha, hora, duración y ubicación.
    /// Aplica Command Pattern y SRP.
    /// </summary>
    public class RegistrarReunionCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public RegistrarReunionCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para registrar una reunión con un cliente.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar reuniones.");
            
            if (args.Length < 5)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarReunion <idCliente> <fecha:dd/MM/yyyy-HH:mm> <duracionMin> <ubicacion> <descripcion>\n" +
                                     "Ejemplo: Kbr4 registrarReunion 1 15/12/2024-10:00 60 \"Oficina Central\" Presentación de propuesta");
            
            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");
            
            if (!DateTime.TryParseExact(args[1], "dd/MM/yyyy-HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
                return Task.FromResult("❌ Formato de fecha inválido. Usa: dd/MM/yyyy-HH:mm (ejemplo: 15/12/2024-10:00)");
            
            if (!int.TryParse(args[2], out int duracionMinutos))
                return Task.FromResult("❌ La duración debe ser un número (en minutos).");
            
            string ubicacion = args[3];
            string descripcion = string.Join(" ", args, 4, args.Length - 4);
            
            string resultado = _interfaz.RegistrarReunion(idCliente, descripcion, ubicacion, duracionMinutos, fecha);
            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
