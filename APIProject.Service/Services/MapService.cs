using APIProject.Common.DTOs.Config;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class MapService : IMapService
    {
        private readonly IConfigRepository _ConfigRepository;
        private readonly IHub _sentryHub;

        public MapService(IConfigRepository configRepository, IHub sentryHub)
        {
            _ConfigRepository = configRepository;
            _sentryHub = sentryHub;
        }

        public async Task<JsonResultModel> GetListMap()
        {
            try
            {
                var map1 = await _ConfigRepository.GetFirstOrDefaultAsync(x => x.Key.Equals("MapFloor1"));
                var map2 = await _ConfigRepository.GetFirstOrDefaultAsync(x => x.Key.Equals("MapFloor2"));
                var map3 = await _ConfigRepository.GetFirstOrDefaultAsync(x => x.Key.Equals("MapFloor3"));
                var map = new MapModel
                {
                    MapFloor1 = map1.Value,
                    MapFloor2 = map2.Value,
                    MapFloor3 = map3.Value,
                };
                return JsonResponse.Success(map);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
