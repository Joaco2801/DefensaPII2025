using CrmUcu.Core;
using System.Threading.Tasks;

namespace CrmUcu.Commands
{
    /// <summary>
    /// Comando para listar todas las etiquetas registradas en el sistema.
    /// </summary>
    public class ListarEtiquetasCommand : ICommand
    {
        private readonly Interfaz _interfaz;

        /// <summary>
        /// Inicializa el comando con la interfaz principal.
        /// </summary>
        public ListarEtiquetasCommand(Interfaz interfaz)
        {
            _interfaz = interfaz;
        }

        /// <summary>
        /// Ejecuta: Kbr4 listarEtiquetas.
        /// Requiere tener sesión iniciada.
        /// </summary>
        /// <param name="args">No se utilizan argumentos para este comando.</param>
        /// <returns>Un texto con el listado de etiquetas o un mensaje informativo.</returns>
        public Task<string> ExecuteAsync(string[] args)
        {
            // Kbr4 listarEtiquetas
            
            if (!_interfaz.EstaLogueado)
                return Task.FromResult("❌ Debes iniciar sesión primero.");

            var etiquetas = _interfaz.ObtenerTodasLasEtiquetas();

            if (etiquetas.Count == 0)
                return Task.FromResult("📋 No hay etiquetas registradas.");

            string respuesta = $"📋 **Etiquetas ({etiquetas.Count}):**\n\n";
            
            foreach (var e in etiquetas)
            {
                respuesta += $"**ID: {e.Id}** - {e.Nombre}";
                if (!string.IsNullOrEmpty(e.Color))
                    respuesta += $" (Color: {e.Color})";
                if (!string.IsNullOrEmpty(e.Descripcion))
                    respuesta += $"\n   {e.Descripcion}";
                respuesta += "\n\n";
            }

            return Task.FromResult(respuesta);
        }
    }
}
