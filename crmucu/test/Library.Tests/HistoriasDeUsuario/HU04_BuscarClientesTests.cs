using NUnit.Framework;
using CrmUcu.Models.Personas;
using CrmUcu.Repositories;

namespace Library.Tests
{
    [TestFixture]
    public class HU04_BuscarClienteTests
    {
        private RepositorioCliente repoCliente;
        private Vendedor vendedor;
        
        [SetUp]
        public void Setup()
        {
            // Limpiar el repositorio antes de cada test
            repoCliente = RepositorioCliente.ObtenerInstancia();
            repoCliente._clientes.Clear();
            
            // Crear vendedor
            vendedor = new Vendedor(1, "vendedor@gmail.com", "Carlos", "López", "099111222", "carlos_v", "pass123");
            
            // Crear clientes de prueba - ORDEN CORRECTO: nombre, apellido, mail, telefono
            vendedor.CrearCliente("Juan", "Pérez", "juan@mail.com", "099123456");
            vendedor.CrearCliente("María", "González", "maria@mail.com", "099987654");
            vendedor.CrearCliente("Pedro", "Martínez", "pedro@mail.com", "098555666");
            vendedor.CrearCliente("Ana", "Juárez", "ana@mail.com", "097444333");
        }
                
        [Test]
        public void buscarPorNombre()
        {
            // Act
            var resultados = repoCliente.BuscarPor("Juan");
            
            // Assert
            Assert.That(resultados, Is.Not.Null);
            Assert.That(resultados.Count, Is.EqualTo(1));
            Assert.That(resultados[0].Nombre, Is.EqualTo("Juan"));
        }
        
        [Test]
        public void buscarPorApellidoa()
        {
            // Act
            var resultados = repoCliente.BuscarPor("González");
            
            // Assert
            Assert.That(resultados, Is.Not.Null);
            Assert.That(resultados.Count, Is.EqualTo(1));
            Assert.That(resultados[0].Apellido, Is.EqualTo("González"));
        }
        
        [Test]
        public void buscarPorTelefono()
        {
            // Act
            var resultados = repoCliente.BuscarPor("099123456");
            
            // Assert
            Assert.That(resultados, Is.Not.Null);
            Assert.That(resultados.Count, Is.EqualTo(1));
            Assert.That(resultados[0].Telefono, Is.EqualTo("099123456"));
        }
        
        [Test]
        public void buscarPorEmail()
        {
            // Act
            var resultados = repoCliente.BuscarPor("maria@mail.com");
            
            // Assert
            Assert.That(resultados, Is.Not.Null);
            Assert.That(resultados.Count, Is.EqualTo(1));
            Assert.That(resultados[0].Mail, Is.EqualTo("maria@mail.com"));
        }
        
       
        
     
        
        [Test]
        public void buscarSinCoincidencias()
        {
            // Act
            var resultados = repoCliente.BuscarPor("NoExiste");
            
            // Assert
            Assert.That(resultados, Is.Not.Null);
            Assert.That(resultados.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void buscarConTextoVacio()
        {
            // Act
            var resultados = repoCliente.BuscarPor("");
            
            // Assert
            Assert.That(resultados, Is.Not.Null);
            Assert.That(resultados.Count, Is.EqualTo(0));
        }
        

        

    }
}
