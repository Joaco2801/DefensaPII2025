using CrmUcu.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar mensajes (SMS, WhatsApp, etc.) con clientes.
    /// Soporta mensajes entrantes y salientes con múltiples destinatarios.
    /// Aplica Command Pattern y SRP.
    /// </summary>
    public class RegistrarMensajeCommand : ICommand
    {
        private readonly Interfaz _interfaz;
        
        public RegistrarMensajeCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }
        
        /// <summary>
        /// Ejecuta el comando para registrar un mensaje con un cliente.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");
            
            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar mensajes.");
            
            if (args.Length < 5)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarMensaje <idCliente> <entrante/saliente> <asunto> <destinatarios> <descripcion>\n" +
                                     "Ejemplo: Kbr4 registrarMensaje 1 saliente \"Seguimiento\" cliente@mail.com Recordatorio de reunión");
            
            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");
            
            string tipoMensaje = args[1].ToLower();
            bool esEntrante;
            
            if (tipoMensaje == "entrante")
                esEntrante = true;
            else if (tipoMensaje == "saliente")
                esEntrante = false;
            else
                return Task.FromResult("❌ Tipo de mensaje inválido. Usa: entrante o saliente");
            
            string asunto = args[2];
            string[] destinatariosArray = args[3].Split(',');
            List<string> destinatarios = new List<string>(destinatariosArray);
            string descripcion = string.Join(" ", args, 4, args.Length - 4);
            
            string resultado = _interfaz.RegistrarMensaje(idCliente, descripcion, esEntrante, asunto, destinatarios);
            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
