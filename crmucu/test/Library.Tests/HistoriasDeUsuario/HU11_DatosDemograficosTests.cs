using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading.Tasks;
using CrmUcu.Core;
using CrmUcu.Models.Personas;
using CrmUcu.Models.Enums;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU11_RegistrarDatosDemograficosTests
    {
        [Test]
        public async Task ClienteExistente()
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

            var fechaNacimiento = DateTime.ParseExact("15/06/1990", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var resultado = interfaz.RegistrarDatosDemograficos(cliente.Id, "Femenino", fechaNacimiento);

            Assert.That(resultado, Does.StartWith("✅"));
            Assert.That(resultado, Does.Contain("exitosamente"));
            Assert.That(cliente.Genero, Is.EqualTo(Genero.Femenino));
            Assert.That(cliente.FechaNacimiento, Is.EqualTo(fechaNacimiento));

            await Task.CompletedTask;
        }

        [Test]
        public async Task ClienteInexistente()
        {
            var repoClientes = RepositorioCliente.ObtenerInstancia();
            repoClientes.ObtenerTodos().Clear();

            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vendedor@test.com", "Test", "Vendedor", "099000000", "testuser", "testpass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("testuser", "testpass");

            var fechaNacimiento = DateTime.ParseExact("15/06/1990", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var resultado = interfaz.RegistrarDatosDemograficos(999, "Masculino", fechaNacimiento);

            Assert.That(resultado, Is.EqualTo("❌ Cliente no encontrado"));

            await Task.CompletedTask;
        }

        [Test]
        public async Task NoLogueado()
        {
            var interfaz = new Interfaz();

            var fechaNacimiento = DateTime.ParseExact("15/06/1990", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var resultado = interfaz.RegistrarDatosDemograficos(1, "Masculino", fechaNacimiento);

            Assert.That(resultado, Is.EqualTo("❌ Debes iniciar sesión primero."));

            await Task.CompletedTask;
        }

        [Test]
        public async Task NoEsVendedor()
        {
            var interfaz = new Interfaz();

            var admin = new Admin(1, "admin@test.com", "Admin", "Uno", "099999999", "adminuser", "adminpass");
            RepositorioAdmin.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioAdmin.ObtenerInstancia().ObtenerTodos().Add(admin);

            interfaz.IniciarSesion("adminuser", "adminpass");

            var fechaNacimiento = DateTime.ParseExact("15/06/1990", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var resultado = interfaz.RegistrarDatosDemograficos(1, "Masculino", fechaNacimiento);

            Assert.That(resultado, Is.EqualTo("❌ Solo los vendedores pueden registrar datos extra."));

            await Task.CompletedTask;
        }

        [Test]
        public async Task ArgumentosInsuficientes()
        {
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vendedor@test.com", "Test", "Vendedor", "099000000", "testuser", "testpass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("testuser", "testpass");

            // Simulamos fecha faltante con DateTime.MinValue
            var resultado = interfaz.RegistrarDatosDemograficos(1, "Masculino", DateTime.MinValue);

            Assert.That(resultado, Does.StartWith("❌ Faltan datos."));

            await Task.CompletedTask;
        }

        [Test]
        public async Task FormatoFechaInvalido()
        {
            var interfaz = new Interfaz();

            var vendedor = new Vendedor(1, "vendedor@test.com", "Test", "Vendedor", "099000000", "testuser", "testpass");
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Clear();
            RepositorioVendedor.ObtenerInstancia().ObtenerTodos().Add(vendedor);

            interfaz.IniciarSesion("testuser", "testpass");

            // Simulamos fecha inválida con un valor imposible
            var fechaInvalida = new DateTime(0001, 01, 01); // DateTime.MinValue

            var resultado = interfaz.RegistrarDatosDemograficos(1, "Masculino", fechaInvalida);

            Assert.That(resultado, Does.StartWith("❌ Formato de fecha inválido"));

            await Task.CompletedTask;
        }
    }
}
