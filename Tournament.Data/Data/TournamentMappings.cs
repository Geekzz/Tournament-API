using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings() 
        {
            // Mapping Tournament to TournamentDto with custom logic
            CreateMap<TournamentDetails, TournamentDto>()
                .ForMember(dest => dest.StartDate,
                           opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Titel, opt => opt.MapFrom(src => src.Title.Trim()));  // Custom formatting for start date

            // Mapping Game to GameDto, adding custom logic if needed
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.Titel, opt => opt.MapFrom(src => src.Title.Trim()));  // Trim whitespace if needed

            // Mapping for reverse (e.g., creating new tournament from DTO)
            CreateMap<TournamentDto, TournamentDetails>();
            CreateMap<GameDto, Game>();
        }
    }
}
