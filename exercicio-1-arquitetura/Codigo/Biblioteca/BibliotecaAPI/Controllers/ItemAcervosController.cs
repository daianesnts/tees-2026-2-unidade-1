using Application.ItemAcervo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ItemAcervosController : ControllerBase
{
    private readonly UseCaseCriarItemAcervo _useCaseCriarItemAcervo;
    private readonly UseCaseEditarItemAcervo _useCaseEditarItemAcervo;
    private readonly UseCaseExcluirItemAcervo _useCaseExcluirItemAcervo;
    private readonly UseCaseObterItemAcervoPorId _useCaseObterItemAcervoPorId;
    private readonly UseCaseListarItemAcervo _useCaseListarItemAcervo;

    public ItemAcervosController(
        UseCaseCriarItemAcervo useCaseCriarItemAcervo,
        UseCaseEditarItemAcervo useCaseEditarItemAcervo,
        UseCaseExcluirItemAcervo useCaseExcluirItemAcervo,
        UseCaseObterItemAcervoPorId useCaseObterItemAcervoPorId,
        UseCaseListarItemAcervo useCaseListarItemAcervo)
    {
        _useCaseCriarItemAcervo = useCaseCriarItemAcervo;
        _useCaseEditarItemAcervo = useCaseEditarItemAcervo;
        _useCaseExcluirItemAcervo = useCaseExcluirItemAcervo;
        _useCaseObterItemAcervoPorId = useCaseObterItemAcervoPorId;
        _useCaseListarItemAcervo = useCaseListarItemAcervo;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var listaItens = _useCaseListarItemAcervo.Execute();
        return Ok(listaItens);
    }

    [HttpGet("{id}")]
    public ActionResult Get(uint id)
    {
        var item = _useCaseObterItemAcervoPorId.Execute(id);

        if (item == null)
            return NotFound("Item do acervo não encontrado.");

        return Ok(item);
    }

    [HttpPost]
    public ActionResult Post([FromBody] ItemAcervoDTO dto)
    {
        var id = _useCaseCriarItemAcervo.Execute(dto);

        return CreatedAtAction(nameof(Get), new { id }, dto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(uint id, [FromBody] ItemAcervoDTO dto)
    {
        dto.Id = id;

        var atualizado = _useCaseEditarItemAcervo.Execute(dto);

        if (!atualizado)
        {
            return NotFound("Item do acervo não encontrado.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(uint id)
    {
        var itemAcervo = _useCaseObterItemAcervoPorId.Execute(id);

        if (itemAcervo == null)
            return NotFound("Item do acervo não encontrado.");

        _useCaseExcluirItemAcervo.Execute(id);
        return NoContent();
    }
}
