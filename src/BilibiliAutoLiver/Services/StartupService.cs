﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Bilibili.AspNetCore.Apis.Interface;
using Bilibili.AspNetCore.Apis.Models;
using BilibiliAutoLiver.Config;
using BilibiliAutoLiver.Jobs.Scheduler;
using BilibiliAutoLiver.Services.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BilibiliAutoLiver.Services
{
    class StartupService : IStartupService
    {
        private readonly ILogger<StartupService> _logger;
        private readonly IBilibiliAccountApiService _accountService;
        private readonly IJobSchedulerService _jobScheduler;
        private readonly IPushStreamProxyService _pushProxyService;
        private readonly IMemoryCache _cache;
        private readonly ILocalLockService _lockService;
        private readonly IFFMpegService _ffmpeg;

        public StartupService(ILogger<StartupService> logger
            , IBilibiliAccountApiService accountService
            , IJobSchedulerService jobScheduler
            , IPushStreamProxyService pushProxyService
            , IMemoryCache cache
            , ILocalLockService lockService
            , IFFMpegService ffmpeg)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _jobScheduler = jobScheduler ?? throw new ArgumentNullException(nameof(jobScheduler));
            _pushProxyService = pushProxyService ?? throw new ArgumentNullException(nameof(pushProxyService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _lockService = lockService ?? throw new ArgumentNullException(nameof(lockService));
            _ffmpeg = ffmpeg ?? throw new ArgumentNullException(nameof(ffmpeg));
        }

        public async Task Start(CancellationToken token)
        {
            try
            {
                UserInfo userInfo = await Login();
                if (userInfo == null)
                {
                    _logger.LogWarning("用户未登录！");
                    return;
                }
                Preheat();
                //登录成功之后，启动定时任务
                await _jobScheduler.Start(token);
                //开始推流
                await _pushProxyService.Start(true);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogWarning($"自动开启推流失败！{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"自动开启推流失败！{ex.Message}");
            }
        }

        private async Task<UserInfo> Login()
        {
            try
            {
                UserInfo userInfo = null;
                //通过保存的Cookie登录
                try
                {
                    _lockService.Lock(CacheKeyConstant.LOGING_STATUS_CACHE_KEY, TimeSpan.FromSeconds(300));
                    userInfo = await _accountService.LoginByCookie();
                }
                finally
                {
                    _lockService.UnLock(CacheKeyConstant.LOGING_STATUS_CACHE_KEY);
                }

                if (userInfo == null)
                {
                    if (_lockService.Lock(CacheKeyConstant.QRCODE_LOGIN_STATUS_CACHE_KEY, TimeSpan.FromSeconds(300)))
                    {
                        try
                        {
                            //通过扫描二维码登录
                            userInfo = await _accountService.LoginByQrCode();
                        }
                        finally
                        {
                            _lockService.UnLock(CacheKeyConstant.QRCODE_LOGIN_STATUS_CACHE_KEY);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("正在扫描二维码登录中...");
                    }
                }
                if (userInfo == null)
                {
                    _logger.LogWarning("用户未登录，启动进程将挂起。再用户登录成功之后，将继续执行");
                    //通过Cookie和二维码登录都未成功，那么挂起，直到完成用户登录
                    while (!await _accountService.IsLogged())
                    {
                        await Task.Delay(1000);
                    }
                    userInfo = await _accountService.LoginByCookie();
                }
                if (userInfo != null)
                {
                    _logger.LogInformation($"用户{userInfo.Uname}({userInfo.Mid})登录成功！");
                }
                return userInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"用户登录失败");
                return null;
            }
        }

        /// <summary>
        /// 预热ffmpeg
        /// </summary>
        /// <returns></returns>
        private void Preheat()
        {
            try
            {
                //加载直播间信息

                _ffmpeg.GetVersion();
                _ffmpeg.GetVideoDevices();
                _ffmpeg.GetAudioDevices();
                _ffmpeg.GetVideoCodecs();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"预加载设备信息失败");
            }
        }
    }
}
