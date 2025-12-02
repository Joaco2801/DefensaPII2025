using CrmUcu.Models.Personas;
using CrmUcu.Models.Enums;
using CrmUcu.Repositories;
using CrmUcu.Models.Interacciones;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrmUcu.Core
{
    /// <summary>
    /// Interfaz principal del sistema CRM.
    /// Gestiona la autenticación, operaciones de vendedores, administración de usuarios,
    /// manejo de clientes, etiquetas e interacciones.
    /// Todo acceso a repositorios y lógica del negocio pasa por esta clase.
    /// </summary>
    public class Interfaz
    {
        private readonly RepositorioAdmin _repoAdmins;
        private readonly RepositorioVendedor _repoVendedores;
        private readonly RepositorioCliente _repoClientes;
        private readonly RepositorioEtiqueta _repoEtiquetas;
        private Stack<string> _historialAcciones;
        
        /// <summary>
        /// Usuario actualmente logueado en el sistema. Puede ser Admin o Vendedor.
        /// </summary>
        public Usuario? UsuarioActual { get; private set; }

        /// <summary>
        /// Indica si hay un usuario logueado en la plataforma.
        /// </summary>
        public bool EstaLogueado => UsuarioActual != null;

        /// <summary>
        /// Constructor de la interfaz del CRM.
        /// Inicializa todos los repositorios y deja la sesión sin usuario activo.
        /// </summary>
        public Interfaz()
        {
            _repoAdmins = RepositorioAdmin.ObtenerInstancia();
            _repoVendedores = RepositorioVendedor.ObtenerInstancia();
            _repoClientes = RepositorioCliente.ObtenerInstancia();
            _repoEtiquetas = RepositorioEtiqueta.ObtenerInstancia();
            _historialAcciones = new Stack<string>();
            UsuarioActual = null;
        }

        // ============================================
        // AUTENTICACIÓN
        // ============================================

        /// <summary>
        /// Inicia sesión como admin o vendedor.
        /// Devuelve el usuario autenticado o null si las credenciales no coinciden.
        /// </summary>
        public Usuario? IniciarSesion(string username, string password)
        {
            _historialAcciones.Push("Login");


            //busca el primer elemento a tal que a.Autenticar(username, password) sea true. Una forma mas elegante que un for o un foreach.
            var admin = _repoAdmins._admins.FirstOrDefault(a => a.Autenticar(username, password));
            if (admin != null)
            {
                UsuarioActual = admin;
                return admin;
            }

            var vendedor = _repoVendedores._vendedores.FirstOrDefault(v => v.Autenticar(username, password));
            if (vendedor != null)
            {
                UsuarioActual = vendedor;
                return vendedor;
            }

            return null;
        }

        /// <summary>
        /// Cierra sesión del usuario actual y limpia el historial de acciones.
        /// </summary>
        public void CerrarSesion()
        {
            UsuarioActual = null;
            _historialAcciones.Clear();
        }

        /// <summary>
        /// Autenticación directa para administradores.
        /// </summary>
        public Admin? IniciarSesionAdmin(string username, string password)
        {
            return _repoAdmins._admins.FirstOrDefault(a => a.Autenticar(username, password));
        }

        /// <summary>
        /// Autenticación directa para vendedores.
        /// </summary>
        public Vendedor? IniciarSesionVendedor(string username, string password)
        {
            return _repoVendedores._vendedores.FirstOrDefault(v => v.Autenticar(username, password));
        }

        // ============================================
        // HELPERS DE SESIÓN
        // ============================================

        /// <summary>
        /// Indica si el usuario actual es administrador.
        /// </summary>
        public bool EsAdmin()
        {
            return UsuarioActual is Admin;
        }

        /// <summary>
        /// Indica si el usuario actual es vendedor.
        /// </summary>
        public bool EsVendedor()
        {
            
            bool resultado = UsuarioActual is Vendedor;
            
            return resultado;
        }

        /// <summary>
        /// Devuelve el vendedor actualmente logueado.
        /// </summary>
        public Vendedor? ObtenerVendedorActual()
        {
            return UsuarioActual as Vendedor;
        }

        /// <summary>
        /// Devuelve el administrador actualmente logueado.
        /// </summary>
        public Admin? ObtenerAdminActual()
        {
            return UsuarioActual as Admin;
        }

        // ============================================
        // CLIENTES
        // ============================================

        /// <summary>
        /// Crea un cliente asociado al vendedor logueado.
        /// </summary>
        public bool CrearClienteComoVendedor(string nombre, string apellido, string mail, string telefono)
        {
            _historialAcciones.Push("CrearCliente");
            
            if (UsuarioActual is not Vendedor vendedor)
                return false;

            vendedor.CrearCliente(nombre, apellido, mail, telefono);
            return true;
        }

        /// <summary>
        /// Registra datos demográficos adicionales para un cliente.
        /// Solo un vendedor puede ejecutar esta acción.
        /// </summary>
        public string RegistrarDatosDemograficos(int idCliente, string generoTexto, DateTime fechaNacimiento)
        {
            _historialAcciones.Push("RegistrarDatosExtraCliente");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar datos extra";
            
            var cliente = _repoClientes.ObtenerClientePorId(idCliente);
            if (cliente == null) 
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";

            generoTexto = generoTexto.Trim();
            if (!Enum.TryParse<Genero>(generoTexto, true, out var parsedGenero))
                return "Género inválido. Usa: Masculino, Femenino, Otro o NoEspecifica";

            cliente.Genero = parsedGenero;
            cliente.FechaNacimiento = fechaNacimiento;
            
            return "Datos extra registrados exitosamente";
        }

        /// <summary>
        /// Busca un cliente por ID.
        /// </summary>
        public Cliente BuscarClientePorId(int id)
        {
            _historialAcciones.Push("BuscarCliente");
            return _repoClientes.BuscarPorId(id);
        }

        /// <summary>
        /// Busca clientes por un criterio textual.
        /// </summary>
        public List<Cliente> BuscarClientesPor(string criterio)
        {
            _historialAcciones.Push("BuscarClientesPor");
            
            if (!EstaLogueado)
                return new List<Cliente>();
            
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            return repoClientes.BuscarPor(criterio);
        }

        /// <summary>
        /// Modifica los datos de un cliente perteneciente al vendedor actual.
        /// </summary>
        public string ModificarClienteComoVendedor(int idCliente, string? nuevoNombre = null, 
                                           string? nuevoApellido = null, string? nuevoMail = null, 
                                           string? nuevoTelefono = null)
        {
            _historialAcciones.Push("ModificarCliente");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden modificar clientes";

            return vendedor.ModificarCliente(idCliente, nuevoNombre, nuevoApellido, nuevoMail, nuevoTelefono);
        }

        /// <summary>
        /// Elimina un cliente perteneciente al vendedor actual.
        /// </summary>
        public string EliminarClienteComoVendedor(int idCliente)
        {
            _historialAcciones.Push("EliminarCliente");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden eliminar clientes";

            return vendedor.EliminarCliente(idCliente);
        }

        /// <summary>
        /// Lista los clientes asociados a un vendedor.
        /// </summary>
        public List<Cliente> ListadoDeClientes(int idVendedor)
        {
            _historialAcciones.Push("listadoDeClientes");
            return _repoClientes.vendedorResponsableDelCliente(idVendedor);
        }

        /// <summary>
        /// Obtiene todos los clientes de un vendedor por ID.
        /// </summary>
        public List<Cliente> ObtenerClientesDeVendedor(int idVendedor)
        {
            _historialAcciones.Push("ObtenerClientesDeVendedor");
            return _repoClientes.vendedorResponsableDelCliente(idVendedor);
        }

        /// <summary>
        /// Obtiene los clientes del vendedor actualmente logueado.
        /// </summary>
        public List<Cliente> ObtenerClientesDeVendedorActual()
        {
            _historialAcciones.Push("ObtenerClientesDeVendedor");
            
            if (UsuarioActual is not Vendedor vendedor)
                return new List<Cliente>();
                
            return _repoClientes.vendedorResponsableDelCliente(vendedor.Id);
        }

        /// <summary>
        /// Devuelve un cliente por su ID.
        /// </summary>
        public Cliente? ObtenerClientePorId(int idCliente)
        {
            return _repoClientes.ObtenerClientePorId(idCliente);
        }

        /// <summary>
        /// Detecta clientes con interacciones pendientes sin responder.
        /// </summary>
        public string DetectarContactosPendientes(int diasSinRespuesta = 3)
        {
            _historialAcciones.Push("DetectarContactosPendientes");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden detectar contactos pendientes";
            
            var clientesPendientes = vendedor.DetectarContactoPendiente(diasSinRespuesta);
            
            if (clientesPendientes.Count == 0)
                return $"✅ ¡Buenazo! No hay clientes con contacto pendiente de más de {diasSinRespuesta} días.";
            
            string resultado = $"⚠️ **Clientes con contacto pendiente (más de {diasSinRespuesta} días sin responder):**\n\n";
            
            foreach (var cliente in clientesPendientes)
            {
                resultado += $"🔸 **{cliente.Nombre} {cliente.Apellido}** (ID: {cliente.Id})\n";
                resultado += $"   Mail: {cliente.Mail} | Tel: {cliente.Telefono}\n";
                resultado += $"   **Interacciones sin responder:**\n";

                foreach (var inter in cliente.Interacciones)
                {
                    if (inter is IRespondible resp && resp.EsEntrante && !resp.Respondida)
                    {
                        int diasReal = (DateTime.Now - inter.Fecha).Days;
                        //formateamos el string para que quede lindo en el chat de discord
                        resultado += $"      • {inter.Tipo} - {inter.Fecha:dd/MM/yyyy} ({diasReal} días sin respuesta)\n";
                        resultado += $"        {inter.Descripcion}\n";
                    }
                }
                resultado += "\n";
            }
            
            return resultado;
        }

        /// <summary>
        /// Detecta clientes sin ninguna interacción reciente.
        /// </summary>
        public string DetectarClientesSinInteraccion(int diasSinInteraccion = 30)
        {
            _historialAcciones.Push("DetectarClientesSinInteraccion");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden detectar clientes sin interacción";
            
            var clientesSinInteraccion = vendedor.DetectarClientesSinInteraccion(diasSinInteraccion);
            
            if (clientesSinInteraccion.Count == 0)
                return $"✅ Todos los clientes tienen interacciones recientes (últimos {diasSinInteraccion} días).";
            
            string resultado = $"⚠️ **Clientes sin interacción (más de {diasSinInteraccion} días):**\n\n";
            
            foreach (var cliente in clientesSinInteraccion)
            {
                resultado += $"🔸 **{cliente.Nombre} {cliente.Apellido}** (ID: {cliente.Id})\n";
                resultado += $"   Mail: {cliente.Mail} | Tel: {cliente.Telefono}\n";
                
                DateTime? ultimaFecha = null;
                Interaccion? ultimaInter = null;
                
                foreach (var inter in cliente.Interacciones)
                {
                    if (ultimaFecha == null || inter.Fecha > ultimaFecha)
                    {
                        ultimaFecha = inter.Fecha;
                        ultimaInter = inter;
                    }
                }
                
                if (ultimaInter != null)
                {
                    int dias = (DateTime.Now - ultimaFecha.Value).Days;
                    resultado += $"   Última interacción: {ultimaInter.Tipo} - {ultimaFecha.Value:dd/MM/yyyy} ({dias} días atrás)\n";
                    resultado += $"   {ultimaInter.Descripcion}\n";
                }
                else
                {
                    resultado += "   Sin interacciones registradas.\n";
                }
                
                resultado += "\n";
            }
            
            return resultado;
        }

        /// <summary>
        /// Asigna un cliente a otro vendedor.
        /// </summary>
        public string AsignarClienteAOtroVendedor(int idCliente, int idVendedorDestino)
        {
            _historialAcciones.Push("AsignarClienteAOtroVendedor");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden asignar clientes";
            
            return vendedor.AsignarClienteAOtroVendedor(idCliente, idVendedorDestino);
        }

        // ============================================
        // INTERACCIONES
        // ============================================

        /// <summary>
        /// Muestra todas las interacciones de un cliente con filtros opcionales.
        /// </summary>
        public string VerInteraccionesCliente(int idCliente, TipoInteraccion? tipoFiltro = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            _historialAcciones.Push("VerInteraccionesCliente");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden ver interacciones";
            
            var cliente = _repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";
            
            var interacciones = vendedor.VerInteracciones(idCliente, tipoFiltro, fechaDesde, fechaHasta);
            
            if (interacciones.Count == 0)
                return "No se encontraron interacciones para este cliente";
            
            string resultado = $"📋 Interacciones de {cliente.Nombre} {cliente.Apellido} ({interacciones.Count}):\n\n";
            
            foreach (var inter in interacciones)
            {
                resultado += $"🔹 {inter.Tipo} (ID: {inter.Id}) - {inter.Fecha:dd/MM/yyyy HH:mm}\n";
                resultado += $"   {inter.Descripcion}\n";
                
                if (inter is Llamada llamada)
                {
                    resultado += $"   Duración: {llamada.DuracionSegundos}s | Entrante: {(llamada.EsEntrante ? "Sí" : "No")} | Contestada: {(llamada.Contestada ? "Sí" : "No")}\n";
                }
                else if (inter is Reunion reunion)
                {
                    resultado += $"   Ubicación: {reunion.Ubicacion} | Duración: {reunion.DuracionMinutos} min | Estado: {reunion.Estado}\n";
                }
                else if (inter is Cotizacion cotizacion)
                {
                    resultado += $"   Monto: ${cotizacion.Monto} | Estado: {cotizacion.Estado}\n";
                }
                else if (inter is Venta venta)
                {
                    resultado += $"   Producto: {venta.Producto} | Monto: ${venta.Monto} | Cotización: #{venta.IdCotizacion}\n";
                }
                
                resultado += "\n";
            }
            
            return resultado;
        }

        /// <summary>
        /// Registra una llamada asociada a un cliente del vendedor actual.
        /// </summary>
        public string RegistrarLlamada(int idCliente, string descripcion, bool esEntrante, int duracionSegundos, bool contestada)
        {
            _historialAcciones.Push("RegistrarLlamada");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar llamadas";
            
            var cliente = _repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";
            
            var llamada = new Llamada(idCliente, DateTime.Now, descripcion, esEntrante, duracionSegundos, contestada);
            
            return vendedor.RegistrarInteraccion(idCliente, llamada);
        }

        /// <summary>
        /// Registra un mail asociado a un cliente.
        /// </summary>
        public string RegistrarMail(int idCliente, string descripcion, bool esEntrante, string asunto, List<string> destinatarios)
        {
            _historialAcciones.Push("RegistrarMail");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar mails";
            
            var cliente = _repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";
            
            var mail = new Mail(idCliente, DateTime.Now, descripcion, esEntrante, asunto, destinatarios);
            
            return vendedor.RegistrarInteraccion(idCliente, mail);
        }

        /// <summary>
        /// Registra un mensaje instantáneo como interacción.
        /// </summary>
        public string RegistrarMensaje(int idCliente, string descripcion, bool esEntrante, string asunto, List<string> destinatarios)
        {
            _historialAcciones.Push("RegistrarMensaje");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar mensajes";
            
            var cliente = _repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";
            
            var mensaje = new Mensaje(idCliente, DateTime.Now, descripcion, esEntrante, asunto, destinatarios);
            
            return vendedor.RegistrarInteraccion(idCliente, mensaje);
        }

        /// <summary>
        /// Registra una reunión con un cliente.
        /// </summary>
        public string RegistrarReunion(int idCliente, string descripcion, string ubicacion, int duracionMinutos, DateTime fecha)
        {
            _historialAcciones.Push("RegistrarReunion");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar reuniones";
            
            var cliente = _repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";
            
            var reunion = new Reunion(idCliente, fecha, descripcion, ubicacion, duracionMinutos, EstadoReunion.Agendada);
            
            return vendedor.RegistrarInteraccion(idCliente, reunion);
        }

        /// <summary>
        /// Registra una cotización para un cliente.
        /// </summary>
        public string RegistrarCotizacion(int idCliente, string descripcion, decimal monto)
        {
            _historialAcciones.Push("RegistrarCotizacion");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar cotizaciones";
            
            var cliente = _repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != vendedor.Id)
                return "Este cliente no te pertenece";
            
            var cotizacion = new Cotizacion(idCliente, DateTime.Now, descripcion, monto);
            
            return vendedor.RegistrarInteraccion(idCliente, cotizacion);
        }

        /// <summary>
        /// Convierte una cotización existente en una venta.
        /// </summary>
        public string ConvertirCotizacionAVenta(int idCliente, int idCotizacion, string producto)
        {
            _historialAcciones.Push("ConvertirCotizacionAVenta");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden registrar ventas";
            
            return vendedor.ConvertirCotizacionAVenta(idCliente, idCotizacion, producto);
        }

        /// <summary>
        /// Obtiene el total vendido en un rango de fechas.
        /// </summary>
        public string ObtenerTotalVentas(DateTime fechaDesde, DateTime fechaHasta)
        {
            _historialAcciones.Push("ObtenerTotalVentas");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden ver el total de ventas";
            
            decimal total = vendedor.CalcularTotalVentas(fechaDesde, fechaHasta);
            
            return $"💰 Total de ventas del {fechaDesde:dd/MM/yyyy} al {fechaHasta:dd/MM/yyyy}: ${total:N2}";
        }

        // ============================================
        // ETIQUETAS
        // ============================================

        /// <summary>
        /// Crea o asigna una etiqueta a un cliente.
        /// </summary>
        public bool AgregarEtiquetaACliente(int idCliente, string nombreEtiqueta, string? color = null, string? descripcion = null)
        {
            var cliente = _repoClientes.ObtenerClientePorId(idCliente);
            if (cliente == null) return false;

            var etiqueta = _repoEtiquetas._Etiquetas.FirstOrDefault(e => e.Nombre.Equals(nombreEtiqueta, StringComparison.OrdinalIgnoreCase));
            if (etiqueta == null)
            {
                etiqueta = new Etiqueta(nombreEtiqueta, color, descripcion)
                {
                    Id = _repoEtiquetas._Etiquetas.Count + 1
                };
                _repoEtiquetas._Etiquetas.Add(etiqueta);
            }

            return _repoClientes.AgregarEtiquetaACliente(idCliente, etiqueta);
        }

        /// <summary>
        /// Devuelve todas las etiquetas registradas en el sistema.
        /// </summary>
        public List<Etiqueta> ObtenerTodasLasEtiquetas()
        {
            _historialAcciones.Push("ObtenerEtiquetas");
            return _repoEtiquetas._Etiquetas;
        }

        /// <summary>
        /// Permite al vendedor crear una nueva etiqueta.
        /// </summary>
        public string CrearEtiqueta(string nombre, string? color = null, string? descripcion = null)
        {
            _historialAcciones.Push("CrearEtiqueta");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden crear etiquetas";
            
            return vendedor.DefinirEtiqueta(nombre, color, descripcion);
        }

        /// <summary>
        /// Busca clientes que poseen una etiqueta específica.
        /// </summary>
        public string BuscarClientesPorEtiqueta(string nombreEtiqueta)
        {
            _historialAcciones.Push("BuscarClientesPorEtiqueta");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden buscar clientes por etiqueta";
            
            var clientes = vendedor.BuscarClientesPorEtiqueta(nombreEtiqueta);
            
            if (clientes.Count == 0)
                return $"No se encontraron clientes con la etiqueta '{nombreEtiqueta}'";
            
            string respuesta = $"Clientes con etiqueta '{nombreEtiqueta}' ({clientes.Count}):\n\n";
            
            foreach (var c in clientes)
            {
                respuesta += $"ID: {c.Id} - {c.Nombre} {c.Apellido}\n";
                respuesta += $"   Mail: {c.Mail} | Tel: {c.Telefono}\n";
                
                if (c.Etiquetas != null && c.Etiquetas.Count > 0)
                {
                    string etiquetas = string.Join(", ", c.Etiquetas.Select(e => e.Nombre));
                    respuesta += $"   Etiquetas: {etiquetas}\n";
                }
                
                respuesta += "\n";
            }
            
            return respuesta;
        }

        // ============================================
        // USUARIOS (ADMIN)
        // ============================================

        /// <summary>
        /// Crea un vendedor en el sistema. Solo admins pueden hacerlo.
        /// </summary>
        public string CrearVendedor(string nombre, string apellido, string mail, string telefono, string nombreUsuario, string password)
        {
            _historialAcciones.Push("CrearVendedor");
            
            if (UsuarioActual is not Admin)
                return "Solo los administradores pueden crear vendedores";
            
            var vendedor = _repoVendedores.CrearVendedor(nombre, apellido, mail, telefono, nombreUsuario, password);
            
            return $"Vendedor {vendedor.Nombre} {vendedor.Apellido} creado exitosamente";
        }

        /// <summary>
        /// Crea un usuario administrador. Solo admins pueden hacerlo.
        /// </summary>
        public string CrearAdmin(string nombre, string apellido, string mail, string telefono, string nombreUsuario, string password)
        {
            _historialAcciones.Push("CrearAdmin");
            
            if (UsuarioActual is not Admin)
                return "Solo los administradores pueden crear administradores";
            
            var admin = _repoAdmins.CrearAdmin(nombre, apellido, mail, telefono, nombreUsuario, password);
            
            return $"Administrador {admin.Nombre} {admin.Apellido} creado exitosamente";
        }

        /// <summary>
        /// Suspende un usuario existente.
        /// Puede aplicarse a vendedores o administradores.
        /// </summary>
        public string SuspenderUsuario(int id)
        {
            _historialAcciones.Push("SuspenderUsuario");
            
            if (UsuarioActual is not Admin)
                return "Solo los administradores pueden suspender usuarios";

            Usuario usuario = null;

            var vendedor = _repoVendedores._vendedores.FirstOrDefault(v => v.Id == id);
            if (vendedor != null) usuario = vendedor;
            else
            {
                var admin = _repoAdmins._admins.FirstOrDefault(a => a.Id == id);
                if (admin != null) usuario = admin;
            }

            if (usuario == null)
                return "Usuario no encontrado";
            
            if (usuario.Estado == EstadoUsuario.Suspendido)
                return "El usuario ya está suspendido";

            usuario.Estado = EstadoUsuario.Suspendido;
            return $"Usuario {usuario.Nombre} {usuario.Apellido} suspendido exitosamente";
        }

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        public string EliminarUsuario(int id)
        {
            _historialAcciones.Push("EliminarUsuario");
            
            if (UsuarioActual is not Admin)
                return "Solo los administradores pueden eliminar usuarios";

            var vendedor = _repoVendedores._vendedores.FirstOrDefault(v => v.Id == id);
            if (vendedor != null)
            {
                _repoVendedores._vendedores.Remove(vendedor);
                return $"Vendedor {vendedor.Nombre} {vendedor.Apellido} eliminado exitosamente";
            }

            var admin = _repoAdmins._admins.FirstOrDefault(a => a.Id == id);
            if (admin != null)
            {
                _repoAdmins._admins.Remove(admin);
                return $"Administrador {admin.Nombre} {admin.Apellido} eliminado exitosamente";
            }

            return "Usuario no encontrado";
        }

        /// <summary>
        /// Devuelve la lista de todos los vendedores registrados.
        /// </summary>
        public List<Vendedor> ObtenerTodosLosVendedores()
        {
            _historialAcciones.Push("ObtenerVendedores");
            return _repoVendedores._vendedores;
        }

        /// <summary>
        /// Devuelve la lista de todos los administradores registrados.
        /// </summary>
        public List<Admin> ObtenerTodosLosAdmins()
        {
            _historialAcciones.Push("ObtenerAdmins");
            return _repoAdmins._admins;
        }

        // ============================================
        // NAVEGACIÓN
        // ============================================

        /// <summary>
        /// Regresa a la acción anterior registrada en el historial.
        /// </summary>
        public string VolverPasoAnterior()
        {
            if (_historialAcciones.Count > 1)
            {
                _historialAcciones.Pop();
                return _historialAcciones.Peek();
            }
            return null;
        }

        /// <summary>
        /// Muestra el dashboard del vendedor logueado,
        /// incluyendo métricas de actividades recientes.
        /// </summary>
        public string VerDashboard(int diasInteracciones = 7, int cantidadInteracciones = 5)
        {
            _historialAcciones.Push("VerDashboard");
            
            if (UsuarioActual is not Vendedor vendedor)
                return "Solo los vendedores pueden ver el dashboard";
            
            return vendedor.VerDashboard(diasInteracciones, cantidadInteracciones);
        }
    }
}
