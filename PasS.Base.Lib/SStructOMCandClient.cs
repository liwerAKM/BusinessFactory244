using System;
using System.Collections.Generic;
using System.Text;

namespace PasS.Base.Lib
{

    public class OMCandZZJInfoHeadBusID
    {
        /// <summary>
        /// 直接复制文件到程序相对目录下
        /// </summary>
        public const string File = "FileandInfo";
        /// <summary>
        /// 资源管理器
        /// </summary>
        public const int LCCNExplorer = 1;
        /// <summary>
        /// 资源管理器
        /// </summary>
        public const string Explorer = "Explorer:";
        /// <summary>
        /// 资源管理器 发送文件(夹)<see cref="DragFileInfo"/>
        /// </summary>
        public const string ExplorerSendFile = Explorer + "SFile";
        /// <summary>
        /// 资源管理器 获取文件(夹)
        /// </summary>
        public const string ExplorerGetFile = Explorer + "GFile";
        /// <summary>
        /// 复制 文件(夹)
        /// </summary>
        public const string ExplorerCopyF = Explorer + "CopyF";
        /// <summary>
        /// 移动 文件(夹)
        /// </summary>
        public const string ExplorerMoveF = Explorer + "MoveF";
        /// <summary>
        /// 删除文件(夹)
        /// </summary>
        public const string ExplorerDelF = Explorer + "DelF";
        /// <summary>
        /// 新建文件(夹)
        /// </summary>
        public const string ExplorerCreateF = Explorer + "CreateF";
        /// <summary>
        /// 重命名文件(夹)
        /// </summary>
        public const string ExplorerReNameF = Explorer + "ReNameF";
        /// <summary>
        /// 打开、运行文件 
        /// </summary>
        public const string ExplorerOpenRunF = Explorer + "OpenRunF";

        /// <summary>
        ///  屏幕及控制
        /// </summary>
        public const string Screen = "Screen:";
        /// <summary>
        /// 通知发送屏幕
        /// </summary>
        public const string ScreenBegin = "Screen:Begin";
        /// <summary>
        /// 通知结束屏幕发送 
        /// </summary>
        public const string ScreenEnd = "Screen:End";
        /// <summary>
        /// 屏幕截图发送 
        /// </summary>
        public const string ScreenImage = "Screen:Image";
        /// <summary>
        /// 控制鼠标 
        /// </summary>
        public const string ScreenMOUSE = "Screen:MOUSE";
        /// <summary>
        /// 控制键盘 
        /// </summary>
        public const string ScreenKEYBOARD = "Screen:KEYBOARD";

    }
    public class SOMCandClientFileInfo
    {

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后写入时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        ///文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        ///文件相对路径或者绝对路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///FilePath 是相对还是绝对路径
        /// </summary>
        public bool AP { get; set; }

    }
}
