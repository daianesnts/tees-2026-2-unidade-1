using Application.Autor;
using AutoMapper;
using Core.Datatables;
using Domain.Autor;
using Domain.Comum;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BibliotecaWeb.Controllers
{
    public class AutorController : Controller
    {
        private readonly UseCaseCriarAutor _UseCaseCriarAutor;
        private readonly UseCaseEditarAutor _UseCaseEditarAutor;
        private readonly UseCaseExcluirAutor _UseCaseExcluirAutor;
        private readonly UseCaseObterAutorPorId _UseCaseObterAutorPorId;
        private readonly UseCaseListarAutores _UseCaseListarAutores;
        private readonly GetAutoresPageUseCase _getAutoresPageUseCase;
        private readonly IMapper _mapper;


        public AutorController(
            UseCaseCriarAutor UseCaseCriarAutor,
            UseCaseEditarAutor UseCaseEditarAutor,
            UseCaseExcluirAutor UseCaseExcluirAutor,
            UseCaseObterAutorPorId UseCaseObterAutorPorId,
            UseCaseListarAutores UseCaseListarAutores,
            GetAutoresPageUseCase getAutoresPageUseCase,
            IMapper mapper)
        {
            _UseCaseCriarAutor = UseCaseCriarAutor;
            _UseCaseEditarAutor = UseCaseEditarAutor;
            _UseCaseExcluirAutor = UseCaseExcluirAutor;
            _UseCaseObterAutorPorId = UseCaseObterAutorPorId;
            _UseCaseListarAutores = UseCaseListarAutores;
            _getAutoresPageUseCase = getAutoresPageUseCase;

            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var listaAutores =
                _UseCaseListarAutores.Execute();

            var listaAutorViewModel =
                _mapper.Map<List<AutorViewModel>>(
                    listaAutores
                );

            return View(listaAutorViewModel);
        }

        public ActionResult Details(uint id)
        {
            var autor =
                _UseCaseObterAutorPorId.Execute(id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorViewModel =
                _mapper.Map<AutorViewModel>(autor);

            return View(autorViewModel);
        }


        public ActionResult Create()
        {
            var autorViewModel =
                new AutorViewModel
                {
                    DataNascimento = DateTime.Now
                };

            return View(autorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            AutorViewModel autorViewModel
        )
        {
            if (!ModelState.IsValid)
            {
                return View(autorViewModel);
            }

            var autor =
                _mapper.Map<AutorEntity>(
                    autorViewModel
                );

            _UseCaseCriarAutor.Execute(autor);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(uint id)
        {
            var autor =
                _UseCaseObterAutorPorId.Execute(id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorViewModel =
                _mapper.Map<AutorViewModel>(autor);

            return View(autorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            uint id,
            AutorViewModel autorViewModel
        )
        {
            if (!ModelState.IsValid)
            {
                return View(autorViewModel);
            }

            var autor =
                _mapper.Map<AutorEntity>(
                    autorViewModel
                );

            autor.Id = id;

            var atualizado =
                _UseCaseEditarAutor.Execute(autor);

            if (!atualizado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(uint id)
        {
            var autor =
                _UseCaseObterAutorPorId.Execute(id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorViewModel =
                _mapper.Map<AutorViewModel>(autor);

            return View(autorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(
            AutorViewModel autorViewModel
        )
        {
            _UseCaseExcluirAutor.Execute(
                autorViewModel.Id
            );

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult GetDataPage(DatatableRequest request)
        {
            var pageRequest = new PageRequest
            {
                Start = request.Start,
                Length = request.Length,
                Search = request.Search != null &&
                        request.Search.ContainsKey("value")
                    ? request.Search["value"]
                    : null
            };

            var response = _getAutoresPageUseCase.Execute(pageRequest);

            return Json(response);
        }
    }
}