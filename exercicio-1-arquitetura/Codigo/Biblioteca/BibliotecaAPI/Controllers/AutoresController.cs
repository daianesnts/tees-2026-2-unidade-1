using Application.Autor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AutoresController : ControllerBase
{
    private readonly ICriarAutor _criarAutor;
    private readonly IEditarAutor _editarAutor;
    private readonly IExcluirAutor _excluirAutor;
    private readonly IObterAutorPorId _obterAutorPorId;
    private readonly IListarAutores _listarAutores;

    public AutoresController(
        ICriarAutor criarAutor,
        IEditarAutor editarAutor,
        IExcluirAutor excluirAutor,
        IObterAutorPorId obterAutorPorId,
        IListarAutores listarAutores)
    {
        _criarAutor = criarAutor;
        _editarAutor = editarAutor;
        _excluirAutor = excluirAutor;
        _obterAutorPorId = obterAutorPorId;
        _listarAutores = listarAutores;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var listaAutores = _listarAutores.Execute();
        return Ok(listaAutores);
    }

    [HttpGet("{id}")]
    public ActionResult Get(uint id)
    {
        var autor = _obterAutorPorId.Execute(id);

        if (autor == null)
            return NotFound();

        return Ok(autor);
    }

    [HttpPost]
    public ActionResult Post([FromBody] AutorDTO dto)
    {
        var id = _criarAutor.Execute(dto);

        return CreatedAtAction(nameof(Get), new { id }, dto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(uint id, [FromBody] AutorDTO dto)
    {
        dto.Id = id;

        var atualizado = _editarAutor.Execute(dto);

        if (!atualizado)
        {
            return NotFound("Autor não encontrado.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(uint id)
    {
        var autor = _obterAutorPorId.Execute(id);

        if (autor == null)
            return NotFound("Autor não encontrado.");

        _excluirAutor.Execute(id);

        return NoContent();
    }
}
