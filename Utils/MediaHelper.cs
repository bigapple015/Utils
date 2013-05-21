using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

namespace Com.Utility.Commons
{
    /// <summary>
    /// 多媒体文件类
    /// </summary>
    public class MediaHelper
    {
        #region 已同步的方式播放wav文件

        /// <summary>
        /// 同步播放文件
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="wavFilePath"></param>
        public static void SyncPlayWav(SoundPlayer sp,string wavFilePath)
        {
            //设置音频文件路径
            sp.SoundLocation = wavFilePath;
            //异步加载音频文件
            sp.LoadAsync();

            while(!sp.IsLoadCompleted)
            {
                Thread.Sleep(10);
            }
            sp.PlaySync();
        }


        /// <summary>
        /// 同步播放文件
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="wavFilePath"></param>
        public static void SyncPlayWav(string wavFilePath)
        {
            SoundPlayer sp = new SoundPlayer();
            //设置音频文件路径
            sp.SoundLocation = wavFilePath;
            //异步加载音频文件
            sp.LoadAsync();

            while (!sp.IsLoadCompleted)
            {
                Thread.Sleep(10);
            }
            sp.PlaySync();
        }

        #endregion

        #region 以异步的方式播放wav文件

        /// <summary>
        /// 异步播放文件
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="wavPath"></param>
        public static void ASyncPlayWav(SoundPlayer sp,string wavPath)
        {
            sp.SoundLocation = wavPath;
            //使用同步方式加载wav文件
            sp.Load();
            sp.Play();
        }

        /// <summary>
        /// 异步播放文件
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="wavPath"></param>
        public static void ASyncPlayWav( string wavPath)
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = wavPath;
            //使用同步方式加载wav文件
            sp.Load();
            sp.Play();
        }

        #endregion


        #region 停止播放音频


        public static void StopWav(SoundPlayer player)
        {
            if(player != null)
            {
                player.Stop();
            }
        }

        #endregion
    }
}
