using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU16_VerHistorialInteraccionesTests
    {
        [Test]
        public async Task VerInteraccionesCliente()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(
                1, "nacho@gmail.com", "Nacho", "Silva",
                "098xxxyyy", "Tachoviendo", "123nacho");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("Tachoviendo", "123nacho");

            var cliente = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);

            // Registrar distintas interacciones
            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);
            Assert.That(rCot, Does.Contain("exitosamente"));

            var cotizacion = cliente.Cotizaciones[0];
            var rVenta = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");
            Assert.That(rVenta, Does.Contain("exitosamente"));

            // Ver historial
            var listado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(listado, Does.Contain("📋 **Interacciones de Florencia Ferreira"));
            Assert.That(listado, Does.Contain("Cotizacion"));
            Assert.That(listado, Does.Contain("Venta"));
            Assert.That(listado, Does.Contain("Laptop Dell"));
            Assert.That(listado, Does.Contain($"Cotización: #{cotizacion.Id}"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerInteraccionesClienteSinInteracciones()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var listado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(listado, Is.EqualTo("No se encontraron interacciones para este cliente"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerInteraccionesClienteClienteInexistente()
        {
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var listado = interfaz.VerInteraccionesCliente(999);

            Assert.That(listado, Is.EqualTo("Cliente no encontrado"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerInteraccionesClienteNoLogueado()
        {
            var interfaz = new Interfaz();

            var listado = interfaz.VerInteraccionesCliente(1);

            Assert.That(listado, Is.EqualTo("Solo los vendedores pueden ver interacciones"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerInteraccionesClienteNoPerteneceAlVendedor()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor1 = new Vendedor(1, "vend1@test.com", "Vend", "Uno", "099000001", "user1", "pass1");
            var vendedor2 = new Vendedor(2, "vend2@test.com", "Vend", "Dos", "099000002", "user2", "pass2");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor1);
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor2);

            interfaz.IniciarSesion("user1", "pass1");

            var cliente = repoClientes.CrearCliente("otro@test.com", "Otro", "Cliente", "099222222", vendedor2.Id);

            var listado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(listado, Is.EqualTo("Este cliente no te pertenece"));

            await Task.CompletedTask;
        }
    }
}