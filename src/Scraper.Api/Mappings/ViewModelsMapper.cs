using AutoMapper;
using Scraper.Domain.Models;

namespace Scraper.Api.Mappings
{
    public class ViewModelsMapper : Profile
    {
        public ViewModelsMapper()
        {
            CreateMap<Show, ViewModels.Show>()
                .ForMember(s => s.Id, o => o.MapFrom( s => s.Id))
                .ForMember( s => s.Name, o => o.MapFrom( s => s.Name))
                .ForMember( s => s.Cast, o => o.MapFrom(s => s.Cast));

            CreateMap<Person, ViewModels.Person>()
                .ForMember( s => s.Id, o => o.MapFrom( s => s.Id))
                .ForMember( s => s.Name, o => o.MapFrom( s => s.Name))
                .ForMember( s => s.Birthday, o => o.MapFrom( s => s.Birthday != null ? s.Birthday.Value.ToString("yyyy-MM-dd") : null));
        }
    }
}