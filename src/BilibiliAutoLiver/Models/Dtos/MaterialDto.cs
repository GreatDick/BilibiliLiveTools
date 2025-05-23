﻿using System;

namespace BilibiliAutoLiver.Models.Dtos
{
    public class MaterialDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }

        public MediaInfo MediaInfo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedTime { get; set; }

        /// <summary>
        /// 是否为队列
        /// </summary>
        public bool IsDemuxConcat
        {
            get
            {
                return this.FullPath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
