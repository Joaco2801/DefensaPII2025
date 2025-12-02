using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para registrar una llamada (entrante o saliente) asociada a un cliente.
    /// </summary>
    public class RegistrarLlamadaCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Usa la interfaz principal del sistema para registrar la llamada.
        /// </summary>
        /// <param name="interfaz">Instancia de <see cref="Interfaz"/>.</param>
        public RegistrarLlamadaCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta:
        /// <c>Kbr4 registrarLlamada &lt;idCliente&gt; &lt;entrante/saliente&gt; &lt;duracionSeg&gt; &lt;si/no&gt; &lt;descripcion...&gt;</c>.
        /// Solo puede usarlo un vendedor logueado.
        /// </summary>
        /// <param name="args">
        /// Argumentos: id del cliente, tipo de llamada, duración en segundos,
        /// si fue contestada y una descripción libre.
        /// </param>
        /// <returns>Mensaje de éxito o error para mostrar en consola.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 registrarLlamada <idCliente> <entrante/saliente> <duracionSeg> <contestada:si/no> <descripcion...>
            // Ejemplo: Kbr4 registrarLlamada 1 entrante 180 si Consulta sobre precios
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            if (!_interfaz.EsVendedor())
                return Task.FromResult("❌ Solo los vendedores pueden registrar llamadas.");

            if (args.Length < 5)
                return Task.FromResult("❌ Faltan datos. Usa: Kbr4 registrarLlamada <idCliente> <entrante/saliente> <duracionSeg> <si/no> <descripcion>\n" +
                                     "Ejemplo: Kbr4 registrarLlamada 1 entrante 180 si Consulta sobre productos");

            if (!int.TryParse(args[0], out int idCliente))
                return Task.FromResult("❌ El ID del cliente debe ser un número.");

            string tipoLlamada = args[1].ToLower();
            bool esEntrante;
            
            if (tipoLlamada == "entrante")
                esEntrante = true;
            else if (tipoLlamada == "saliente")
                esEntrante = false;
            else
                return Task.FromResult("❌ Tipo de llamada inválido. Usa: entrante o saliente");

            if (!int.TryParse(args[2], out int duracionSegundos))
                return Task.FromResult("❌ La duración debe ser un número (en segundos).");

            string contestadaStr = args[3].ToLower();
            bool contestada;
            
            if (contestadaStr == "si" || contestadaStr == "sí")
                contestada = true;
            else if (contestadaStr == "no")
                contestada = false;
            else
                return Task.FromResult("❌ Contestada debe ser: si o no");

            // El resto de los argumentos es la descripción
            string descripcion = string.Join(" ", args, 4, args.Length - 4);

            string resultado = _interfaz.RegistrarLlamada(idCliente, descripcion, esEntrante, duracionSegundos, contestada);

            return Task.FromResult(resultado.Contains("exitosamente") 
                ? $"✅ {resultado}" 
                : $"❌ {resultado}");
        }
    }
}
