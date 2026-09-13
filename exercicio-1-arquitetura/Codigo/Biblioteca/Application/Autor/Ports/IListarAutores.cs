namespace Application.Autor;

public interface IListarAutores
{
    IEnumerable<AutorDTO> Execute();
}