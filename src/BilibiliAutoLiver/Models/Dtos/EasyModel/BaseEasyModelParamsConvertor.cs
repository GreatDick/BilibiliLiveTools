﻿using System;
using BilibiliAutoLiver.Models.Entities;
using BilibiliAutoLiver.Utils;

namespace BilibiliAutoLiver.Models.Dtos.EasyModel
{
    public abstract class BaseEasyModelParamsConvertor : IEasyModelParamsConvertor
    {
        public PushSetting Setting { get; }

        public BaseEasyModelParamsConvertor(PushSetting setting)
        {
            this.Setting = setting;
        }

        public abstract void ToEntity(PushSettingUpdateRequest request);

        protected void BaseParamsCheck(PushSettingUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.OutputResolution))
            {
                throw new Exception($"输出分辨率不能为空");
            }
            if (!ResolutionHelper.TryParse(request.OutputResolution, out int width, out int height))
            {
                throw new Exception($"错误的输入分辨率：{request.OutputResolution}");
            }
        }
    }
}