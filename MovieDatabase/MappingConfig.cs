using AutoMapper;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;

namespace MovieDatabase
{
    public class MappingConfig : Profile
    {

        public MappingConfig() 
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<Genre, GenreCreateDto>().ReverseMap();
            CreateMap<Genre, GenreUpdateDto>().ReverseMap();

            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()))
                .ReverseMap();

            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore());

            CreateMap<Movie, MovieCreateDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));

            CreateMap<MovieUpdateDto, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore());

            CreateMap<People, PeopleDto>().ReverseMap();
            CreateMap<People, PeopleCreateDto>().ReverseMap();
            CreateMap<People, PeopleUpdateDto>().ReverseMap();
        }
    }
}
