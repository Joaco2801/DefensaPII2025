using System;
using NUnit.Framework;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Models.Interacciones;

namespace Library.Tests
{
    [TestFixture]
    public class HU23_VerVentasDisminuidasTests
    {
        [Test]
        public async Task VerVentasDisminuidasSinSesion()
        {
            var interfaz = new Interfaz();

            var resultado = interfaz.VerVentasDisminuidas();

            Assert.That(resultado, Does.Contain("Debes iniciar sesión primero"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerVentasDisminuidasUsuarioNoAdmin()
        {
            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            repoVendedores.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");
            repoVendedores.ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("userVend", "passVend");

            var resultado = interfaz.VerVentasDisminuidas();

            Assert.That(resultado, Does.Contain("Solo los administradores"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerVentasDisminuidasSinDisminucion()
        {
            var repoAdmins = RepositorioAdmin.ObtenerInstancia();
            repoAdmins.ObtenerTodos().Clear();

            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            repoVendedores.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var admin = new Admin(1, "admin@test.com", "Admin", "Root", "099999999", "adminUser", "adminPass");
            repoAdmins.ObtenerTodos().Add(admin);

            interfaz.IniciarSesion("adminUser", "adminPass");

            var vendedor = new Vendedor(2, "vend@test.com", "Test", "Vendedor", "099000000", "userVend", "passVend");
            vendedor.Ventas.Add(new Venta(DateTime.Now.AddMonths(-1), 100));
            vendedor.Ventas.Add(new Venta(DateTime.Now, 100));
            repoVendedores.ObtenerTodos().Add(vendedor);

            var resultado = interfaz.VerVentasDisminuidas();

            Assert.That(resultado, Does.Contain("No se encontraron vendedores"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task VerVentasDisminuidasConDisminucion()
        {
            var repoAdmins = RepositorioAdmin.ObtenerInstancia();
            repoAdmins.ObtenerTodos().Clear();

            var repoVendedores = RepositorioVendedor.ObtenerInstancia();
            repoVendedores.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var admin = new Admin(1, "admin@test.com", "Admin", "Root", "099999999", "adminUser", "adminPass");
            repoAdmins.ObtenerTodos().Add(admin);

            interfaz.IniciarSesion("adminUser", "adminPass");

            var vendedor = new Vendedor(2, "vend@test.com", "Maria", "Gomez", "099000000", "userVend", "passVend");
            vendedor.Ventas.Add(new Venta(DateTime.Now.AddMonths(-1), 200));
            vendedor.Ventas.Add(new Venta(DateTime.Now, 50));
            repoVendedores.ObtenerTodos().Add(vendedor);

            var resultado = interfaz.VerVentasDisminuidas();

            Assert.That(resultado, Does.Contain("Vendedores con disminución de ventas"));
            Assert.That(resultado, Does.Contain("Maria Gomez"));

            await Task.CompletedTask;
        }
    }
}
