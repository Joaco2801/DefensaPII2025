using NUnit.Framework;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU15_RegistrarVenta
    {
        [Test]
        public async Task ConvertirCotizacionAVenta()
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

            // Registrar cotización primero
            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);
            Assert.That(rCot, Does.Contain("exitosamente"));
            var cotizacion = cliente.Cotizaciones[0];

            // Convertir cotización a venta
            var rVenta = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");

            Assert.That(rVenta, Does.Contain("exitosamente"));
            Assert.That(cliente.Ventas.Count, Is.EqualTo(1));
            Assert.That(cliente.Ventas[0].Producto, Is.EqualTo("Laptop Dell"));
            Assert.That(cliente.Ventas[0].Monto, Is.EqualTo(1200m));
            Assert.That(cliente.Ventas[0].IdCotizacion, Is.EqualTo(cotizacion.Id));

            await Task.CompletedTask;
        }

        [Test]
        public async Task ConvertirCotizacionAVentaClienteInexistente()
        {
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var resultado = interfaz.ConvertirCotizacionAVenta(999, 1, "Laptop Dell");

            Assert.That(resultado, Is.EqualTo("Cliente no encontrado"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task ConvertirCotizacionAVentaNoLogueado()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);
            Assert.That(rCot, Does.Contain("exitosamente"));
            var cotizacion = cliente.Cotizaciones[0];

            var resultado = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");

            Assert.That(resultado, Is.EqualTo("Solo los vendedores pueden registrar ventas"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task ConvertirCotizacionAVentaClienteNoPerteneceAlVendedor()
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

            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización ajena", 999m);
            Assert.That(rCot, Does.Contain("exitosamente"));
            var cotizacion = cliente.Cotizaciones[0];

            var resultado = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");

            Assert.That(resultado, Is.EqualTo("Este cliente no te pertenece"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerInteraccionesClienteIncluyeVenta()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "user", "pass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("user", "pass");

            var cliente = repoClientes.CrearCliente("cli@test.com", "Cli", "ente", "099111111", vendedor.Id);

            var rCot = interfaz.RegistrarCotizacion(cliente.Id, "Cotización Laptop Dell", 1200m);
            Assert.That(rCot, Does.Contain("exitosamente"));
            var cotizacion = cliente.Cotizaciones[0];

            var rVenta = interfaz.ConvertirCotizacionAVenta(cliente.Id, cotizacion.Id, "Laptop Dell");
            Assert.That(rVenta, Does.Contain("exitosamente"));

            var listado = interfaz.VerInteraccionesCliente(cliente.Id);

            Assert.That(listado, Does.Contain("📋 **Interacciones de Cli ente"));
            Assert.That(listado, Does.Contain("Venta"));
            Assert.That(listado, Does.Contain("Producto: Laptop Dell"));
            Assert.That(listado, Does.Contain($"Cotización: #{cotizacion.Id}"));

            await Task.CompletedTask;
        }
    }
}