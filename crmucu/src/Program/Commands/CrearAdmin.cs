using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para crear un nuevo administrador desde la consola.
    /// </summary>
    public class CrearAdminCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        public CrearAdminCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 crearAdmin &lt;nombre&gt; &lt;apellido&gt; &lt;mail&gt; &lt;telefono&gt; &lt;nombreUsuario&gt; &lt;password&gt;.
        /// Solo puede usarlo un admin logueado.
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 crearAdmin <nombre> <apellido> <mail> <telefono> <nombreUsuario> <password>
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsAdmin())
                return Task.FromResult("❌ Solo los administradores pueden crear administradores.");

            if (args.Length < 6)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 crearAdmin <nombre> <apellido> <mail> <telefono> <nombreUsuario> <password>");

            string nombre = args[0];
            string apellido = args[1];
            string mail = args[2];
            string telefono = args[3];
            string nombreUsuario = args[4];
            string password = args[5];

            string resultado = _interfaz.CrearAdmin(nombre, apellido, mail, telefono, nombreUsuario, password);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
