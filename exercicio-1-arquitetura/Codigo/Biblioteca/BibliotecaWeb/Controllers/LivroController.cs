using Application.Autor;
using Application.Editora;
using Application.Livro;
using Application.Livro.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace BibliotecaWeb.Controllers
{
    public class LivroController : Controller
    {
        private readonly UseCaseCriarLivro _useCaseCriarLivro;
        private readonly UseCaseEditarLivro _useCaseEditarLivro;
        private readonly UseCaseExcluirLivro _useCaseExcluirLivro;
        private readonly UseCaseObterLivroPorId _useCaseObterLivroPorId;
        private readonly UseCaseListarLivros _useCaseListarLivros;
        private readonly UseCaseListarAutores _useCaseListarAutores;
        private readonly UseCaseListarEditoras _useCaseListarEditoras;
        private readonly IMapper _mapper;

        public LivroController(
            UseCaseCriarLivro useCaseCriarLivro,
            UseCaseEditarLivro useCaseEditarLivro,
            UseCaseExcluirLivro useCaseExcluirLivro,
            UseCaseObterLivroPorId useCaseObterLivroPorId,
            UseCaseListarLivros useCaseListarLivros,
            UseCaseListarAutores useCaseListarAutores,
            UseCaseListarEditoras useCaseListarEditoras,
            IMapper mapper)
        {
            _useCaseCriarLivro = useCaseCriarLivro;
            _useCaseEditarLivro = useCaseEditarLivro;
            _useCaseExcluirLivro = useCaseExcluirLivro;
            _useCaseObterLivroPorId = useCaseObterLivroPorId;
            _useCaseListarLivros = useCaseListarLivros;
            _useCaseListarAutores = useCaseListarAutores;
            _useCaseListarEditoras = useCaseListarEditoras;
            _mapper = mapper;
        }

        // GET: LivroController
        public ActionResult Index()
        {
            var listaLivros = _useCaseListarLivros.Execute();
            var listaLivrosModel = _mapper.Map<List<LivroViewModel>>(listaLivros);
            return View(listaLivrosModel);
        }

        // GET: LivroController/Details/5
        public ActionResult Details(uint id)
        {
            var livro = _useCaseObterLivroPorId.Execute(id);
            if (livro == null)
            {
                return NotFound();
            }
            LivroViewModel livroViewModel = _mapper.Map<LivroViewModel>(livro);
            return View(livroViewModel);
        }

        // GET: LivroController/Create
        public ActionResult Create()
        {
            LivroViewModel livroModel = new();

            var listaAutores = _useCaseListarAutores.Execute();
            var listaEditoras = _useCaseListarEditoras.Execute();

            livroModel.ListaEditoras = new SelectList(listaEditoras, "Id", "Nome", null);
            livroModel.ListaAutores = new SelectList(listaAutores, "Id", "Nome", null);
            return View(livroModel);
        }

        // POST: LivroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LivroViewModel livroViewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<CriarLivroDTO>(livroViewModel);
                _useCaseCriarLivro.Execute(dto);
                return RedirectToAction(nameof(Index));
            }

            var listaAutores = _useCaseListarAutores.Execute();
            var listaEditoras = _useCaseListarEditoras.Execute();
            livroViewModel.ListaEditoras = new SelectList(listaEditoras, "Id", "Nome", null);
            livroViewModel.ListaAutores = new SelectList(listaAutores, "Id", "Nome", null);
            return View(livroViewModel);
        }

        // GET: LivroController/Edit/5
        public ActionResult Edit(uint id)
        {
            var livro = _useCaseObterLivroPorId.Execute(id);
            if (livro == null)
            {
                return NotFound();
            }

            LivroViewModel livroModel = _mapper.Map<LivroViewModel>(livro);

            var listaAutores = _useCaseListarAutores.Execute();
            var listaEditoras = _useCaseListarEditoras.Execute();

            livroModel.ListaEditoras = new SelectList(listaEditoras, "Id", "Nome",
                        listaEditoras.FirstOrDefault(e => e.Id == livro.EditoraId));
            livroModel.ListaAutores = new SelectList(listaAutores, "Id", "Nome", null);

            return View(livroModel);
        }

        // POST: LivroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LivroViewModel livroViewModel)
        {
            if (ModelState.IsValid)
            {
                livroViewModel.Id = id;
                var dto = _mapper.Map<EditarLivroDTO>(livroViewModel);
                _useCaseEditarLivro.Execute(dto);
                return RedirectToAction(nameof(Index));
            }

            var listaAutores = _useCaseListarAutores.Execute();
            var listaEditoras = _useCaseListarEditoras.Execute();
            livroViewModel.ListaEditoras = new SelectList(listaEditoras, "Id", "Nome", null);
            livroViewModel.ListaAutores = new SelectList(listaAutores, "Id", "Nome", null);
            return View(livroViewModel);
        }

        // GET: LivroController/Delete/5
        public ActionResult Delete(uint id)
        {
            var livro = _useCaseObterLivroPorId.Execute(id);
            if (livro == null)
            {
                return NotFound();
            }
            LivroViewModel livroViewModel = _mapper.Map<LivroViewModel>(livro);
            return View(livroViewModel);
        }

        // POST: LivroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, LivroViewModel livroViewModel)
        {
            _useCaseExcluirLivro.Execute(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
