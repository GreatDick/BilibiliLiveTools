﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using BilibiliAutoLiver.Models.Dtos;
using BilibiliAutoLiver.Models.Entities;
using Newtonsoft.Json;

namespace BilibiliAutoLiver.Models.ViewModels
{
    public class PushIndexPageViewModel
    {
        public PushSetting PushSetting { get; set; }

        /// <summary>
        /// 支持的视频设备
        /// </summary>
        public List<VideoDeviceInfo> VideoDevices { get; set; }

        /// <summary>
        /// 设备JSON数据
        /// </summary>
        public string VideoDeivceJson
        {
            get
            {
                return JsonConvert.SerializeObject(VideoDevices);
            }
        }

        /// <summary>
        /// 支持的音频设备
        /// </summary>
        public List<AudioDeviceInfo> AudioDevices { get; set; }

        /// <summary>
        /// 支持的视频编码器
        /// </summary>
        public List<string> VideoCodecs { get; set; }

        /// <summary>
        /// 视频素材
        /// </summary>
        public Dictionary<long, string> Videos { get; set; } = new Dictionary<long, string>();

        /// <summary>
        /// 音频素材
        /// </summary>
        public Dictionary<long, string> Audios { get; set; } = new Dictionary<long, string>();

        /// <summary>
        /// 输出质量
        /// </summary>
        public Dictionary<int, string> OutputQuality { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// 插件
        /// </summary>
        public List<string> Plugins { get; set; }

        public string PluginsToJsArray()
        {
            if (this.Plugins?.Any() != true)
            {
                return "[]";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < this.Plugins.Count; i++)
            {
                if (i == this.Plugins.Count - 1)
                {
                    sb.Append("{ name: '" + this.Plugins[i] + "', value: '" + this.Plugins[i] + "' }");
                }
                else
                {
                    sb.Append("{ name: '" + this.Plugins[i] + "', value: '" + this.Plugins[i] + "' },");
                }
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
