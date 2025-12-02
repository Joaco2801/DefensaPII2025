using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU22_VerDashboardTests
    {
        [Test]
        public async Task VerDashboard()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            repoVendedores.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");
            repoVendedores.ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);
            Assert.That(rCot, Does.Contain("exitosamente"));

            var cotizacion = cliente.Cotizaciones[0];
            var rVenta = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");
            Assert.That(rVenta, Does.Contain("exitosamente"));

            // Método real de la Interfaz para ver Dashboard
            var dashboard = interfaz.VerDashboard();

            Assert.That(dashboard, Does.Contain("Clientes: 1"));
            Assert.That(dashboard, Does.Contain("Cotizaciones: 1"));
            Assert.That(dashboard, Does.Contain("Ventas: 1"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerDashboardSinSesion()
        {
            var interfaz = new Interfaz();

            var dashboard = interfaz.VerDashboard();

            Assert.That(dashboard, Is.EqualTo("Debes iniciar sesión primero"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerDashboardVendedorSinClientes()
        {
            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            repoVendedores.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");
            repoVendedores.ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var dashboard = interfaz.VerDashboard();

            Assert.That(dashboard, Does.Contain("Clientes: 0"));
            Assert.That(dashboard, Does.Contain("Cotizaciones: 0"));
            Assert.That(dashboard, Does.Contain("Ventas: 0"));

            await Task.CompletedTask;
        }
    }
}
