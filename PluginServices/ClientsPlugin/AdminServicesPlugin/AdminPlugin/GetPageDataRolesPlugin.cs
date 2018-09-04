﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminManager;
using Domain.BaseModels;
using Domain.DTOModels;
using Domain.QueryModels;
using Infrastructure.Utility;

namespace ClientsPlugin.AdminServicesPlugin
{
    public class GetPageDataRolesPlugin : BasePluginCore, IGetPageDataRolesPlugin
    {
        public TBaseResult<RolesQueryItem> GetPageDataRoles(RolesQueryItem queryItem)
        {
            this.SafetySecretKey = queryItem.SafetySecretKey;
            this.Usable = queryItem.IsUsable ? BasePluginType.Type.启用 : BasePluginType.Type.卸载;
            var result = new TBaseResult<RolesQueryItem>()
            {
                Code = (int)EnumCore.CodeType.失败,
                Message = "系统错误",
                TData = new RolesQueryItem()
            };
            if (string.IsNullOrWhiteSpace(this.SafetySecretKey) || this.Usable == BasePluginType.Type.卸载 || PluginCore.GetInstance.VerifySafetySecretKey(this.SafetySecretKey))
            {
                return result;
            }
            var dt = MenuManager.GetInstance.GetPageDataRoles(queryItem);
            result.Code = (int)EnumCore.CodeType.成功;
            if (dt.PageData.Count <= 0)
            {
                result.Message = "暂无数据";
                result.TData = dt;
                return result;
            }
            result.Message = "获取成功";
            result.TData = dt;
            return result;
        }

    }
}
