using AutoMapper;
using CloudFront.Models;

namespace CloudFront.Mapper
{
    public class MappingProfile
    {
        public static IMapper Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto, StudentService.Models.Student>();
                cfg.CreateMap<StudentService.Models.Student, RegisterDto>();
                cfg.CreateMap<StudentService.Models.Student, Student>();
                cfg.CreateMap<Student, StudentService.Models.Student>();
                cfg.CreateMap<LoginDto, StudentService.Models.Student>();
               

                // Add more mappings as needed
            });

            return configuration.CreateMapper();
        }
    }
}
