using AutoMapper;
using Front.Models;

namespace Front.Mapper
{
    public class MappingProfile
    {
        public static IMapper Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto,StudentServiceStatefull.Models.Student>();
                cfg.CreateMap<StudentServiceStatefull.Models.Student, RegisterDto>();
                cfg.CreateMap<StudentServiceStatefull.Models.Student, Student>();
                cfg.CreateMap<Student, StudentServiceStatefull.Models.Student>();
                cfg.CreateMap<LoginDto, StudentServiceStatefull.Models.Student>();
                cfg.CreateMap<Izvestaj, StudentServiceStatefull.Models.Izvestaj>();
                cfg.CreateMap<StudentServiceStatefull.Models.Izvestaj, Izvestaj>();
                cfg.CreateMap<StudentServiceStatefull.Models.OcenaNaPredmetu, OcenaNaPredmetu>();
                cfg.CreateMap<OcenaNaPredmetu, StudentServiceStatefull.Models.Izvestaj>();


                // Add more mappings as needed
            });

            return configuration.CreateMapper();
        }
    }
}
