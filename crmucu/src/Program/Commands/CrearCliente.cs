using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para crear un nuevo cliente desde la consola.
    /// Solo puede ser usado por un vendedor logueado.
    /// </summary>
    public class CrearClienteCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        public CrearClienteCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 crearCliente &lt;nombre&gt; &lt;apellido&gt; &lt;mail&gt; &lt;telefono&gt;.
        /// Valida sesión, rol de vendedor y datos mínimos.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            Console.WriteLine($"[DEBUG] EstaLogueado: {_interfaz.EstaLogueado}");
            Console.WriteLine($"[DEBUG] UsuarioActual: {_interfaz.UsuarioActual?.Nombre}");
            Console.WriteLine($"[DEBUG] EsVendedor: {_interfaz.EsVendedor()}");
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden crear clientes.");

            // Kbr4 crearCliente <nombre> <apellido> <mail> <telefono>
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden crear clientes.");

            if (args.Length < 4)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 crearCliente <nombre> <apellido> <mail> <telefono>");

            string nombre = args[0];
            string apellido = args[1];
            string mail = args[2];
            string telefono = args[3];

            bool creado = _interfaz.CrearClienteComoVendedor(nombre, apellido, mail, telefono);

            return Task.FromResult(creado
                ? $"✅ Cliente {nombre} {apellido} creado exitosamente."
                : "❌ Error al crear cliente.");
        }
    }
}
