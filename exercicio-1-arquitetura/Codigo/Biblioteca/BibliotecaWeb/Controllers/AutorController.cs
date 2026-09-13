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
        private readonly ICriarAutor _criarAutor;
        private readonly IEditarAutor _editarAutor;
        private readonly IExcluirAutor _excluirAutor;
        private readonly IObterAutorPorId _obterAutorPorId;
        private readonly IListarAutores _listarAutores;
        private readonly IGetAutoresPage _getAutoresPage;
        private readonly IMapper _mapper;


        public AutorController(
            ICriarAutor criarAutor,
            IEditarAutor editarAutor,
            IExcluirAutor excluirAutor,
            IObterAutorPorId obterAutorPorId,
            IListarAutores listarAutores,
            IGetAutoresPage getAutoresPage,
            IMapper mapper)
        {
            _criarAutor = criarAutor;
            _editarAutor = editarAutor;
            _excluirAutor = excluirAutor;
            _obterAutorPorId = obterAutorPorId;
            _listarAutores = listarAutores;
            _getAutoresPage = getAutoresPage;

            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var listaAutores =
                _listarAutores.Execute();

            var listaAutorViewModel =
                _mapper.Map<List<AutorViewModel>>(
                    listaAutores
                );

            return View(listaAutorViewModel);
        }

        public ActionResult Details(uint id)
        {
            var autor =
                _obterAutorPorId.Execute(id);

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
                _mapper.Map<AutorDTO>(
                    autorViewModel
                );

            _criarAutor.Execute(autor);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(uint id)
        {
            var autor =
                _obterAutorPorId.Execute(id);

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
                _mapper.Map<AutorDTO>(
                    autorViewModel
                );

            autor.Id = id;

            var atualizado =
                _editarAutor.Execute(autor);

            if (!atualizado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(uint id)
        {
            var autor =
                _obterAutorPorId.Execute(id);

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
            _excluirAutor.Execute(
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

            var response = _getAutoresPage.Execute(pageRequest);

            return Json(response);
        }
    }
}