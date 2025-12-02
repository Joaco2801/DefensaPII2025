using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class CRMTests
    {
        [Test]
        public async Task RegistrarCotizacion()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var resultado = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);

            Assert.That(resultado, Does.Contain("exitosamente"));
            Assert.That(cliente.Cotizaciones.Count, Is.EqualTo(1));

            await Task.CompletedTask;
        }

        [Test]
        public async Task CotizacionAVentaClienteExistente()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);
            Assert.That(rCot, Does.Contain("exitosamente"));

            var cotizacion = cliente.Cotizaciones[0];
            var rVenta = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");

            Assert.That(rVenta, Does.Contain("exitosamente"));
            Assert.That(cliente.Ventas.Count, Is.EqualTo(1));

            await Task.CompletedTask;
        }

        [Test]
        public async Task InteraccionesClienteSinInteracciones()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var resultado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(resultado, Is.EqualTo("No se encontraron interacciones para este cliente"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task IniciarYCerrarSesionAdmin()
        {
            var repoAdmins = RepositorioAdmin.ObtenerInstancia();
            repoAdmins.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var admin = new Admin(1, "admin@test.com", "Admin", "Uno", "099111111", "userAdmin", "passAdmin");
            repoAdmins.ObtenerTodos().Add(admin);

            // Iniciar sesión (devuelve void, verificamos estado)
            interfaz.IniciarSesion("userAdmin", "passAdmin");
            Assert.That(interfaz.EstaLogueado, Is.True);
            Assert.That(interfaz.UsuarioActual, Is.EqualTo(admin));

            // Cerrar sesión (devuelve void, verificamos estado)
            interfaz.CerrarSesion();
            Assert.That(interfaz.EstaLogueado, Is.False);

            await Task.CompletedTask;
        }

        [Test]
        public async Task ContactosPendientesClienteSinInteraccion()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var resultado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(resultado, Is.EqualTo("No se encontraron interacciones para este cliente"));

            await Task.CompletedTask;
        }
    }
}
