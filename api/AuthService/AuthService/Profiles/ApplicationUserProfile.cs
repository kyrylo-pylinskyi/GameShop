using AuthService.Models.DTO.Request;
using AuthService.Models.Entities;
using AutoMapper;

namespace AuthService.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            // Source -> Target
            CreateMap<SignUpRequest, ApplicationUser>();
        }
    }
}
