using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU14_RegistrarCotizacionInterfazTests
    {
        [Test]
        public async Task RegistrarCotizacion()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(
                1,
                "nacho@gmail.com",
                "Nacho",
                "Silva",
                "098xxxyyy",
                "Tachoviendo",
                "123nacho");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("Tachoviendo", "123nacho");

            var cliente = repoClientes.CrearCliente("flor@gmail.com", "Florencia", "Ferreira", "098111222", vendedor.Id);

            var resultado = interfaz.RegistrarCotizacion(cliente.Id, "Cotización para sistema de gestión completo", 15000.50m);

            Assert.That(resultado, Does.Contain("exitosamente"));
            Assert.That(cliente.Cotizaciones.Count, Is.EqualTo(1));
            Assert.That(cliente.Cotizaciones[0].Descripcion, Is.EqualTo("Cotización para sistema de gestión completo"));
            Assert.That(cliente.Cotizaciones[0].Monto, Is.EqualTo(15000.50m));

            await Task.CompletedTask;
        }

        [Test]
        public async Task RegistrarCotizacionClienteInexistente()
        {
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var resultado = interfaz.RegistrarCotizacion(999, "Cotización inválida", 1000m);

            Assert.That(resultado, Is.EqualTo("Cliente no encontrado"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task RegistrarCotizacionNoLogueado()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var resultado = interfaz.RegistrarCotizacion(cliente.Id, "Cotización sin login", 500m);

            Assert.That(resultado, Is.EqualTo("Solo los vendedores pueden registrar cotizaciones"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task RegistrarCotizacionClienteNoPerteneceAlVendedor()
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

            var resultado = interfaz.RegistrarCotizacion(cliente.Id, "Intento inválido", 999m);

            Assert.That(resultado, Is.EqualTo("Este cliente no te pertenece"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerInteraccionesClienteIncluyeCotizacion()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var r = interfaz.RegistrarCotizacion(cliente.Id, "Cotización visible", 1234.56m);
            Assert.That(r, Does.Contain("exitosamente"));

            var listado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(listado, Does.Contain("📋 **Interacciones de Cli ente"));
            Assert.That(listado, Does.Contain("Cotizacion"));
            Assert.That(listado, Does.Contain("Monto: $1234.56"));

            await Task.CompletedTask;
        }
    }
}
