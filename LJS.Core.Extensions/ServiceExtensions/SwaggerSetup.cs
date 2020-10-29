﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LJS.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using static LJS.Core.Extensions.CustomApiVersion;

namespace LJS.Core.Extensions
{
    /// <summary>
    /// Swagger 启动服务
    /// </summary>
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var basePath = AppContext.BaseDirectory;
            //var basePath2 = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            var ApiName = AppSettings.app(new string[] { "Startup", "ApiName" });

            services.AddSwaggerGen(c =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} 接口文档——{RuntimeInformation.FrameworkDescription}",
                        Description = $"{ApiName} HTTPS API " + version
                    });
                    c.OrderActionsBy(o => o.RelativePath);
                });

                try
                {
                    var xmlPath = Path.Combine(basePath, "LJS.Core.Api.xml");
                    c.IncludeXmlComments(xmlPath, true);
                    var xmlModelPath = Path.Combine(basePath, "LJS.Core.Model.xml");
                    c.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("Blog.Core.xml和Blog.Core.Model.xml 丢失，请检查并拷贝。\n" + ex.Message);
                }
            });
        }
    }

    /// <summary>
    /// 自定义版本
    /// </summary>
    public class CustomApiVersion
    {
        /// <summary>
        /// Api接口版本 自定义
        /// </summary>
        public enum ApiVersions
        {
            /// <summary>
            /// v1 版本
            /// </summary>
            v1 = 1,
            /// <summary>
            /// v2 版本
            /// </summary>
            v2 = 2,
            /// <summary>
            /// v3 版本
            /// </summary>
            v3 = 3,
        }
    }
}
