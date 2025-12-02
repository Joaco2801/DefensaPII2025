using CrmUcu.Models.Personas;

namespace CrmUcu.Repositories
{
    /// <summary>
    /// Repositorio que gestiona los vendedores del sistema.
    /// Aplica Singleton, Expert, Creator y SRP.
    /// </summary>
    public class RepositorioVendedor
    {
        private static RepositorioVendedor? _instancia;
        private static readonly object _lock = new object();
        public List<Vendedor> _vendedores;
        private int _proximoId;
        
        /// <summary>
        /// Constructor privado (Singleton). Crea un vendedor inicial de prueba.
        /// </summary>
        private RepositorioVendedor()
        {
            _vendedores = new List<Vendedor>();
            _proximoId = 1;
            
            var vendedorInicial = new Vendedor(
                id: _proximoId,
                mail: "pancho@mail.com",
                nombre: "Juan",
                apellido: "Pérez",
                telefono: "099999999",
                nombreUsuario: "pancho",
                password: "123"
            );
            
            _vendedores.Add(vendedorInicial);
            _proximoId++;
        }
        
        /// <summary>
        /// Crea un nuevo vendedor. Aplica Creator.
        /// </summary>
        public Vendedor CrearVendedor(string nombre, string apellido, string mail, string telefono, string nombreUsuario, string password)
        {
            var vendedor = new Vendedor(_proximoId, mail, nombre, apellido, telefono, nombreUsuario, password);
            _vendedores.Add(vendedor);
            _proximoId++;
            return vendedor;
        }
        
        /// <summary>
        /// Busca un vendedor por ID. Aplica Expert.
        /// </summary>
        public Vendedor? BuscarPorId(int id)
        {
            for (int i = 0; i < _vendedores.Count; i++)
            {
                if (_vendedores[i].Id == id)
                {
                    return _vendedores[i];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Obtiene todos los vendedores.
        /// </summary>
        public List<Vendedor> ObtenerTodos()
        {
            return _vendedores;
        }
        
        /// <summary>
        /// Obtiene la instancia única (Singleton con thread-safety).
        /// </summary>
        public static RepositorioVendedor ObtenerInstancia()
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new RepositorioVendedor();
                    }
                }
            }
            return _instancia;
        }
    }
}
