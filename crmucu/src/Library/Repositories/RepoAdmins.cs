using CrmUcu.Models.Personas;

namespace CrmUcu.Repositories
{
    /// <summary>
    /// Repositorio que gestiona los administradores del sistema. Aplicamos los siguientes patrones
    /// Patrón Singleton: solo existe una instancia.
    /// Patrón Expert: conoce toda la colección de administradores.
    /// </summary>
    public class RepositorioAdmin
    {
        private static RepositorioAdmin? _instancia;
        private static readonly object _lock = new object();
        
        public List<Admin> _admins;
        
        private int _proximoId;
        
        /// <summary>
        /// Constructor privado (Singleton).
        /// Crea un administrador inicial por defecto.
        /// </summary>
        private RepositorioAdmin()
        {
            _admins = new List<Admin>();
            _proximoId = 1;
            
            // Administrador inicial para acceso al sistema
            var adminInicial = new Admin(
                id: _proximoId,
                mail: "admin@crm.com",
                nombre: "Administrador",
                apellido: "Principal",
                telefono: "099000000",
                nombreUsuario: "admin",
                password: "admin123"
            );
            _admins.Add(adminInicial);
            _proximoId++;
        }
        
        /// <summary>
        /// Crea un nuevo administrador y lo agrega al repositorio.
        /// Patrón Creator: el repositorio crea instancias de Admin.
        /// </summary>
        /// <param name="nombre">Nombre del administrador</param>
        /// <param name="apellido">Apellido del administrador</param>
        /// <param name="mail">Email del administrador</param>
        /// <param name="telefono">Teléfono del administrador</param>
        /// <param name="nombreUsuario">Usuario para login</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Administrador creado</returns>
        public Admin CrearAdmin(string nombre, string apellido, string mail, string telefono, string nombreUsuario, string password)
        {
            var admin = new Admin(_proximoId, mail, nombre, apellido, telefono, nombreUsuario, password);
            _admins.Add(admin);
            _proximoId++;
            return admin;
        }
        
        /// <summary>
        /// Busca un administrador por su ID.
        /// </summary>
        /// <param name="id">ID del administrador</param>
        /// <returns>Administrador encontrado o null</returns>
        public Admin? BuscarPorId(int id)
        {
            for (int i = 0; i < _admins.Count; i++)
            {
                if (_admins[i].Id == id)
                {
                    return _admins[i];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Obtiene todos los administradores del sistema.
        /// </summary>
        /// <returns>Lista completa de administradores</returns>
        public List<Admin> ObtenerTodos()
        {
            return _admins;
        }
        
        /// <summary>
        /// Obtiene la única instancia del repositorio (Singleton).
        /// </summary>
        /// <returns>Instancia única de RepositorioAdmin</returns>
        public static RepositorioAdmin ObtenerInstancia()
        {
          
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new RepositorioAdmin();
                    }
                }
            }
            return _instancia;
        }
    }
}
