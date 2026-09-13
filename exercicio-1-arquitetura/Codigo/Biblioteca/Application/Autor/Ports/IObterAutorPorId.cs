namespace Application.Autor;

public interface IObterAutorPorId
{
    AutorDTO? Execute(uint id);
}