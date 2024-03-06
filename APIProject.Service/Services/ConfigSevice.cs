using APIProject.Common.DTOs.Config;
using APIProject.Common.DTOs.Customer;
using APIProject.Common.Utils;
using APIProject.Domain.Migrations;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class ConfigSevice : IConfigService
    {
        private readonly IConfigRepository _ConfigRepository;
        private readonly IMapper _mapper;
        public ConfigSevice(IConfigRepository ConfigRepository, IMapper mapper)
        {
            _ConfigRepository = ConfigRepository;
            _mapper = mapper;
        }
        public async Task<JsonResultModel> GetContact()
        {
            try
            {
                var hl = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_HOTLINE && c.IsActive.Equals(SystemParam.ACTIVE));
                var fb = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_FACEBOOK && c.IsActive.Equals(SystemParam.ACTIVE));
                var ws = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_WEBSITE && c.IsActive.Equals(SystemParam.ACTIVE));
                ConfigContactModel model = new ConfigContactModel
                {
                    LinkHotline = hl.Value,
                    LinkHotFacebook = fb.Value,
                    LinkWebsite = ws.Value,
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> GetContactInfo()
        {
            try
            {

                var fb = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Key.Contains("LinkFacebook"));
                var ws = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Key.Contains("LinkWebsite"));
                var hl = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Key.Contains("LinkHotline"));
                var Model = new ContactInfoModel
                {
                    Facebook = fb.Value,
                    Website = ws.Value,
                    Hotline = hl.Value,
                };

                return JsonResponse.Success(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> GetEventInfo()
        {
            try
            {
                var point = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.POINT_ADD && c.IsActive.Equals(SystemParam.ACTIVE));
                var od = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.ORDER_VALUE && c.IsActive.Equals(SystemParam.ACTIVE));

                ConfigEventInfoModel model = new ConfigEventInfoModel
                {
                    PointAdd = point.ValueLong,
                    OrderValue = od.ValueLong,
                };
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> GetLinkSurvery()
        {
            try
            {
                var Model = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_SURVERY && c.IsActive.Equals(SystemParam.ACTIVE));
                ConfigSurveyModel survery = new ConfigSurveyModel
                {
                    linkSurvery = Model.Value,
                };
                return JsonResponse.Success(survery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<JsonResultModel> UpdateContact(string linkHotLine, string linkWebsite, string linkFacebook)
        {
            try
            {

                var hl = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_HOTLINE && c.IsActive.Equals(SystemParam.ACTIVE));
                var fb = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_FACEBOOK && c.IsActive.Equals(SystemParam.ACTIVE));
                var ws = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_WEBSITE && c.IsActive.Equals(SystemParam.ACTIVE));
                hl.Value = linkHotLine;
                fb.Value = linkFacebook;
                ws.Value = linkWebsite;
                await _ConfigRepository.UpdateAsync(hl);
                await _ConfigRepository.UpdateAsync(fb);
                await _ConfigRepository.UpdateAsync(ws);
                return JsonResponse.Success();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<JsonResultModel> UpdateEventInfo(long pointAdd, long orderValue)
        {
            try
            {

                var point = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.POINT_ADD && c.IsActive.Equals(SystemParam.ACTIVE));
                var od = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.ORDER_VALUE && c.IsActive.Equals(SystemParam.ACTIVE));
                point.ValueLong = pointAdd;
                od.ValueLong = orderValue;
                await _ConfigRepository.UpdateAsync(point);
                await _ConfigRepository.UpdateAsync(od);
                return JsonResponse.Success();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<JsonResultModel> UpdateLinkSurvery(string linkSurvery)
        {
            try
            {
                var Model = await _ConfigRepository.GetFirstOrDefaultAsync(c => c.Key == SystemParam.LINK_SURVERY && c.IsActive.Equals(SystemParam.ACTIVE));
                Model.Value = linkSurvery;
                await _ConfigRepository.UpdateAsync(Model);
                return JsonResponse.Success(Model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
