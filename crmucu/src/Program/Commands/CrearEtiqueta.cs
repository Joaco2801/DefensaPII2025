using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para crear una nueva etiqueta en el sistema.
    /// Solo puede ser usado por un vendedor logueado.
    /// </summary>
    public class CrearEtiquetaCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Recibe la interfaz principal para ejecutar la lógica de negocio.
        /// </summary>
        public CrearEtiquetaCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 crearEtiqueta &lt;nombre&gt; [color] [descripcion].
        /// </summary>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 crearEtiqueta <nombre> [color] [descripcion]
            // Ejemplo: Kbr4 crearEtiqueta VIP "#FFD700" "Clientes prioritarios"
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden crear etiquetas.");

            if (args.Length < 1)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 crearEtiqueta <nombre> [color] [descripcion]\n" +
                                     "Ejemplo: Kbr4 crearEtiqueta VIP \"#FFD700\" \"Clientes prioritarios\"");

            string nombre = args[0];
            string? color = args.Length > 1 ? args[1] : null;
            string? descripcion = args.Length > 2 ? args[2] : null;

            string resultado = _interfaz.CrearEtiqueta(nombre, color, descripcion);
            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
