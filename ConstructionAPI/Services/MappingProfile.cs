using AutoMapper;
using ConstructionAPI.Models;

namespace ConstructionAPI.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Blog, BlogDTO>()
            //   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
            //   .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.AppUser.Email))
            //   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.Name))
            //   .ForMember(dest => dest.UserSurname, opt => opt.MapFrom(src => src.AppUser.Surname))
            //   .ForMember(dest => dest.UserImageUrl, opt => opt.MapFrom(src => src.AppUser.ImageUrl))
            //   .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            //   .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
            //   .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
            //   .ForMember(dest => dest.SaveCount, opt => opt.MapFrom(src => src.SavedBlogs.Count))
            //   .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("MMMM dd, yyyy")))
            //   .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("MMMM dd, yyyy")));

            //CreateMap<Image, ImageDTO>();

            //CreateMap<Review, ReviewDTO>()
            //   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AppUser.Id))
            //   .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.AppUser.Email))
            //   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.Name))
            //   .ForMember(dest => dest.UserSurname, opt => opt.MapFrom(src => src.AppUser.Surname))
            //   .ForMember(dest => dest.UserImageUrl, opt => opt.MapFrom(src => src.AppUser.ImageUrl))
            //   .ForMember(dest => dest.ParentUserId, opt => opt.MapFrom(src => src.ParentReview.AppUser.Id))
            //   .ForMember(dest => dest.ParentUserName, opt => opt.MapFrom(src => src.ParentReview.AppUser.Name))
            //   .ForMember(dest => dest.ParentUserSurname, opt => opt.MapFrom(src => src.ParentReview.AppUser.Surname))
            //   .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            //   .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("MMMM dd, yyyy")))
            //   .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentReviewId))
            //   .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Reviews));
        }
    }
}
