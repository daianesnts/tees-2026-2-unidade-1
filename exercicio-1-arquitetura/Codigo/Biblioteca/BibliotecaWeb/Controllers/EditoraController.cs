using Application.Editora;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BibliotecaWeb.Controllers
{
    public class EditoraController : Controller
    {
        private readonly UseCaseCriarEditora _useCaseCriarEditora;
        private readonly UseCaseEditarEditora _useCaseEditarEditora;
        private readonly UseCaseExcluirEditora _useCaseExcluirEditora;
        private readonly UseCaseListarEditoras _useCaseListarEditoras;
        private readonly UseCaseObterEditoraPorId _useCaseObterEditoraPorId;
        private readonly IMapper _mapper;

        public EditoraController(
            UseCaseCriarEditora useCaseCriarEditora,
            UseCaseEditarEditora useCaseEditarEditora,
            UseCaseExcluirEditora useCaseExcluirEditora,
            UseCaseListarEditoras useCaseListarEditoras,
            UseCaseObterEditoraPorId useCaseObterEditoraPorId,
            IMapper mapper)
        {
            _useCaseCriarEditora = useCaseCriarEditora;
            _useCaseEditarEditora = useCaseEditarEditora;
            _useCaseExcluirEditora = useCaseExcluirEditora;
            _useCaseListarEditoras = useCaseListarEditoras;
            _useCaseObterEditoraPorId = useCaseObterEditoraPorId;
            _mapper = mapper;
        }

        // GET: EditoraController
        public ActionResult Index()
        {
            var listaEditoras = _useCaseListarEditoras.Execute();
            var listaEditorasModel = _mapper.Map<List<EditoraViewModel>>(listaEditoras);
            return View(listaEditorasModel);
        }

        // GET: EditoraController/Details/5
        public ActionResult Details(int id)
        {
            var editora = _useCaseObterEditoraPorId.Execute((uint)id);
            if (editora == null)
            {
                return NotFound();
            }
            var editoraViewModel = _mapper.Map<EditoraViewModel>(editora);
            return View(editoraViewModel);
        }

        // GET: EditoraController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EditoraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditoraViewModel editoraViewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<CriarEditoraDTO>(editoraViewModel);
                _useCaseCriarEditora.Execute(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(editoraViewModel);
        }

        // GET: EditoraController/Edit/5
        public ActionResult Edit(int id)
        {
            var editora = _useCaseObterEditoraPorId.Execute((uint)id);
            if (editora == null)
            {
                return NotFound();
            }
            var editoraViewModel = _mapper.Map<EditoraViewModel>(editora);
            return View(editoraViewModel);
        }

        // POST: EditoraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditoraViewModel editoraModel)
        {
            if (ModelState.IsValid)
            {
                editoraModel.Id = id;
                var dto = _mapper.Map<AtualizarEditoraDTO>(editoraModel);
                _useCaseEditarEditora.Execute(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(editoraModel);
        }

        // GET: EditoraController/Delete/5
        public ActionResult Delete(int id)
        {
            var editora = _useCaseObterEditoraPorId.Execute((uint)id);
            if (editora == null)
            {
                return NotFound();
            }
            var editoraViewModel = _mapper.Map<EditoraViewModel>(editora);
            return View(editoraViewModel);
        }

        // POST: EditoraController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EditoraViewModel editoraModel)
        {
            _useCaseExcluirEditora.Execute((uint)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
