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
    private readonly CreateAutorUseCase _createAutorUseCase;
    private readonly UpdateAutorUseCase _updateAutorUseCase;
    private readonly DeleteAutorUseCase _deleteAutorUseCase;
    private readonly GetAutorByIdUseCase _getAutorByIdUseCase;
    private readonly GetAllAutoresUseCase _getAllAutoresUseCase;
    private readonly IMapper _mapper;

    public AutoresController(
        CreateAutorUseCase createAutorUseCase,
        UpdateAutorUseCase updateAutorUseCase,
        DeleteAutorUseCase deleteAutorUseCase,
        GetAutorByIdUseCase getAutorByIdUseCase,
        GetAllAutoresUseCase getAllAutoresUseCase,
        IMapper mapper)
    {
        _createAutorUseCase = createAutorUseCase;
        _updateAutorUseCase = updateAutorUseCase;
        _deleteAutorUseCase = deleteAutorUseCase;
        _getAutorByIdUseCase = getAutorByIdUseCase;
        _getAllAutoresUseCase = getAllAutoresUseCase;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var listaAutores = _getAllAutoresUseCase.Execute();

        return Ok(listaAutores);
    }

    [HttpGet("{id}")]
    public ActionResult Get(uint id)
    {
        var autor = _getAutorByIdUseCase.Execute(id);

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

        _createAutorUseCase.Execute(autor);

        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Put(uint id, [FromBody] AutorViewModel autorModel)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        var autor = _mapper.Map<AutorEntity>(autorModel);

        autor.Id = id;

        var atualizado = _updateAutorUseCase.Execute(autor);

        if (!atualizado)
        {
            return NotFound("Autor não encontrado.");
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(uint id)
    {
        var autor = _getAutorByIdUseCase.Execute(id);

        if (autor == null)
            return NotFound();

        _deleteAutorUseCase.Execute(id);

        return Ok();
    }
}