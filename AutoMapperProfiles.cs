using AutoMapper;

namespace FileSharingApp
{
    public class UploadProfile : Profile
    {
        public UploadProfile()
        {
            CreateMap<Models.InputUpload, Data.Uploads>()
                .ForMember(u => u.Id, op => op.Ignore())
                .ForMember(u => u.UploadDate, op => op.Ignore());

            CreateMap<Data.Uploads, Models.UploadViewModel>();
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Data.ApplicationUser, Models.UserViewModel>();
        }
    }
}
