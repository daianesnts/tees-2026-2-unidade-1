using Application.Autor;
using AutoMapper;
using Domain.Autor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AutoresController : ControllerBase
{
    private readonly UseCaseCriarAutor _UseCaseCriarAutor;
    private readonly UseCaseEditarAutor _UseCaseEditarAutor;
    private readonly UseCaseExcluirAutor _UseCaseExcluirAutor;
    private readonly UseCaseObterAutorPorId _UseCaseObterAutorPorId;
    private readonly UseCaseListarAutores _UseCaseListarAutores;
    private readonly IMapper _mapper;

    public AutoresController(
        UseCaseCriarAutor UseCaseCriarAutor,
        UseCaseEditarAutor UseCaseEditarAutor,
        UseCaseExcluirAutor UseCaseExcluirAutor,
        UseCaseObterAutorPorId UseCaseObterAutorPorId,
        UseCaseListarAutores UseCaseListarAutores,
        IMapper mapper)
    {
        _UseCaseCriarAutor = UseCaseCriarAutor;
        _UseCaseEditarAutor = UseCaseEditarAutor;
        _UseCaseExcluirAutor = UseCaseExcluirAutor;
        _UseCaseObterAutorPorId = UseCaseObterAutorPorId;
        _UseCaseListarAutores = UseCaseListarAutores;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var listaAutores = _UseCaseListarAutores.Execute();

        return Ok(listaAutores);
    }

    [HttpGet("{id}")]
    public ActionResult Get(uint id)
    {
        var autor = _UseCaseObterAutorPorId.Execute(id);

        if (autor == null)
            return NotFound();

        return Ok(autor);
    }

    [HttpPost]
    public ActionResult Post([FromBody] AutorViewModel autorModel)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        var autor = _mapper.Map<AutorEntity>(autorModel);

        _UseCaseCriarAutor.Execute(autor);

        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Put(uint id, [FromBody] AutorViewModel autorModel)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        var autor = _mapper.Map<AutorEntity>(autorModel);

        autor.Id = id;

        var atualizado = _UseCaseEditarAutor.Execute(autor);

        if (!atualizado)
        {
            return NotFound("Autor não encontrado.");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(uint id)
    {
        var autor = _UseCaseObterAutorPorId.Execute(id);

        if (autor == null)
            return NotFound();

        _UseCaseExcluirAutor.Execute(id);

        return Ok();
    }
}