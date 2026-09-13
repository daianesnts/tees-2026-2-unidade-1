using AutoMapper;
using Core;
using Domain.Autor;
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