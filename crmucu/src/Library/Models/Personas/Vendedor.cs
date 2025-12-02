using CrmUcu.Models.Enums;
using CrmUcu.Repositories;
using CrmUcu.Models.Interacciones;
using CrmUcu.Models.Utils;

namespace CrmUcu.Models.Personas
{
    /// <summary>
    /// Representa un vendedor del CRM.
    /// Gestiona sus clientes, interacciones, etiquetas y ventas.
    /// </summary>
    public class Vendedor : Usuario
    {
        public List<int> Clientes { get; private set; } = new List<int>();

        public List<int> ObtenerClientes()
        {
            return Clientes;
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Vendedor() : base() {
        }

        /// <summary>
        /// Crea un vendedor con sus datos básicos y credenciales.
        /// </summary>
        public Vendedor(int id, string nombre, string apellido, string mail, string telefono, string nombreUsuario, string password ) :
            base(id, nombre, apellido, mail, telefono, nombreUsuario, password)
        {

        }

        public void AgregarCliente(int cliente)
        {
            Clientes.Add(cliente);
        }

        /// <summary>
        /// Crea un cliente y lo asocia a este vendedor.
        /// </summary>
        public void CrearCliente(string nombre, string apellido, string mail, string telefono)
        {
            var repo = RepositorioCliente.ObtenerInstancia();
            var cliente = repo.CrearCliente(nombre, apellido, mail, telefono, this.Id);
            AgregarCliente(cliente.Id);
        }

        /// <summary>
        /// Modifica los datos de un cliente propio, solo en los campos no nulos.
        /// </summary>
        public string ModificarCliente(int idCliente, string? nuevoNombre = null, string? nuevoApellido = null, 
                               string? nuevoMail = null, string? nuevoTelefono = null)
        {
            var repo = RepositorioCliente.ObtenerInstancia();
            var cliente = repo.BuscarPorId(idCliente);
            
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != this.Id)
                return "Este cliente no te pertenece";
            
            if (nuevoNombre != null)
                cliente.Nombre = nuevoNombre;
                
            if (nuevoApellido != null)
                cliente.Apellido = nuevoApellido;
                
            if (nuevoMail != null)
                cliente.Mail = nuevoMail;
                
            if (nuevoTelefono != null)
                cliente.Telefono = nuevoTelefono;
            
            return "Cliente modificado exitosamente";
        }

        /// <summary>
        /// Elimina un cliente propio del repositorio y de la lista del vendedor.
        /// </summary>
        public string EliminarCliente(int id)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.BuscarPorId(id);
            
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != this.Id)
                return "Este cliente no te pertenece";
            
            Clientes.Remove(id);
            repoClientes.EliminarCliente(id);
            
