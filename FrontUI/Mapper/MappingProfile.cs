using AutoMapper;
using FrontUI.Models;


namespace FrontUI.Mapper
{
    public class MappingProfile
    {
        public static IMapper Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto, StudentServiceStatefull.Models.Student>();
                cfg.CreateMap<StudentServiceStatefull.Models.Student, RegisterDto>();
                cfg.CreateMap<StudentServiceStatefull.Models.Student, Student>();
                cfg.CreateMap<Student, StudentServiceStatefull.Models.Student>();
                cfg.CreateMap<LoginDto, StudentServiceStatefull.Models.Student>();
                cfg.CreateMap<StudentServiceStatefull.Models.Student, LoginDto>();
                cfg.CreateMap<Izvestaj, StudentServiceStatefull.Models.Izvestaj>();
                cfg.CreateMap<StudentServiceStatefull.Models.Izvestaj, Izvestaj>();
                cfg.CreateMap<StudentServiceStatefull.Models.OcenaNaPredmetu, OcenaNaPredmetu>();
                cfg.CreateMap<OcenaNaPredmetu, StudentServiceStatefull.Models.Izvestaj>();
                cfg.CreateMap<Predmet, PredmetService.Models.Predmet>();
                cfg.CreateMap<PredmetService.Models.Predmet, Predmet>();
                cfg.CreateMap<PredmetDto, PredmetService.Models.PredmetDto>();
                cfg.CreateMap<PredmetService.Models.PredmetDto, PredmetDto>();
                cfg.CreateMap<Predmet, StudentServiceStatefull.Models.Predmet>();
                cfg.CreateMap<StudentServiceStatefull.Models.Predmet, Predmet>();
                cfg.CreateMap<StudentServiceStatefull.Models.Profesor, Profesor>();
                cfg.CreateMap<Profesor, StudentServiceStatefull.Models.Profesor>();
                
                // Add more mappings as needed
            });

            return configuration.CreateMapper();
        }
    }
}
