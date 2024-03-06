using AutoMapper;
using APIProject.Service.Models;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using APIProject.Service.Utils;
using APIProject.Common.Models.Users;
using APIProject.Service.Models.Authentication;
using APIProject.Common.DTOs.Authentication;
using APIProject.Common.DTOs.Stall;
using APIProject.Common.DTOs.Category;
using APIProject.Common.DTOs.News;
using APIProject.Common.DTOs.Gift;
using APIProject.Common.DTOs.Staff;
using APIProject.Common.DTOs.Customer;
using APIProject.Common.DTOs.Event;
using APIProject.Common.DTOs.EventParticipant;
using APIProject.Common.DTOs.Survey;

namespace APIProject.Service
{
    // Cung cấp lớp map Ping to Profile : 
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MappingEntityToViewModel();
            MappingViewModelToEntity();
        }

        private void MappingEntityToViewModel()
        {
            // case get data
            //        CreateMap<Province, ProvinceModel>()
            //.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Code));
            //        CreateMap<District, DistrictModel>()
            //            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Code))
            //            .ForMember(dest => dest.ProvinceID, opt => opt.MapFrom(src => src.ProvinceCode));
            //        CreateMap<Ward, WardModel>()
            //            .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.District_id));
            //        CreateMap<News, BannerModel>();
            CreateMap<Customer, RegisterModel>();
            CreateMap<User, UserModel>();
            CreateMap<User, UserInfoModel>();
            CreateMap<News, CreateNewsModel>();
            CreateMap<News, CreateEventModel>();
            CreateMap<Gift, CreateGiftModel>();
            CreateMap<EventParticipant, AccumulatePointModel>();
            CreateMap<SurveyQuestion, CreateSurveryModel>();
        }

        private void MappingViewModelToEntity()
        {
            // case insert or update
            CreateMap<UserModel, User>();
            CreateMap<RegisterPhone, Customer>();
            CreateMap<CreateStallModel, Stall>();
            CreateMap<StallDetailModel, Stall>();
            CreateMap<CategoryModel, Category>();
            CreateMap<CreateGiftModel, Gift>();
            CreateMap<CreateNewsModel, News>();
            CreateMap<UpdateNewsModel, News>();
            CreateMap<CreateEventModel, News>();
            CreateMap<UpdateEventModel, News>();
            CreateMap<UpdateGiftModel, Gift>();
            CreateMap<AddStaffModel, Customer>();
            CreateMap<UpdateStaffModel, Customer>();
            CreateMap<AccumulatePointModel, EventParticipant>();
            CreateMap<CreateQRCodeModel, QRCode>()
            .ForMember(dest => dest.NewsID, opt => opt.MapFrom(src => src.EventID));
            CreateMap<CreateQRCodeModel, Customer>()
            .ForMember(dest => dest.AgeType, opt => opt.MapFrom(src => src.Age));
            CreateMap<QRCodeBillModel, QRCodeBill>();
            CreateMap<QRCodeBillModel, Bill>();
            CreateMap<EventParticipantModel, EventParticipant>()
             .ForMember(dest => dest.NewsID, opt => opt.MapFrom(src => src.EventID));
            CreateMap<CustomerInfo, Customer>();
        }
    }
}
