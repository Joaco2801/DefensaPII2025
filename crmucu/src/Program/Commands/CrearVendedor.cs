using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para crear un nuevo vendedor desde la consola.
    /// Solo puede ser usado por un administrador logueado.
    /// </summary>
    public class CrearVendedorCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal del sistema.
        /// </summary>
        /// <param name="interfaz">Instancia de la clase <see cref="Interfaz"/>.</param>
        public CrearVendedorCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 crearVendedor &lt;nombre&gt; &lt;apellido&gt; &lt;mail&gt; 
        /// &lt;telefono&gt; &lt;nombreUsuario&gt; &lt;password&gt;.
        /// </summary>
        /// <param name="args">
        /// Parámetros del comando en este orden:
        /// nombre, apellido, mail, teléfono, nombre de usuario y contraseña.
        /// </param>
        /// <returns>Mensaje de resultado para mostrar en consola.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 crearVendedor <nombre> <apellido> <mail> <telefono> <nombreUsuario> <password>
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsAdmin())
                return Task.FromResult("❌ Solo los administradores pueden crear vendedores.");

            if (args.Length < 6)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 crearVendedor <nombre> <apellido> <mail> <telefono> <nombreUsuario> <password>");

            string nombre = args[0];
            string apellido = args[1];
            string mail = args[2];
            string telefono = args[3];
            string nombreUsuario = args[4];
            string password = args[5];

            string resultado = _interfaz.CrearVendedor(nombre, apellido, mail, telefono, nombreUsuario, password);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
