using Application.Livro;
using Application.Livro.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly UseCaseListarLivros _useCaseListarLivros;
        private readonly UseCaseObterLivroPorId _useCaseObterLivroPorId;
        private readonly UseCaseCriarLivro _useCaseCriarLivro;
        private readonly UseCaseEditarLivro _useCaseEditarLivro;
        private readonly UseCaseExcluirLivro _useCaseExcluirLivro;
        private readonly IMapper _mapper;

        public LivrosController(UseCaseListarLivros useCaseListarLivros, UseCaseObterLivroPorId useCaseObterLivroPorId,UseCaseCriarLivro useCaseCriarLivro,UseCaseEditarLivro useCaseEditarLivro,UseCaseExcluirLivro useCaseExcluirLivro,IMapper mapper)
        {
            _useCaseListarLivros = useCaseListarLivros;
            _useCaseObterLivroPorId = useCaseObterLivroPorId;
            _useCaseCriarLivro = useCaseCriarLivro;
            _useCaseEditarLivro = useCaseEditarLivro;
            _useCaseExcluirLivro = useCaseExcluirLivro;
            _mapper = mapper;
        }

        // GET: api/Livros
        [HttpGet]
        public ActionResult Get()
        {
            var listaLivros = _useCaseListarLivros.Execute();
            return Ok(listaLivros);
        }

        // GET api/Livros/5
        [HttpGet("{id}")]
        public ActionResult Get(uint id)
        {
            var livro = _useCaseObterLivroPorId.Execute(id);
            if (livro == null)
                return NotFound("Livro não encontrado");

            return Ok(livro);
        }

        // POST api/Livros
        [HttpPost]
        public ActionResult Post([FromBody] CriarLivroDTO dto)
        {
            _useCaseCriarLivro.Execute(dto);
            return Ok();
        }

        // PUT api/Livros/5
        [HttpPut("{id}")]
        public ActionResult Put(uint id, [FromBody] EditarLivroDTO dto)
        {
            _useCaseEditarLivro.Execute(dto);
            return NoContent(); 
        }

        // DELETE api/Livros/5
        [HttpDelete("{id}")]
        public ActionResult Delete(uint id)
        {
            _useCaseExcluirLivro.Execute(id);
            return NoContent(); 
        }
    }
}