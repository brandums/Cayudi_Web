using AutoMapper;
using EPlatformWebApp.Model;

namespace EPlatformWebApp.DTO.Utils
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<Video, VideoDTO>().ReverseMap();
            CreateMap<Video, VideoUpdateDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryDTO>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryCreationDTO>().ReverseMap();
            CreateMap<Course, CourseCreationDTO>().ReverseMap();
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<Blog, BlogCreationDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostCreationDTO>().ReverseMap();
            CreateMap<BlogPostAnswer, BlogPostAnswerDTO>().ReverseMap();
            CreateMap<BlogPostAnswer, BlogPostAnswerCreationDTO>().ReverseMap();
            CreateMap<PostLike, PostLikeDTO>().ReverseMap();
            CreateMap<PostLike, PostLikeCreationDTO>().ReverseMap();
            CreateMap<PostAnswerLikes, PostAnswerLikesDTO>().ReverseMap();
            CreateMap<PostAnswerLikes, PostAnswerLikesCreationDTO>().ReverseMap();
            CreateMap<UserFavoriteCourse, UserFavoriteCourseDTO>().ReverseMap();
            CreateMap<UserFavoriteCourse, UserFavoriteCourseCreationDTO>().ReverseMap();
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<UserCourse, UserCourseDTO>().ReverseMap();
            CreateMap<UserCourse, UserCourseCreationDTO>().ReverseMap();
            CreateMap<Video, VideoPlayerFormatDTO>()
                .ForMember(x => x.Name, v => v.MapFrom(u => u.Tittle))
                .ForMember(x => x.Src, v => v.MapFrom(u => u.Source))
                .ForMember(x => x.Type, v => v.MapFrom(u => u.Description)).ReverseMap();
            CreateMap<CourseTest, CourseTestDTO>().ReverseMap();
            CreateMap<CourseTest, CourseTestCreationDTO>().ReverseMap();
            CreateMap<PDFFile, PDFileDTO>().ReverseMap();
            CreateMap<UserImage, UserImagesDTO>().ReverseMap();
            CreateMap<Cert, CertDTO>().ReverseMap();
            CreateMap<Cert, CertCreationDTO>().ReverseMap();
        }
    }
}
