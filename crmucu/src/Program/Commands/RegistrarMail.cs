using CrmUcu.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar emails con clientes.
    /// Soporta emails entrantes y salientes con múltiples destinatarios.
    /// Aplica Command Pattern y SRP.
    /// </summary>
    public class RegistrarMailCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public RegistrarMailCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para registrar un email.
        /// Valida parámetros y parsea destinatarios separados por comas.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar mails.");
            
            if (args.Length < 5)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarMail <idCliente> <entrante/saliente> <asunto> <destinatarios> <descripcion>\n" +
                                     "Ejemplo: Kbr4 registrarMail 1 saliente \"Propuesta comercial\" cliente@mail.com Enviamos propuesta");
            
            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");
            
            string tipoMail = args[1].ToLower();
            bool esEntrante;
            
            if (tipoMail == "entrante")
                esEntrante = true;
            else if (tipoMail == "saliente")
                esEntrante = false;
            else
                return Task.FromResult("❌ Tipo de mail inválido. Usa: entrante o saliente");
            
            string asunto = args[2];
            
            // Parsear destinatarios (soporta múltiples separados por comas)
            string[] destinatariosArray = args[3].Split(',');
            List<string> destinatarios = new List<string>(destinatariosArray);
            
            string descripcion = string.Join(" ", args, 4, args.Length - 4);
            
            string resultado = _interfaz.RegistrarMail(idCliente, descripcion, esEntrante, asunto, destinatarios);
            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
