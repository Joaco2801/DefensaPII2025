using Discord.WebSocket;
using CrmUcu.Core;
using CrmUcu.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmUcu
{
    /// <summary>
    /// Manejador central de comandos del bot.
    /// Aplica Command Pattern: cada comando es una clase que implementa ICommand.
    /// Aplica Controller: recibe mensajes de Discord y delega a comandos específicos.
    /// Aplica SRP: solo se encarga de parsear mensajes y ejecutar comandos.
    /// Aplica OCP: agregar nuevos comandos solo requiere registrarlos en el diccionario.
    /// </summary>
    public class CommandHandler
    {
        private readonly Interfaz _interfaz;
        private readonly Dictionary<string, ICommand> _commands;
        
        /// <summary>
        /// Constructor. Inicializa la interfaz del sistema y registra todos los comandos disponibles.
        /// </summary>
        public CommandHandler()
        {
            _interfaz = new Interfaz();
            _commands = new Dictionary<string, ICommand>
            {
                { "login", new LoginCommand(_interfaz) },
                { "logout", new LogoutCommand(_interfaz) },
                { "crearCliente", new CrearClienteCommand(_interfaz)},
                { "modificarCliente", new ModificarClienteCommand(_interfaz) },
                { "mostrarMisClientes", new MostrarMisClientesCommand(_interfaz) },
                { "eliminarCliente", new EliminarClienteCommand(_interfaz) },
                { "buscarClientes", new BuscarClientesCommand(_interfaz) },
                { "registrarLlamada", new RegistrarLlamadaCommand(_interfaz) },
                { "registrarMail", new RegistrarMailCommand(_interfaz) },
                { "registrarMensaje", new RegistrarMensajeCommand(_interfaz) },
                { "registrarReunion", new RegistrarReunionCommand(_interfaz)},
                { "registrarCotizacion", new RegistrarCotizacionCommand(_interfaz) },
                { "registrarDatosDemograficos", new RegistrarDatosDemograficosCommand(_interfaz) },
                { "verInteracciones", new VerInteraccionesCommand(_interfaz) },
                { "contactosPendientes", new DetectarContactosPendientesCommand(_interfaz) },
                { "clientesSinInteraccion", new ClientesSinInteraccionCommand(_interfaz) },
                { "listarVendedores", new ListarVendedoresCommand(_interfaz) }, 
                { "asignarCliente", new AsignarClienteCommand(_interfaz) },
                { "crearVendedor", new CrearVendedorCommand(_interfaz) },
                { "crearAdmin", new CrearAdminCommand(_interfaz) },
                { "suspenderUsuario", new SuspenderUsuarioCommand(_interfaz) },
                { "eliminarUsuario", new EliminarUsuarioCommand(_interfaz) },
                { "registrarVenta", new RegistrarVentaCommand(_interfaz) },
                { "totalVentas", new TotalVentasCommand(_interfaz) },
                { "crearEtiqueta", new CrearEtiquetaCommand(_interfaz) },
                { "agregarEtiqueta", new AgregarEtiquetaCommand(_interfaz) },
                { "listarEtiquetas", new ListarEtiquetasCommand(_interfaz) },
                { "buscarPorEtiqueta", new BuscarPorEtiquetaCommand(_interfaz) },
                { "dashboard", new DashboardCommand(_interfaz) },
                {"ventasDisminuidas", new VentasDisminuidasCommand(_interfaz)}, //Comando añadido al programa para la Defensa del Proyecto
            };
        }
     
        /// <summary>
        /// Procesa mensajes de Discord. Parsea el comando y ejecuta la acción correspondiente.
        /// Formato esperado: "Kbr4 [comando] [argumentos]"
        /// </summary>
        public async Task HandleMessageAsync(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;
            
            var partes = message.Content.Split(' ');
            
            if (partes.Length < 2 || partes[0] != "Kbr4")
                return;
            
            string comandoNombre = partes[1];
            string[] args = partes.Skip(2).ToArray();
            
            if (_commands.TryGetValue(comandoNombre, out var comando))
            {
                try
                {
                    string respuesta = await comando.ExecuteAsync(args);
                    await message.Channel.SendMessageAsync(respuesta);
                }
                catch (Exception ex)
                {
                    await message.Channel.SendMessageAsync($"❌ Error: {ex.Message}");
                }
            }
            else
            {
                await message.Channel.SendMessageAsync($"❌ Comando '{comandoNombre}' no reconocido.");
            }
        }
    }
}
