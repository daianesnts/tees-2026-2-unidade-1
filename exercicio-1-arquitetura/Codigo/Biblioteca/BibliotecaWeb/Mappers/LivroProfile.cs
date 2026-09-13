using Application.Livro.DTOs;
using AutoMapper;
using Models;

namespace Mappers
{
    public class LivroProfile : Profile
    {
        public LivroProfile()
        {
            CreateMap<LivroViewModel, LivroDTO>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Nome ?? string.Empty))
                .ForMember(dest => dest.EditoraId, opt => opt.MapFrom(src => (uint?)src.IdEditora))
                .ReverseMap()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.IdEditora, opt => opt.MapFrom(src => (int)(src.EditoraId ?? 0)));

            CreateMap<LivroViewModel, CriarLivroDTO>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Nome ?? string.Empty))
                .ForMember(dest => dest.EditoraId, opt => opt.MapFrom(src => (uint?)src.IdEditora));

            CreateMap<LivroViewModel, EditarLivroDTO>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Nome ?? string.Empty))
                .ForMember(dest => dest.EditoraId, opt => opt.MapFrom(src => (uint?)src.IdEditora));
        }
    }
}
