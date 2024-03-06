using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using Quartz;
using APIProject.Service.Interfaces;
using APIProject.Service.Utils;
using Microsoft.Extensions.Configuration;
using APIProject.Common.Utils;

namespace APIProject.CronJob
{
    public class Jobclass : IJob
    {
        private readonly IConfiguration _Configuration;

        public Jobclass(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public async Task CheckTaskService()
        {


        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.WhenAll(CheckTaskService());
            }
            catch
            {
                return;
            }
        }
    }
}