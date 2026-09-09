using AutoMapper;
using Core;
using Domain.Autor;
using Models;

namespace Mappers;

public class AutorProfile : Profile
{
    public AutorProfile()
    {
        // Sistema antigo
        CreateMap<AutorViewModel, Autor>()
            .ReverseMap();

        // Clean Architecture
        CreateMap<AutorViewModel, AutorEntity>()
            .ReverseMap();
    }
}