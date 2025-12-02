using System;
using NUnit.Framework;
using CrmUcu.Models.Interacciones;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU21_CalcularVentasTests
    {
        private RepositorioCliente _repoCliente;
        private Vendedor _vendedor;

        [SetUp]
        public void Setup()
        {
            _repoCliente = RepositorioCliente.ObtenerInstancia();
            _repoCliente._clientes.Clear();

            _vendedor = new Vendedor(1, "Juan", "Perez", "juan.perez@mail.com", "099000000", "juanp", "password123");
        }

        [TearDown]
        public void TearDown()
        {
            _repoCliente._clientes.Clear();
        }

        [Test]
        public void CalcularTotalVentas()
        {
            // Arrange
            var clienteA = _repoCliente.CrearCliente("clienteA@mail.com", "ClienteA", "Prueba", "099111111", _vendedor.Id);
            var clienteB = _repoCliente.CrearCliente("clienteB@mail.com", "ClienteB", "Prueba", "099222222", _vendedor.Id);

            _vendedor.AgregarCliente(clienteA.Id);
            _vendedor.AgregarCliente(clienteB.Id);

            var ventaInicio = new Venta(clienteA.Id, new DateTime(2024, 12, 01), "Venta inicial", "Producto A", 120m, 1);
            var ventaFin = new Venta(clienteB.Id, new DateTime(2024, 12, 31), "Venta final", "Producto B", 230m, 2);
            var ventaFuera = new Venta(clienteA.Id, new DateTime(2024, 11, 30), "Fuera de rango", "Producto C", 999m, 3);

            clienteA.Interacciones.Add(ventaInicio);
            clienteB.Interacciones.Add(ventaFin);
            clienteA.Interacciones.Add(ventaFuera);

            var fechaDesde = new DateTime(2024, 12, 1);
            var fechaHasta = new DateTime(2024, 12, 31);

            // Act
            decimal total = _vendedor.CalcularTotalVentas(fechaDesde, fechaHasta);

            // Assert
            Assert.That(total, Is.EqualTo(350m));
        }

        [Test]
        public void CalcularTotalVentasSoloClientePropio()
        {
            // Arrange
            var clientePropio = _repoCliente.CrearCliente("propio@mail.com", "Propio", "Perez", "099333333", _vendedor.Id);
            var clienteAjeno = _repoCliente.CrearCliente("ajeno@mail.com", "Ajeno", "Gomez", "099444444", 99);

            _vendedor.AgregarCliente(clientePropio.Id);

            var ventaPropia = new Venta(clientePropio.Id, new DateTime(2024, 10, 15), "Venta propia", "Producto X", 180m, 4);
            var ventaAjena = new Venta(clienteAjeno.Id, new DateTime(2024, 10, 15), "Venta ajena", "Producto Y", 300m, 5);

            clientePropio.Interacciones.Add(ventaPropia);
            clienteAjeno.Interacciones.Add(ventaAjena);

            var fechaDesde = new DateTime(2024, 10, 1);
            var fechaHasta = new DateTime(2024, 10, 31);

            // Act
            decimal total = _vendedor.CalcularTotalVentas(fechaDesde, fechaHasta);

            // Assert
            Assert.That(total, Is.EqualTo(180m));
        }
    }
}
