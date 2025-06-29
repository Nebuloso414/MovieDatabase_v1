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

            // Movie to MovieDto mapping with cast information
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    src.Genres.Select(g => g.Name).ToList()))
                .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => 
                    src.Cast.Select(c => new MovieCastDto
                    {
                        Person = c.Person.Name,
                        Role = c.Role.Name
                    }).ToList()));

            CreateMap<MovieDto, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Cast, opt => opt.Ignore());

            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Cast, opt => opt.Ignore());

            CreateMap<Movie, MovieCreateDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => 
                    src.Genres.Select(g => g.Name).ToList()));

            CreateMap<MovieUpdateDto, Movie>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Cast, opt => opt.Ignore());

            // MovieCast to MovieCastDto
            CreateMap<MovieCast, MovieCastDto>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<People, PeopleDto>().ReverseMap();
            CreateMap<People, PeopleCreateDto>().ReverseMap();
            CreateMap<People, PeopleUpdateDto>().ReverseMap();
        }
    }
}
