using AutoMapper;
using Core;
using Domain.Autor;
using Models;

namespace Mappers
{
    public class AutorProfile : Profile
    {
        public AutorProfile()
        {
            // Arquitetura antiga
            CreateMap<AutorViewModel, Autor>()
                .ReverseMap();

            // Clean Architecture
            CreateMap<AutorViewModel, AutorEntity>()
                .ReverseMap();
        }
    }
}