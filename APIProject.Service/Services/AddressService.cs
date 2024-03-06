using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardRepository _wardRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public AddressService(IProvinceRepository provinceRepository, IDistrictRepository districtRepository, IWardRepository wardRepository, IMapper mapper, IHub hub)
        {
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _mapper = mapper;
            _sentryHub = hub;
        }

        public async Task<JsonResultModel> GetListProvice()
        {
            try
            {
                var data = await _provinceRepository.GetListProvice();
                return JsonResponse.Success(data);
            }
            catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListDistrict(int provinceID)
        {
            try
            {
                var data = await _districtRepository.GetListDistrict(provinceID);
                return JsonResponse.Success(data);
            }
            catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListWard(int districtID)
        {
            try
            {
                var data = await _wardRepository.GetListWard(districtID);
                return JsonResponse.Success(data);
            }
            catch(Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
