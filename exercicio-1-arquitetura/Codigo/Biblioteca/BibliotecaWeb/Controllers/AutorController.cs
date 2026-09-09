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
        private readonly CreateAutorUseCase _createAutorUseCase;
        private readonly UpdateAutorUseCase _updateAutorUseCase;
        private readonly DeleteAutorUseCase _deleteAutorUseCase;
        private readonly GetAutorByIdUseCase _getAutorByIdUseCase;
        private readonly GetAllAutoresUseCase _getAllAutoresUseCase;
        private readonly GetAutoresPageUseCase _getAutoresPageUseCase;
        private readonly IMapper _mapper;


        public AutorController(
            CreateAutorUseCase createAutorUseCase,
            UpdateAutorUseCase updateAutorUseCase,
            DeleteAutorUseCase deleteAutorUseCase,
            GetAutorByIdUseCase getAutorByIdUseCase,
            GetAllAutoresUseCase getAllAutoresUseCase,
            GetAutoresPageUseCase getAutoresPageUseCase,
            IMapper mapper)
        {
            _createAutorUseCase = createAutorUseCase;
            _updateAutorUseCase = updateAutorUseCase;
            _deleteAutorUseCase = deleteAutorUseCase;
            _getAutorByIdUseCase = getAutorByIdUseCase;
            _getAllAutoresUseCase = getAllAutoresUseCase;
            _getAutoresPageUseCase = getAutoresPageUseCase;

            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var listaAutores =
                _getAllAutoresUseCase.Execute();

            var listaAutorViewModel =
                _mapper.Map<List<AutorViewModel>>(
                    listaAutores
                );

            return View(listaAutorViewModel);
        }

        public ActionResult Details(uint id)
        {
            var autor =
                _getAutorByIdUseCase.Execute(id);

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

            _createAutorUseCase.Execute(autor);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(uint id)
        {
            var autor =
                _getAutorByIdUseCase.Execute(id);

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
                _updateAutorUseCase.Execute(autor);

            if (!atualizado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(uint id)
        {
            var autor =
                _getAutorByIdUseCase.Execute(id);

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
            _deleteAutorUseCase.Execute(
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