using Application.Editora;
using AutoMapper;
using Models;

namespace Mappers
{
    public class EditoraProfile : Profile
    {
        public EditoraProfile()
        {
            CreateMap<EditoraViewModel, EditoraDTO>().ReverseMap();
            CreateMap<EditoraViewModel, CriarEditoraDTO>();
            CreateMap<EditoraViewModel, AtualizarEditoraDTO>();
        }
    }
}
