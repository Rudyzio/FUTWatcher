using AutoMapper;
using FUTWatcher.API.Models;
using FUTWatcher.API.ViewModels;

namespace FUTWatcher.API.Mapping {
    public class MappingProfile : Profile {
        public MappingProfile() {
            // ViewModel to Model
            CreateMap<UserViewModel, User>();

            // Model to ViewModel
        }
    }
}