using AutoMapper;
using ExPaper.Organisation.API.Models;
using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Organisation.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrganisationModel, OrganisationDto>();
            config.CreateMap<OrganisationDto, OrganisationModel>();
        });


        return mappingConfig;
    }
}
