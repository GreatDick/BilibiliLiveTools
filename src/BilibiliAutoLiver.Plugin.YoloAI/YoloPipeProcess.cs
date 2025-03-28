﻿using BilibiliAutoLiver.Plugin.Base;
using SkiaSharp;

namespace BilibiliAutoLiver.Plugin.YoloAI
{
    public class YoloPipeProcess : IPipeProcess
    {
        /// <summary>
        /// 插件执行序号
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; } = "YoloV8检测";

        public YoloPipeProcess()
        {
            this.Index = 99;
        }

        public SKBitmap Process(SKBitmap bitmap)
        {
            return bitmap;
        }

        public void Dispose()
        {

        }
    }
}
