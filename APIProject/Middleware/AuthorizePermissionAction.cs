using APIProject.Common.Utils;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Middleware
{
    public class AuthorizePermissionAction : IAuthorizationFilter
    {
        private readonly int _permission;

        public AuthorizePermissionAction(int permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authCode = (int?)context.HttpContext.Items["AuthCode"];
            if (authCode == SystemParam.TOKEN_INVALID)
            {
                context.Result = new JsonResult(JsonResponse.Error(SystemParam.TOKEN_INVALID, SystemParam.MESSAGE_TOKEN_INVALID))
                { };
            }else if (authCode == SystemParam.TOKEN_ERROR)
            {
                context.Result = new JsonResult(JsonResponse.Error(SystemParam.TOKEN_ERROR, SystemParam.MESSAGE_TOKEN_ERROR))
                { };
            }
            else
            {
                try
                {
                }
                catch (Exception ex)
                {
                    context.Result = new JsonResult(JsonResponse.ServerError())
                    { };
                }
            }
        }
    }
}
