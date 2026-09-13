using Application.ItemAcervo;
using AutoMapper;
using BibliotecaWEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    //[Authorize]
    public class ItemAcervoController : Controller
    {
        private readonly UseCaseCriarItemAcervo _useCaseCriarItemAcervo;
        private readonly UseCaseEditarItemAcervo _useCaseEditarItemAcervo;
        private readonly UseCaseExcluirItemAcervo _useCaseExcluirItemAcervo;
        private readonly UseCaseObterItemAcervoPorId _useCaseObterItemAcervoPorId;
        private readonly UseCaseListarItemAcervo _useCaseListarItemAcervo;
        private readonly IMapper _mapper;

        public ItemAcervoController(
            UseCaseCriarItemAcervo useCaseCriarItemAcervo,
            UseCaseEditarItemAcervo useCaseEditarItemAcervo,
            UseCaseExcluirItemAcervo useCaseExcluirItemAcervo,
            UseCaseObterItemAcervoPorId useCaseObterItemAcervoPorId,
            UseCaseListarItemAcervo useCaseListarItemAcervo,
            IMapper mapper)
        {
            _useCaseCriarItemAcervo = useCaseCriarItemAcervo;
            _useCaseEditarItemAcervo = useCaseEditarItemAcervo;
            _useCaseExcluirItemAcervo = useCaseExcluirItemAcervo;
            _useCaseObterItemAcervoPorId = useCaseObterItemAcervoPorId;
            _useCaseListarItemAcervo = useCaseListarItemAcervo;
            _mapper = mapper;
        }


        // GET: ItemAcervoController
        public ActionResult Index()
        {
            var listaItemAcervo = _useCaseListarItemAcervo.Execute();
            var listaViewModel = _mapper.Map<List<ItemAcervoViewModel>>(listaItemAcervo);
            return View(listaViewModel);
        }

        // GET: ItemAcervoController/Details/5
        public ActionResult Details(int id)
        {
            var itemAcervo = _useCaseObterItemAcervoPorId.Execute((uint)id);
            if (itemAcervo == null)
            {
                return NotFound();
            }
            ItemAcervoViewModel itemAcervoViewModel = _mapper.Map<ItemAcervoViewModel>(itemAcervo);
            return View(itemAcervoViewModel);
        }

        // GET: ItemAcervoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemAcervoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemAcervoViewModel itemAcervoViewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<ItemAcervoDTO>(itemAcervoViewModel);
                _useCaseCriarItemAcervo.Execute(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(itemAcervoViewModel);
        }

        // GET: ItemAcervoController/Edit/5
        public ActionResult Edit(int id)
        {
            var itemAcervo = _useCaseObterItemAcervoPorId.Execute((uint)id);
            if (itemAcervo == null)
            {
                return NotFound();
            }
            ItemAcervoViewModel itemAcervoViewModel = _mapper.Map<ItemAcervoViewModel>(itemAcervo);
            return View(itemAcervoViewModel);
        }

        // POST: ItemAcervoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ItemAcervoViewModel itemAcervoViewModel)
        {
            if (ModelState.IsValid)
            {
                itemAcervoViewModel.Id = id;
                var dto = _mapper.Map<ItemAcervoDTO>(itemAcervoViewModel);
                _useCaseEditarItemAcervo.Execute(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(itemAcervoViewModel);
        }

        // GET: ItemAcervoController/Delete/5
        public ActionResult Delete(int id)
        {
            var itemAcervo = _useCaseObterItemAcervoPorId.Execute((uint)id);
            if (itemAcervo == null)
            {
                return NotFound();
            }
            ItemAcervoViewModel itemAcervoViewModel = _mapper.Map<ItemAcervoViewModel>(itemAcervo);
            return View(itemAcervoViewModel);
        }

        // POST: ItemAcervoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ItemAcervoViewModel itemAcervoViewModel)
        {
            _useCaseExcluirItemAcervo.Execute((uint)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
