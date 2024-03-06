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
    public class RelatedStallService: IRelatedStallService
    {
        private readonly IRelatedStallRepository _relatedStallReposirory;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public RelatedStallService(IRelatedStallRepository relatedStallReposirory, IMapper mapper, IHub sentryHub)
        {
            _relatedStallReposirory = relatedStallReposirory;
            _mapper = mapper;
            _sentryHub = sentryHub;
        }
    }
}
