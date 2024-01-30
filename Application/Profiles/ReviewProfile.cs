using Application.Commands.ReviewCommands;
using Application.DTOs.ReviewsDtos;
using Application.Queries.ReviewQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>();
        
        // Commands And Queries
        CreateMap<ReviewForCreationDto, CreateReviewCommand>();
        CreateMap<CreateReviewCommand, Review>();
        CreateMap<ReviewQueryDto, GetReviewsQuery>();
    }
}