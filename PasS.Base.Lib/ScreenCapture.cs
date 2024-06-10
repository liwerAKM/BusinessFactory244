using System;
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 屏幕捕获（带有压缩）
    /// </summary>
    public class ScreenCapture
    {
        /// <summary>
        /// 把当前屏幕捕获到位图对象中
        /// </summary>
        /// <param name="hdcDest">目标设备的句柄</param>
        /// <param name="nXDest">目标对象的左上角的X坐标</param>
        /// <param name="nYDest">目标对象的左上角的X坐标</param>
        /// <param name="nWidth">目标对象的矩形的宽度</param>
        /// <param name="nHeight">目标对象的矩形的长度</param>
        /// <param name="hdcSrc">源设备的句柄</param>
        /// <param name="nXSrc">源对象的左上角的X坐标</param>
        /// <param name="nYSrc">源对象的左上角的X坐标</param>
        /// <param name="dwRop">光栅的操作值</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        int dwRop
        );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr CreateDC(
        string lpszDriver, // 驱动名称
        string lpszDevice, // 设备名称
        string lpszOutput, // 无用，可以设定位"NULL"
        IntPtr lpInitData // 任意的打印机数据
        );

        /// <summary>
        /// 屏幕捕获到位图对象中
        /// </summary>
        /// <returns></returns>
        public static Image Capture(int percent =100)
        {
            //创建显示器的DC
            IntPtr dc1 = CreateDC("DISPLAY", null, null, (IntPtr)null);
            //由一个指定设备的句柄创建一个新的Graphics对象
            Graphics g1 = Graphics.FromHdc(dc1);
            //根据屏幕大小创建一个与之相同大小的Bitmap对象
            Bitmap ScreenImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, g1);

            Graphics g2 = Graphics.FromImage(ScreenImage);
            //获得屏幕的句柄
            IntPtr dc3 = g1.GetHdc();
            //获得位图的句柄
            IntPtr dc2 = g2.GetHdc();
            //把当前屏幕捕获到位图对象中
            BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, 13369376);
            //释放屏幕句柄
            g1.ReleaseHdc(dc3);
            //释放位图句柄
            g2.ReleaseHdc(dc2);

            //压缩图片
            //Image bmp = MakeThumbnail(ScreenImage, ScreenImage.Width * 3 / 4, ScreenImage.Height * 3 / 4);
            if (percent != 100)
            {
                Image bmp = MakeThumbnail(ScreenImage, ScreenImage.Width * percent / 100, ScreenImage.Height * percent / 100);
                return bmp;
            }
            return ScreenImage;
            //ScreenImage.SetResolution(800,600);

        }

        public static byte[] GetCapture(int percent = 100)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image ima = Capture(percent);
                ima.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byers = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byers, 0, (int)ms.Length);
                return byers;
            }
           
        }
        public static byte[] GetCapture2(int percent = 100,long  leave=50)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image ima = Capture(percent);
                Compress(ima, ms, leave);
                byte[] byers = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byers, 0, (int)ms.Length);
                return byers;
            }

        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="originalImage"></param>
        public static Image MakeThumbnail(Image originalImage, int towidth, int toheight)
        {
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight,PixelFormat.Format16bppRgb565);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置低质量,高速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            return bitmap;
        }
        
        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Image srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, jgpEncoder, myEncoderParameters);
           
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        /// <summary>
        /// 执行鼠标事件
        /// </summary>
        /// <param name="code"></param>
        public static  void DoMouseEvent(MouseEvent mouseCode   )
        {
           
            MouseHook hook = new MouseHook();
            if (mouseCode != null)
            {
                switch (mouseCode.Type)
                {
                    case MouseEventType.MouseMove:
                        hook.MouseWork(mouseCode);
                        break;
                    case MouseEventType.MouseClick:
                        hook.MouseWork(mouseCode);
                        break;
                    default:
                        hook.MouseWork(mouseCode);
                        break;
                }
            }
        }

        /// <summary>
        /// 执行键盘事件
        /// </summary>
        /// <param name="code"></param>
        public static void doKeyBoardEvent(KeyBoardEvent keyboardCode)
        {
             
            KeyBoardHook hook = new KeyBoardHook();
            if (keyboardCode != null)
            {
                switch (keyboardCode.Type)
                {
                    case KeyBoardType.Key_Down:
                        KeyBoardHook.KeyDown(keyboardCode.KeyCode);
                        break;
                    case KeyBoardType.Key_Up:
                        KeyBoardHook.KeyUp(keyboardCode.KeyCode);
                        break;
                    default:
                        break;
                }
            }
        }


    }

    /// <summary>
    /// 鼠标事件类型
    /// </summary>
    [Serializable]
    public enum MouseEventType
    {
        MouseMove,
        MouseLeftDown,
        MouseLeftUp,
        MouseRightDown,
        MouseRightUp,
        MouseClick,
        MouseDoubleClick
    }

    /// <summary>
    /// 鼠标事件结构
    /// </summary>
    [Serializable]
    public class MouseEvent  
    {
        private Byte[] type;
        private Byte[] x;
        private Byte[] y;

        /// <summary>
        /// 创建鼠标事件的实例
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="X">鼠标指针的X坐标</param>
        /// <param name="Y">鼠标指针的Y坐标</param>
        public MouseEvent(MouseEventType Type, int X, int Y)
        {
            this.type = BitConverter.GetBytes((int)Type);
            this.x = BitConverter.GetBytes(X);
            this.y = BitConverter.GetBytes(Y);
        }

        public MouseEvent(Byte[] Type, Byte[] X, Byte[] Y)
        {
            this.type = Type;
            this.x = X;
            this.y = Y;
        }

        public MouseEvent(Byte[] Content)
        {
            type = new byte[4];
            x = new byte[4];
            y = new byte[4];
            for (int i = 0; i < Content.Length; i++)
            {
                if (i >= 0 && i < 4)
                    type[i] = Content[i];
                if (i >= 4 && i < 8)
                    x[i - 4] = Content[i];
                if (i >= 8 && i < 12)
                    y[i - 8] = Content[i];
            }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public MouseEventType Type
        {
            get { return (MouseEventType)BitConverter.ToInt32(type, 0); }
        }
        /// <summary>
        /// 鼠标指针的X坐标
        /// </summary>
        public int X
        {
            get { return BitConverter.ToInt32(x, 0); }
        }
        /// <summary>
        /// 鼠标指针的Y坐标
        /// </summary>
        public int Y
        {
            get { return BitConverter.ToInt32(y, 0); }
        }

        public Byte[] ToBytes()
        {
            Byte[] Bytes = new Byte[12];
            type.CopyTo(Bytes, 0);
            x.CopyTo(Bytes, 4);
            y.CopyTo(Bytes, 8);
            return Bytes;
        }
    }

    /// <summary>
    /// 键盘事件类型
    /// </summary>
    [Serializable]
    public enum KeyBoardType
    {
        /// <summary>
        /// 按下按键
        /// </summary>
        Key_Down,
        /// <summary>
        /// 释放按键
        /// </summary>
        Key_Up,
        /// <summary>
        /// 按下并释放按键
        /// </summary>
        Key_Press,
    }

    /// <summary>
    /// 键盘事件结构
    /// </summary>
    [Serializable]
    public class KeyBoardEvent  
    {
        /// <summary>
        /// 键盘事件类型
        /// </summary>
        private KeyBoardType type;

        /// <summary>
        /// 键代码
        /// </summary>
        private System.Windows.Forms.Keys keyCode;

        /// <summary>
        /// 键盘事件类型
        /// </summary>
        public KeyBoardType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 键代码
        /// </summary>
        public System.Windows.Forms.Keys KeyCode
        {
            get { return keyCode; }
            set { keyCode = value; }
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        public KeyBoardEvent()
        {
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyCode"></param>
        public KeyBoardEvent(KeyBoardType type, System.Windows.Forms.Keys keyCode)
        {
            this.type = type;
            this.keyCode = keyCode;
        }
    }

    /// <summary>
    /// 鼠标Hook类
    /// </summary>
    public class MouseHook
    {
        /// <summary>
        /// 鼠标事件枚举
        /// </summary>
        public enum MouseEventFlag
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }
        /// <summary>
        /// 委托-鼠标按键触发
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        public delegate void DoMouseButtons(int flags, int dx, int dy, int dwData, int dwExtraInfo);
        /// <summary>
        /// 委托-鼠标移动触发
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public delegate bool DoMouseMove(int X, int Y);
        /// <summary>
        /// 模拟鼠标按钮按下的事件
        /// </summary>
        private event DoMouseButtons MouseButton;
        /// <summary>
        /// 模拟鼠标移动的事件
        /// </summary>
        private event DoMouseMove MouseMove;

        /// <summary>
        /// 创建鼠标钩子的实例
        /// </summary>
        public MouseHook()
        {
            MouseButton += new DoMouseButtons(Api.mouse_event);
            MouseMove += new DoMouseMove(Api.SetCursorPos);
        }

        /// <summary>
        /// 控制鼠标执行相应操作
        /// </summary>
        /// <param name="MEvent">指定的鼠标事件</param>
        public void MouseWork(MouseEvent MEvent)
        {

            switch (MEvent.Type)
            {
                case MouseEventType.MouseMove:
                    MouseMove(MEvent.X, MEvent.Y);
                    break;
                case MouseEventType.MouseLeftDown:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseLeftUp:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseRightDown:
                    MouseButton((int)MouseEventFlag.RightDown, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseRightUp:
                    MouseButton((int)MouseEventFlag.RightUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseClick:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
                case MouseEventType.MouseDoubleClick:
                    MouseMove(MEvent.X, MEvent.Y);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftDown, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    MouseButton((int)MouseEventFlag.LeftUp, MEvent.X, MEvent.Y, 0, 0);
                    break;
            }
        }
    }
    /// <summary>
    /// 键盘控制
    /// </summary>
    public class KeyBoardHook
    {
        /// <summary>
        /// 按下按键的参数
        /// </summary>
        private const int KEYEVENTF_KEYDOWN = 0x0001;

        /// <summary>
        /// 释放按键的参数
        /// </summary>
        private const int KEYEVENTF_KEYUP = 0x0002;

        /// <summary>
        /// 模拟键盘事件-按下按键
        /// </summary>
        /// <param name="keyCode"></param>
        public static void KeyDown(System.Windows.Forms.Keys keyCode)
        {
            Api.keybd_event((byte)keyCode, 0, KEYEVENTF_KEYDOWN, 0);
        }

        /// <summary>
        /// 模拟键盘事件-释放按键
        /// </summary>
        /// <param name="keyCode"></param>
        public static void KeyUp(System.Windows.Forms.Keys keyCode)
        {
            Api.keybd_event((byte)keyCode, 0, KEYEVENTF_KEYUP, 0);
        }
    }
    /// <summary>
    /// API类
    /// </summary>
    public class Api
    {
        /// <summary>
        /// 模拟鼠标事件的函数模型
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void mouse_event(int flags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 设置光标到指定位置的函数模型
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        /// 模拟键盘事件的函数模型
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            byte bVk,
            byte bScan,
            int dwFlags,
            int dwExtraInfo
        );
    }
}
