﻿using BilibiliAutoLiver.Services;
using BilibiliAutoLiver.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace BilibiliAutoLiver.DependencyInjection
{
    public static class RegisteBilibiliServices
    {
        public static IServiceCollection AddBilibiliServices(this IServiceCollection services)
        {
            //Cookie模块
            services.AddSingleton<IBilibiliCookieService, BilibiliCookieService>();
            //Http请求相关
            services.AddTransient<IHttpClientService, HttpClientService>();
            //账号
            services.AddTransient<IBilibiliAccountService, BilibiliAccountService>();
            //直播的API
            services.AddTransient<IBilibiliLiveApiService, BilibiliLiveApiService>();
            //推流相关
            services.AddTransient<IPushStreamServiceV1, PushStreamServiceV1>();

            return services;
        }
    }
}