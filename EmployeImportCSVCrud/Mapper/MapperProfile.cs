using AutoMapper;
using EmployeeImportApp.Models;
using EmployeImportCSVCrud.Models;

namespace EmployeImportCSVCrud.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InsertEmployeeDto, Employee>();

            CreateMap<UpdateEmployeeDto, Employee>();

        }
    }
}
