using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU17_VerClientesInactivosTests
    {
        [Test]
        public async Task ClienteSinInteracciones()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            // Usamos VerInteraccionesCliente para validar que no tiene interacciones
            var resultado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(resultado, Is.EqualTo("No se encontraron interacciones para este cliente"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task ClienteConInteracciones()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();
            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");

            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            // Registrar una cotización para que tenga interacciones
            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización activa", 500m);
            Assert.That(rCot, Does.Contain("exitosamente"));

            var resultado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(resultado, Does.Contain("Cotizacion"));

            await Task.CompletedTask;
        }
    }
}