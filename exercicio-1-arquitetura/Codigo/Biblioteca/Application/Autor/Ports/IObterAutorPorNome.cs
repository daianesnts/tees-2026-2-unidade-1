namespace Application.Autor;

public interface IObterAutorPorNome
{
    IEnumerable<AutorDTO> Execute(string nome);
}