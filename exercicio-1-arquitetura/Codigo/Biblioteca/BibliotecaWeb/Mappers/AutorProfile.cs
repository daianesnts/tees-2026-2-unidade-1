using Application.Autor;
using AutoMapper;
using Models;

namespace Mappers;

public class AutorProfile : Profile
{
    public AutorProfile()
    {
        CreateMap<AutorViewModel, AutorDTO>()
            .ReverseMap();
    }
}