using Application.Autor;
using AutoMapper;
using Domain.Autor;
using Mappers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;

namespace BibliotecaWeb.Controllers.Tests
{
    [TestClass()]
    public class AutorControllerTests
    {
        private static AutorController? controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockRepository = new Mock<IAutorRepository>();

            mockRepository
                .Setup(repository => repository.GetAll())
                .Returns(GetTestAutores());

            mockRepository
                .Setup(repository => repository.GetById(1))
                .Returns(GetTargetAutor());

            mockRepository
                .Setup(repository => repository.GetById(2))
                .Returns(GetTargetAutor());

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AutorProfile()))
                .CreateMapper();

            var UseCaseCriarAutor =
                new UseCaseCriarAutor(mockRepository.Object);

            var UseCaseEditarAutor =
                new UseCaseEditarAutor(mockRepository.Object);

            var UseCaseExcluirAutor =
                new UseCaseExcluirAutor(mockRepository.Object);

            var UseCaseObterAutorPorId =
                new UseCaseObterAutorPorId(mockRepository.Object);

            var UseCaseListarAutores =
                new UseCaseListarAutores(mockRepository.Object);

            var getAutoresPageUseCase =
                new GetAutoresPageUseCase(mockRepository.Object);

            controller = new AutorController(
                UseCaseCriarAutor,
                UseCaseEditarAutor,
                UseCaseExcluirAutor,
                UseCaseObterAutorPorId,
                UseCaseListarAutores,
                getAutoresPageUseCase,
                mapper
            );
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller?.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<AutorViewModel>));

            List<AutorViewModel>? lista = (List<AutorViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller?.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AutorViewModel));
            AutorViewModel autorModel = (AutorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Machado de Assis", autorModel.Nome);
            Assert.AreEqual(DateTime.Parse("1839-06-21"), autorModel.DataNascimento);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            // Act
            var result = controller?.Create();
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Valid()
        {
            // Act
            var result = controller?.Create(GetNewAutor());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalid()
        {
            // Arrange
            controller?.ModelState.AddModelError(
                "Nome",
                "Nome é obrigatório."
            );

            // Act
            var result = controller?.Create(GetNewAutor());

            // Assert
            Assert.AreEqual(1, controller?.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void EditTest_Get_Valid()
        {
            // Act
            var result = controller?.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AutorViewModel));
            AutorViewModel autorModel = (AutorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Machado de Assis", autorModel.Nome);
            Assert.AreEqual(DateTime.Parse("1839-06-21"), autorModel.DataNascimento);
        }

        [TestMethod()]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller?.Edit(GetTargetAutorModel().Id, GetTargetAutorModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valid()
        {
            // Act
            var result = controller?.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AutorViewModel));
            AutorViewModel autorModel = (AutorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Machado de Assis", autorModel.Nome);
            Assert.AreEqual(DateTime.Parse("1839-06-21"), autorModel.DataNascimento);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valid()
        {
            // Act
            var result = controller?.Delete(GetTargetAutorModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private AutorViewModel GetNewAutor()
        {
            return new AutorViewModel
            {
                Id = 4,
                Nome = "Ian Sommerville",
                DataNascimento = DateTime.Parse("1951-02-23")
            };

        }
        private static AutorEntity GetTargetAutor()
        {
            return new AutorEntity
            {
                Id = 1,
                Nome = "Machado de Assis",
                DataNascimento = DateTime.Parse("1839-06-21")
            };
        }

        private AutorViewModel GetTargetAutorModel()
        {
            return new AutorViewModel
            {
                Id = 2,
                Nome = "Machado de Assis",
                DataNascimento = DateTime.Parse("1839-06-21")
            };
        }

        private IEnumerable<AutorEntity> GetTestAutores()
        {
            return new List<AutorEntity>
            {
                new AutorEntity
                {
                    Id = 1,
                    Nome = "Graciliano Ramos",
                    DataNascimento = DateTime.Parse("1892-10-27")
                },
                new AutorEntity
                {
                    Id = 2,
                    Nome = "Machado de Assis",
                    DataNascimento = DateTime.Parse("1839-06-21")
                },
                new AutorEntity
                {
                    Id = 3,
                    Nome = "Marcos Dósea",
                    DataNascimento = DateTime.Parse("1982-01-01")
                }
            };
        }
    }
}