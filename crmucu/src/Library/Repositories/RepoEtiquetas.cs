using CrmUcu.Models.Personas;

namespace CrmUcu.Repositories
{
    /// <summary>
    /// Repositorio que gestiona las etiquetas del sistema.
    /// Aplica Singleton, Expert, Creator y SRP.
    /// </summary>
    public class RepositorioEtiqueta
    {
        private static RepositorioEtiqueta? _instancia;
        private static readonly object _lock = new object();
        public List<Etiqueta> _Etiquetas;
        private int _proximoId;
        
        /// <summary>
        /// Constructor privado (Singleton).
        /// </summary>
        private RepositorioEtiqueta()
        {
            _Etiquetas = new List<Etiqueta>();
            _proximoId = 1;
        }
        
        /// <summary>
        /// Crea una nueva etiqueta. Aplica Creator.
        /// </summary>
        public Etiqueta CrearEtiqueta(string nombre, string? color = null, string? descripcion = null)
        {
            var nueva = new Etiqueta(nombre, color, descripcion);
            nueva.Id = _proximoId;
            _Etiquetas.Add(nueva);
            _proximoId++;
            return nueva;
        }
        
        /// <summary>
        /// Obtiene todas las etiquetas.
        /// </summary>
        public List<Etiqueta> ObtenerTodas()
        {
            return _Etiquetas;
        }
        
        /// <summary>
        /// Busca una etiqueta por ID. Aplica Expert.
        /// </summary>
        public Etiqueta? BuscarPorId(int id)
        {
            for (int i = 0; i < _Etiquetas.Count; i++)
            {
                if (_Etiquetas[i].Id == id)
                {
                    return _Etiquetas[i];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Obtiene la instancia única (Singleton con thread-safety).
        /// </summary>
        public static RepositorioEtiqueta ObtenerInstancia()
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new RepositorioEtiqueta();
                    }
                }
            }
            return _instancia;
        }
    }
}