            return $"Cliente {cliente.Nombre} {cliente.Apellido} eliminado exitosamente";
        }

        /// <summary>
        /// Devuelve los clientes sin interacciones en los últimos X días.
        /// </summary>
        public List<Cliente> DetectarClientesSinInteraccion(int diasSinInteraccion)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            List<Cliente> clientesSinInteraccion = new List<Cliente>();
            DateTime fechaLimite = DateTime.Now.AddDays(-diasSinInteraccion);
            
            for (int i = 0; i < Clientes.Count; i++)
            {
                int idCliente = Clientes[i];
                Cliente cliente = repoClientes.BuscarPorId(idCliente);
                
                if (cliente == null) continue;
                
                if (cliente.Interacciones.Count == 0)
                {
                    clientesSinInteraccion.Add(cliente);
                    continue;
                }
                
                DateTime? ultimaInteraccion = null;
                
                for (int j = 0; j < cliente.Interacciones.Count; j++)
                {
                    Interaccion inter = cliente.Interacciones[j];
                    
                    if (ultimaInteraccion == null || inter.Fecha > ultimaInteraccion)
                    {
                        ultimaInteraccion = inter.Fecha;
                    }
                }
                
                if (ultimaInteraccion != null && ultimaInteraccion < fechaLimite)
                {
                    clientesSinInteraccion.Add(cliente);
                }
            }
            
            return clientesSinInteraccion;
        }

        /// <summary>
        /// Registra una interacción en un cliente.
        /// </summary>
        public string RegistrarInteraccion(int idCliente, Interaccion interaccion)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.BuscarPorId(idCliente);
            
            if (cliente == null)
            {
                return "Cliente no encontrado";
            }
            
            cliente.Interacciones.Add(interaccion);
            
            return $"{interaccion.Tipo} registrada exitosamente";
        }

        /// <summary>
        /// Agrega una nota a una interacción específica de un cliente.
        /// </summary>
        public string AgregarNota(int idCliente, int idInteraccion, string textoNota)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.BuscarPorId(idCliente);
            
            if (cliente == null)
            {
                return "Cliente no encontrado";
            }
            
            Interaccion? interaccion = null;
            foreach (var inter in cliente.Interacciones)
            {
                if (inter.Id == idInteraccion)
                {
                    interaccion = inter;
                    break;
                }
            }
            
            if (interaccion == null)
            {
                return "Interacción no encontrada";
            }
            
            Nota nota = new Nota(textoNota);
            interaccion.Notas.Add(nota);
            
            return $"Nota agregada exitosamente a la {interaccion.Tipo}";
        }

        /// <summary>
        /// Crea una etiqueta nueva en el sistema.
        /// </summary>
        public string DefinirEtiqueta(string nombre, string? color = null, string? descripcion = null)
        {
            var repoEtiquetas = RepositorioEtiqueta.ObtenerInstancia();
            Etiqueta etiqueta = repoEtiquetas.CrearEtiqueta(nombre, color, descripcion);
            return $"Etiqueta '{etiqueta.Nombre}' creada exitosamente con ID: {etiqueta.Id}";
        }

        /// <summary>
        /// Obtiene las interacciones de un cliente con filtros opcionales.
        /// </summary>
        public List<Interaccion> VerInteracciones(int idCliente, TipoInteraccion? tipoFiltro = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.BuscarPorId(idCliente);
            
            if (cliente == null)
            {
                return new List<Interaccion>();
            }
            
            List<Interaccion> resultado = new List<Interaccion>();
            
            for (int i = 0; i < cliente.Interacciones.Count; i++)
            {
                Interaccion interaccion = cliente.Interacciones[i];
                bool cumpleFiltros = true;
                
                if (tipoFiltro != null && interaccion.Tipo != tipoFiltro)
                {
                    cumpleFiltros = false;
                }
                
                if (fechaDesde != null && interaccion.Fecha < fechaDesde)
                {
                    cumpleFiltros = false;
                }
                
                if (fechaHasta != null && interaccion.Fecha > fechaHasta)
                {
                    cumpleFiltros = false;
                }
                
                if (cumpleFiltros)
                {
                    resultado.Add(interaccion);
                }
            }
            
            return resultado;
        }

        /// <summary>
        /// Devuelve los clientes con interacciones entrantes sin respuesta
        /// desde hace más de X días.
        /// </summary>
        public List<Cliente> DetectarContactoPendiente(int diasSinRespuesta = 3)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            List<Cliente> clientesPendientes = new List<Cliente>();
            DateTime fechaLimite = DateTime.Now.AddDays(-diasSinRespuesta);
            
            for (int i = 0; i < Clientes.Count; i++)
            {
                int idCliente = Clientes[i];
                Cliente cliente = repoClientes.BuscarPorId(idCliente);
                
                if (cliente == null) continue;
                
                bool tienePendientes = false;
                
                for (int j = 0; j < cliente.Interacciones.Count; j++)
                {
                    Interaccion inter = cliente.Interacciones[j];
                    
                    if (inter.Fecha >= fechaLimite) continue;
                    
                    if (inter is IRespondible respondible)
                    {
                        if (respondible.EsEntrante && !respondible.Respondida)
                        {
                            tienePendientes = true;
                            break;
                        }
                    }
                }
                
                if (tienePendientes && !clientesPendientes.Contains(cliente))
                {
                    clientesPendientes.Add(cliente);
                }
            }
            
            return clientesPendientes;
        }

        /// <summary>
        /// Reasigna un cliente propio a otro vendedor.
        /// </summary>
        public string AsignarClienteAOtroVendedor(int idCliente, int idVendedorDestino)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            
            var cliente = repoClientes.BuscarPorId(idCliente);
            if (cliente == null)
            {
                return "Cliente no encontrado";
            }
            
            if (!Clientes.Contains(idCliente))
            {
                return "El cliente no pertenece a este vendedor";
            }
            
            var vendedorDestino = repoVendedores.BuscarPorId(idVendedorDestino);
            if (vendedorDestino == null)
            {
                return "Vendedor destino no encontrado";
            }
            
            if (idVendedorDestino == this.Id)
            {
                return "No puedes asignar un cliente a ti mismo";
            }
            
            Clientes.Remove(idCliente);
            vendedorDestino.Clientes.Add(idCliente);
            cliente.IdVendedor = idVendedorDestino;
            
            return $"Cliente {cliente.Nombre} {cliente.Apellido} asignado exitosamente a {vendedorDestino.Nombre} {vendedorDestino.Apellido}";
        }

        /// <summary>
        /// Genera un dashboard de resumen para el vendedor:
        /// clientes totales, interacciones recientes y reuniones próximas.
        /// </summary>
        public string VerDashboard(int diasInteracciones = 7, int cantidadInteracciones = 5)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var dashboard = new System.Text.StringBuilder();
            
            dashboard.AppendLine("========================================");
            dashboard.AppendLine("           DASHBOARD VENDEDOR           ");
            dashboard.AppendLine("========================================\n");
            
            dashboard.AppendLine($"📊 CLIENTES TOTALES: {Clientes.Count}\n");
            
            dashboard.AppendLine($"📝 INTERACCIONES RECIENTES (últimos {diasInteracciones} días):");
            DateTime fechaLimite = DateTime.Now.AddDays(-diasInteracciones);
            List<Interaccion> interaccionesRecientes = new List<Interaccion>();
            
            for (int i = 0; i < Clientes.Count; i++)
            {
                var cliente = repoClientes.BuscarPorId(Clientes[i]);
                if (cliente == null) continue;
                
                for (int j = 0; j < cliente.Interacciones.Count; j++)
                {
                    if (cliente.Interacciones[j].Fecha >= fechaLimite)
                    {
                        interaccionesRecientes.Add(cliente.Interacciones[j]);
                    }
                }
            }
            
            interaccionesRecientes.Sort((a, b) => b.Fecha.CompareTo(a.Fecha));
            int cantidadMostrar = Math.Min(cantidadInteracciones, interaccionesRecientes.Count);
            
            if (cantidadMostrar == 0)
            {
                dashboard.AppendLine("   No hay interacciones recientes");
            }
            else
            {
                for (int i = 0; i < cantidadMostrar; i++)
                {
                    var inter = interaccionesRecientes[i];
                    var cliente = repoClientes.BuscarPorId(inter.IdCliente);
                    dashboard.AppendLine($"   • {inter.Tipo} - {inter.Fecha:dd/MM/yyyy HH:mm}");
                    dashboard.AppendLine($"     Cliente: {cliente?.Nombre} {cliente?.Apellido}");
                    dashboard.AppendLine($"     {inter.Descripcion}");
                }
            }
            
            dashboard.AppendLine($"\n📅 REUNIONES PRÓXIMAS:");
            List<Reunion> reunionesProximas = new List<Reunion>();
            DateTime ahora = DateTime.Now;
            
            for (int i = 0; i < Clientes.Count; i++)
            {
                var cliente = repoClientes.BuscarPorId(Clientes[i]);
                if (cliente == null) continue;
                
                for (int j = 0; j < cliente.Interacciones.Count; j++)
                {
                    if (cliente.Interacciones[j] is Reunion reunion)
                    {
                        if (reunion.Fecha > ahora && reunion.Estado == EstadoReunion.Agendada)
                        {
                            reunionesProximas.Add(reunion);
                        }
                    }
                }
            }
            
            reunionesProximas.Sort((a, b) => a.Fecha.CompareTo(b.Fecha));
            
            if (reunionesProximas.Count == 0)
            {
                dashboard.AppendLine("   No hay reuniones próximas agendadas");
            }
            else
            {
                for (int i = 0; i < reunionesProximas.Count; i++)
                {
                    var reunion = reunionesProximas[i];
                    var cliente = repoClientes.BuscarPorId(reunion.IdCliente);
                    dashboard.AppendLine($"   • {reunion.Fecha:dd/MM/yyyy HH:mm} - {reunion.Descripcion}");
                    dashboard.AppendLine($"     Cliente: {cliente?.Nombre} {cliente?.Apellido}");
                    dashboard.AppendLine($"     Ubicación: {reunion.Ubicacion}");
                    dashboard.AppendLine($"     Duración: {reunion.DuracionMinutos} min");
                }
            }
            
            dashboard.AppendLine("\n========================================");
            
            return dashboard.ToString();
        }

        /// <summary>
        /// Convierte una cotización pendiente en una venta para un cliente propio.
        /// </summary>
        public string ConvertirCotizacionAVenta(int idCliente, int idCotizacion, string producto)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var cliente = repoClientes.BuscarPorId(idCliente);
            
            if (cliente == null)
                return "Cliente no encontrado";
            
            if (cliente.IdVendedor != this.Id)
                return "Este cliente no te pertenece";
            
            Cotizacion? cotizacion = null;
            
            for (int i = 0; i < cliente.Interacciones.Count; i++)
            {
                if (cliente.Interacciones[i] is Cotizacion cot && cot.Id == idCotizacion)
                {
                    cotizacion = cot;
                    break;
                }
            }
            
            if (cotizacion == null)
                return "Cotización no encontrada";
            
            if (cotizacion.Estado != EstadoCotizacion.Pendiente)
                return $"Esta cotización ya fue {cotizacion.Estado.ToString().ToLower()}";
            
            var venta = new Venta(
                idCliente: idCliente,
                fecha: DateTime.Now,
                descripcion: $"Venta de {producto} - Cotización #{cotizacion.Id}",
                producto: producto,
                monto: cotizacion.Monto,
                idCotizacion: cotizacion.Id
            );
            
            cotizacion.Estado = EstadoCotizacion.Aceptada;
            cliente.Interacciones.Add(venta);
            
            return $"Venta registrada exitosamente: {producto} - ${cotizacion.Monto}";
        }

        /// <summary>
        /// Calcula el total vendido por el vendedor en un rango de fechas.
        /// </summary>
        public decimal CalcularTotalVentas(DateTime fechaDesde, DateTime fechaHasta)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            decimal totalVentas = 0;
            
            for (int i = 0; i < Clientes.Count; i++)
            {
                int idCliente = Clientes[i];
                Cliente cliente = repoClientes.BuscarPorId(idCliente);
                
                if (cliente == null) continue;
                
                for (int j = 0; j < cliente.Interacciones.Count; j++)
                {
                    if (cliente.Interacciones[j] is Venta venta)
                    {
                        if (venta.Fecha >= fechaDesde && venta.Fecha <= fechaHasta)
                        {
                            totalVentas += venta.Monto;
                        }
                    }
                }
            }
            
            return totalVentas;
        }

        /// <summary>
        /// Devuelve los clientes de este vendedor que tienen una etiqueta dada.
        /// </summary>
        public List<Cliente> BuscarClientesPorEtiqueta(string nombreEtiqueta)
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            var todosConEtiqueta = repoClientes.BuscarPorEtiqueta(nombreEtiqueta);
            
            List<Cliente> misClientesConEtiqueta = new List<Cliente>();
            
            for (int i = 0; i < todosConEtiqueta.Count; i++)
            {
                if (Clientes.Contains(todosConEtiqueta[i].Id))
                {
                    misClientesConEtiqueta.Add(todosConEtiqueta[i]);
                }
            }
            
            return misClientesConEtiqueta;
        }
    }
}
