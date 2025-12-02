using CrmUcu.Models.Personas;
using CrmUcu.Models.Enums;
using CrmUcu.Models.Interacciones;

namespace CrmUcu.Repositories
{
    /// <summary>
    /// Repositorio que gestiona todos los clientes del sistema. Aplicamos los siguientes patrones: 
    /// Patrón Singleton: solo existe una instancia.
    /// Patrón Expert: conoce toda la colección de clientes.
    /// </summary>
    public class RepositorioCliente
    {
        private static RepositorioCliente? _instancia;
        private static readonly object _lock = new object();
        
        public List<Cliente> _clientes;
        
        private int _proximoId;

        /// <summary>
        /// Constructor privado (Singleton).
        /// </summary>
        private RepositorioCliente()
        {
            _clientes = new List<Cliente>();
            _proximoId = 1;
        }

        /// <summary>
        /// Obtiene todos los clientes de un vendedor específico.
        /// </summary>
        /// <param name="idVendedor">ID del vendedor</param>
        /// <returns>Lista de clientes asignados a ese vendedor</returns>
        public List<Cliente> vendedorResponsableDelCliente(int idVendedor)
        {
            return _clientes.Where(c => c.IdVendedor == idVendedor).ToList();
        }
              
        /// <summary>
        /// Busca un cliente por su ID.
        /// </summary>
        /// <param name="idCliente">ID del cliente</param>
        /// <returns>Cliente encontrado o null</returns>
        public Cliente? ObtenerClientePorId(int idCliente)
        {
            return _clientes.FirstOrDefault(c => c.Id == idCliente);
        }
        
        /// <summary>
        /// Agrega una etiqueta a un cliente (evita duplicados).
        /// </summary>
        /// <param name="idCliente">ID del cliente</param>
        /// <param name="etiqueta">Etiqueta a agregar</param>
        /// <returns>True si se agregó correctamente</returns>
        public bool AgregarEtiquetaACliente(int idCliente, Etiqueta etiqueta)
        {
            var cliente = ObtenerClientePorId(idCliente);
            if (cliente == null) return false;

            // Solo agregar si no existe ya
            if (!cliente.Etiquetas.Any(e => e.Nombre.Equals(etiqueta.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                cliente.Etiquetas.Add(etiqueta);
            }
            return true;
        }

        /// <summary>
        /// Actualiza o agrega datos demográficos de un cliente.
        /// </summary>
        /// <param name="idCliente">ID del cliente</param>
        /// <param name="genero">Género del cliente</param>
        /// <param name="fechaNacimiento">Fecha de nacimiento</param>
        /// <returns>True si se actualizó correctamente</returns>
        public bool ActualizarDatosExtra(int idCliente, Genero genero, DateTime fechaNacimiento)
        {
            var cliente = ObtenerClientePorId(idCliente);
            if (cliente == null) return false;

            cliente.Genero = genero;
            cliente.FechaNacimiento = fechaNacimiento;
            return true;
        }

        /// <summary>
        /// Obtiene la única instancia del repositorio (Singleton)
        /// </summary>
        /// <returns>Instancia única de RepositorioCliente</returns>
        public static RepositorioCliente ObtenerInstancia()
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new RepositorioCliente();
                    }
                }
            }
            return _instancia;
        }
        
        /// <summary>
        /// Crea un nuevo cliente y lo agrega al repositorio.
        /// Patrón Creator: el repositorio crea instancias de Cliente.
        /// </summary>
        /// <param name="mail">Email del cliente</param>
        /// <param name="nombre">Nombre del cliente</param>
        /// <param name="apellido">Apellido del cliente</param>
        /// <param name="telefono">Teléfono del cliente</param>
        /// <param name="idVendedor">ID del vendedor responsable</param>
        /// <returns>Cliente creado</returns>
        public Cliente CrearCliente(string mail, string nombre, string apellido, string telefono, int idVendedor)
        {
            var cliente = new Cliente(_proximoId, mail, nombre, apellido, telefono, idVendedor);
            _clientes.Add(cliente);
            Console.WriteLine("cliente creado!");
            cliente.MostrarInfo();
            _proximoId++;
            return cliente;
        }

        /// <summary>
        /// Busca clientes por nombre, apellido, teléfono o email         
        /// Patrón Expert: el repo es experto en búsquedas.
        /// </summary>
        /// <param name="criterio">Texto a buscar</param>
        /// <returns>Lista de clientes que coinciden</returns>
        public List<Cliente> BuscarPor(string criterio)
        {
            //normalizamos el el dato
            string criterioBusqueda = criterio.Trim().ToLower();
            
            if (string.IsNullOrEmpty(criterioBusqueda))
            {
                return new List<Cliente>();
            }
            
            List<Cliente> resultados = new List<Cliente>();
            
            // Buscamos en varios lados.
            for (int i = 0; i < _clientes.Count; i++)
            {
                Cliente cliente = _clientes[i];
                
                if (cliente.Nombre.ToLower().Contains(criterioBusqueda) ||
                    cliente.Apellido.ToLower().Contains(criterioBusqueda) ||
                    cliente.Telefono.ToLower().Contains(criterioBusqueda) ||
                    cliente.Mail.ToLower().Contains(criterioBusqueda))
                {
                    resultados.Add(cliente);
                }
            }
            
            return resultados;
        }
        
        /// <summary>
        /// Busca un cliente por ID.
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Cliente encontrado o null</returns>
        public Cliente BuscarPorId(int id)
        {
            for (int i = 0; i < _clientes.Count; i++)
            {
                if (_clientes[i].Id == id)
                {
                    return _clientes[i];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Elimina un cliente del repositorio.
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        public void EliminarCliente(int id)
        {
            for (int i = 0; i < _clientes.Count(); i++)
            {
                if(_clientes[i].Id == id)
                {
                     _clientes.Remove(_clientes[i]);
                }
            }
        }

        /// <summary>
        /// Obtiene todos los clientes del sistema.
        /// </summary>
        /// <returns>Lista completa de clientes</returns>
        public List<Cliente> ObtenerTodos()
        {
            return _clientes;
        }

        /// <summary>
        /// Busca clientes que tengan una etiqueta específica.
        /// </summary>
        /// <param name="nombreEtiqueta">Nombre de la etiqueta</param>
        /// <returns>Lista de clientes con esa etiqueta</returns>
        public List<Cliente> BuscarPorEtiqueta(string nombreEtiqueta)
        {
            List<Cliente> resultados = new List<Cliente>();
            string etiquetaBusqueda = nombreEtiqueta.Trim().ToLower();
            
            for (int i = 0; i < _clientes.Count; i++)
            {
                Cliente cliente = _clientes[i];
                
                if (cliente.Etiquetas != null)
                {
                    for (int j = 0; j < cliente.Etiquetas.Count; j++)
                    {
                        if (cliente.Etiquetas[j].Nombre.ToLower().Contains(etiquetaBusqueda))
                        {
                            resultados.Add(cliente);
                            break; // Ya lo encontramos
                        }
                    }
                }
            }
            
            return resultados;
        }
    }
}
