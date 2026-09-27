using AutoMapper;
using TrenRezervasyon.Api.DTOs.Requests;
using TrenRezervasyon.Api.Models;

namespace TrenRezervasyon.Api.Mappings
{
    public class RezervasyonMappingProfile : Profile
    {
        public RezervasyonMappingProfile()
        {
            CreateMap<VagonDto, Vagon>();
            CreateMap<TrenDto, Tren>();
        }
    }
}
