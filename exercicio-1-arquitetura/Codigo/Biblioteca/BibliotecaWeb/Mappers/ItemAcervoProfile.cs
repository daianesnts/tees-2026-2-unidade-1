using Application.ItemAcervo;
using AutoMapper;
using BibliotecaWEB.Models;

namespace BibliotecaWEB.Mappers
{
    public class ItemAcervoProfile : Profile
    {
        public ItemAcervoProfile()
        {
            CreateMap<ItemAcervoViewModel, ItemAcervoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (uint)src.Id))
                .ForMember(dest => dest.IdLivro, opt => opt.MapFrom(src => (uint)src.IdLivro))
                .ForMember(dest => dest.IdBiblioteca, opt => opt.MapFrom(src => (uint)src.IdBiblioteca))
                .ForMember(dest => dest.IdDoacao, opt => opt.MapFrom(src => (uint?)src.IdDoacao))
                .ForMember(dest => dest.IdSituacaoItemAcervo, opt => opt.MapFrom(src => src.IdSituacaoLivro ?? string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src.Id))
                .ForMember(dest => dest.IdLivro, opt => opt.MapFrom(src => (int)src.IdLivro))
                .ForMember(dest => dest.IdBiblioteca, opt => opt.MapFrom(src => (int)src.IdBiblioteca))
                .ForMember(dest => dest.IdDoacao, opt => opt.MapFrom(src => (int?)src.IdDoacao))
                .ForMember(dest => dest.IdSituacaoLivro, opt => opt.MapFrom(src => src.IdSituacaoItemAcervo));
        }
    }
}
