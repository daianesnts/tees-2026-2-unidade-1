using Application.Editora;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EditorasController : ControllerBase
{
    private readonly UseCaseCriarEditora _useCaseCriarEditora;
    private readonly UseCaseEditarEditora _useCaseEditarEditora;
    private readonly UseCaseExcluirEditora _useCaseExcluirEditora;
    private readonly UseCaseListarEditoras _useCaseListarEditoras;
    private readonly UseCaseObterEditoraPorId _useCaseObterEditoraPorId;
    private readonly UseCaseBuscarEditoraPorNome _useCaseBuscarEditoraPorNome;

    public EditorasController(
        UseCaseCriarEditora useCaseCriarEditora,
        UseCaseEditarEditora useCaseEditarEditora,
        UseCaseExcluirEditora useCaseExcluirEditora,
        UseCaseListarEditoras useCaseListarEditoras,
        UseCaseObterEditoraPorId useCaseObterEditoraPorId,
        UseCaseBuscarEditoraPorNome useCaseBuscarEditoraPorNome)
    {
        _useCaseCriarEditora = useCaseCriarEditora;
        _useCaseEditarEditora = useCaseEditarEditora;
        _useCaseExcluirEditora = useCaseExcluirEditora;
        _useCaseListarEditoras = useCaseListarEditoras;
        _useCaseObterEditoraPorId = useCaseObterEditoraPorId;
        _useCaseBuscarEditoraPorNome = useCaseBuscarEditoraPorNome;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var editoras = _useCaseListarEditoras.Execute();
        return Ok(editoras);
    }

    [HttpGet("{id}")]
    public ActionResult Get(uint id)
    {
        var editora = _useCaseObterEditoraPorId.Execute(id);

        if (editora == null)
            return NotFound("Editora não encontrada.");

        return Ok(editora);
    }

    [HttpGet("buscar/{nome}")]
    public ActionResult BuscarPorNome(string nome)
    {
        var editoras = _useCaseBuscarEditoraPorNome.Execute(nome);
        return Ok(editoras);
    }

    [HttpPost]
    public ActionResult Post([FromBody] CriarEditoraDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = _useCaseCriarEditora.Execute(dto);
        return CreatedAtAction(nameof(Get), new { id }, dto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(uint id, [FromBody] AtualizarEditoraDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("O ID informado na rota não corresponde ao ID da editora.");

        var atualizado = _useCaseEditarEditora.Execute(dto);

        if (!atualizado)
            return NotFound("Editora não encontrada para atualização.");

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(uint id)
    {
        var editora = _useCaseObterEditoraPorId.Execute(id);

        if (editora == null)
            return NotFound("Editora não encontrada.");

        try
        {
            _useCaseExcluirEditora.Execute(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
