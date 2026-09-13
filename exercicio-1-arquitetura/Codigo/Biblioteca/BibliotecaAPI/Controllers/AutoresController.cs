using Application.Autor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AutoresController : ControllerBase
{
    private readonly UseCaseCriarAutor _useCaseCriarAutor;
    private readonly UseCaseEditarAutor _useCaseEditarAutor;
    private readonly UseCaseExcluirAutor _useCaseExcluirAutor;
    private readonly UseCaseObterAutorPorId _useCaseObterAutorPorId;
    private readonly UseCaseListarAutores _useCaseListarAutores;

    public AutoresController(
        UseCaseCriarAutor useCaseCriarAutor,
        UseCaseEditarAutor useCaseEditarAutor,
        UseCaseExcluirAutor useCaseExcluirAutor,
        UseCaseObterAutorPorId useCaseObterAutorPorId,
        UseCaseListarAutores useCaseListarAutores)
    {
        _useCaseCriarAutor = useCaseCriarAutor;
        _useCaseEditarAutor = useCaseEditarAutor;
        _useCaseExcluirAutor = useCaseExcluirAutor;
        _useCaseObterAutorPorId = useCaseObterAutorPorId;
        _useCaseListarAutores = useCaseListarAutores;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var listaAutores = _useCaseListarAutores.Execute();
        return Ok(listaAutores);
    }

    [HttpGet("{id}")]
    public ActionResult Get(uint id)
    {
        var autor = _useCaseObterAutorPorId.Execute(id);

        if (autor == null)
            return NotFound();

        return Ok(autor);
    }

    [HttpPost]
    public ActionResult Post([FromBody] AutorDTO dto)
    {
        var id = _useCaseCriarAutor.Execute(dto);

        return CreatedAtAction(nameof(Get), new { id }, dto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(uint id, [FromBody] AutorDTO dto)
    {
        dto.Id = id;

        var atualizado = _useCaseEditarAutor.Execute(dto);

        if (!atualizado)
        {
            return NotFound("Autor não encontrado.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(uint id)
    {
        var autor = _useCaseObterAutorPorId.Execute(id);

        if (autor == null)
            return NotFound("Autor não encontrado.");

        _useCaseExcluirAutor.Execute(id);

        return NoContent();
    }
}
