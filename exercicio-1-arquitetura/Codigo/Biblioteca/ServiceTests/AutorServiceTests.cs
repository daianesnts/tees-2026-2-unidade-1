using Application.Autor;
using Domain.Autor;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class AutorServiceTests
    {
        private Context _context;
        private IAutorRepository _autorRepository;

        private UseCaseCriarAutor _useCaseCriarAutor;
        private UseCaseEditarAutor _useCaseEditarAutor;
        private UseCaseExcluirAutor _useCaseExcluirAutor;
        private UseCaseObterAutorPorId _useCaseObterAutorPorId;
        private UseCaseListarAutores _useCaseListarAutores;
        private UseCaseObterAutorPorNome _useCaseObterAutorPorNome;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<Context>();
            builder.UseInMemoryDatabase("BibliotecaCleanDatabase");
            var options = builder.Options;

            _context = new Context(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var autores = new List<AutorEntity>
            {
                new() { Id = 1, Nome = "Machado de Assis", DataNascimento = DateTime.Parse("1917-12-31") },
                new() { Id = 2, Nome = "Ian S. Sommervile", DataNascimento = DateTime.Parse("1935-12-31") },
                new() { Id = 3, Nome = "Gleford Myers", DataNascimento = DateTime.Parse("1900-11-20") }
            };

            _context.AddRange(autores);
            _context.SaveChanges();

            _autorRepository = new AutorRepository(_context);

            _useCaseCriarAutor = new UseCaseCriarAutor(_autorRepository);
            _useCaseEditarAutor = new UseCaseEditarAutor(_autorRepository);
            _useCaseExcluirAutor = new UseCaseExcluirAutor(_autorRepository);
            _useCaseObterAutorPorId = new UseCaseObterAutorPorId(_autorRepository);
            _useCaseListarAutores = new UseCaseListarAutores(_autorRepository);
            _useCaseObterAutorPorNome = new UseCaseObterAutorPorNome(_autorRepository);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            _useCaseCriarAutor.Execute(new AutorDTO
            {
                Id = 4,
                Nome = "Graciliano Ramos",
                DataNascimento = DateTime.Parse("1900-12-25")
            });

            // Assert
            Assert.AreEqual(4, _useCaseListarAutores.Execute().Count());
            var autor = _useCaseObterAutorPorId.Execute(4);
            Assert.IsNotNull(autor);
            Assert.AreEqual("Graciliano Ramos", autor.Nome);
            Assert.AreEqual(DateTime.Parse("1900-12-25"), autor.DataNascimento);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            _useCaseExcluirAutor.Execute(2);

            // Assert
            Assert.AreEqual(2, _useCaseListarAutores.Execute().Count());
            var autor = _useCaseObterAutorPorId.Execute(2);
            Assert.IsNull(autor);
        }

        [TestMethod()]
        public void EditTest()
        {
            // Act
            var autor = _useCaseObterAutorPorId.Execute(3);
            Assert.IsNotNull(autor);
            autor.Nome = "Paulo Coelho";
            autor.DataNascimento = DateTime.Parse("1950-11-21");
            _useCaseEditarAutor.Execute(autor);

            // Assert
            autor = _useCaseObterAutorPorId.Execute(3);
            Assert.IsNotNull(autor);
            Assert.AreEqual("Paulo Coelho", autor.Nome);
            Assert.AreEqual(DateTime.Parse("1950-11-21"), autor.DataNascimento);
        }

        [TestMethod()]
        public void GetTest()
        {
            var autor = _useCaseObterAutorPorId.Execute(1);
            Assert.IsNotNull(autor);
            Assert.AreEqual("Machado de Assis", autor.Nome);
            Assert.AreEqual(DateTime.Parse("1917-12-31"), autor.DataNascimento);
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            // Act
            var autores = _useCaseObterAutorPorNome.Execute("Machado");

            // Assert
            Assert.IsNotNull(autores);
            Assert.AreEqual(1, autores.Count());
            Assert.AreEqual("Machado de Assis", autores.First().Nome);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaAutor = _useCaseListarAutores.Execute();

            // Assert
            Assert.IsInstanceOfType(listaAutor, typeof(IEnumerable<AutorDTO>));
            Assert.IsNotNull(listaAutor);
            Assert.AreEqual(3, listaAutor.Count());
            Assert.AreEqual((uint)1, listaAutor.First().Id);
            Assert.AreEqual("Machado de Assis", listaAutor.First().Nome);
        }
    }
}