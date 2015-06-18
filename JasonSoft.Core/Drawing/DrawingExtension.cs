using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace JasonSoft.Drawing
{
    public static class DrawingExtension
    {

        public static void Resize(this Image source, String newFileName, Int64 quality)
        {
            source.Resize(newFileName, new Size(source.Width, source.Height), quality);
        }

        public static void Resize(this Image source, String newFileName, Size size, Int64 quality)
        {
            source.Resize(newFileName, size, quality, ContentAlignment.MiddleCenter, ThumbMode.Full);
        }

        public static Image Resize(this Image source, Size newSize, Int64 quality)
        {
            return source.Resize(newSize, quality, ContentAlignment.MiddleCenter, ThumbMode.Full);
        }

        public static void Resize(this Image source, String newFilename, Size newSize, long quality, ContentAlignment contentAlignment, ThumbMode mode)
        {
            Image image = source.Resize(newSize, quality, contentAlignment, mode);
            
            using (EncoderParameters encoderParams = new EncoderParameters(1))
            {
                using (EncoderParameter parameter = (encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality)))
                {
                    ImageCodecInfo encoder = null;
                    //取得擴展名
                    string ext = Path.GetExtension(newFilename);
                    if (string.IsNullOrEmpty(ext))
                        ext = ".jpg";
                    //根據擴展名得到解碼、編碼器
                    foreach (ImageCodecInfo codecInfo in ImageCodecInfo.GetImageEncoders())
                    {
                        if (Regex.IsMatch(codecInfo.FilenameExtension, string.Format(@"(;|^)\*\{0}(;|$)", ext), RegexOptions.IgnoreCase))
                        {
                            encoder = codecInfo;
                            break;
                        }
                    }

                    DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(newFilename));
                    if(dir.Exists == false) dir.Create(); 
                    image.Save(newFilename, encoder, encoderParams);
                }
            }
        }

        public static Image Resize(this Image source, Size newSize, long quality, ContentAlignment contentAlignment, ThumbMode mode) 
        {
            //Reference: http://www.cnblogs.com/dao/archive/2008/02/24/1079571.html
            if (newSize.IsEmpty || source.Size.IsEmpty) return source;


            //先取一個寬比例。
            double scale = (double)source.Width / (double)newSize.Width;

            //縮略模式
            switch (mode)
            {
                case ThumbMode.Full:
                    if (source.Height > source.Width)
                        scale = (double)source.Height / (double)newSize.Height;
                    break;
                case ThumbMode.Max:
                    if (source.Height / scale < newSize.Height)
                        scale = (double)source.Height / (double)newSize.Height;
                    break;
            }

            SizeF newSzie = new SizeF((float)(source.Width / scale), (float)(source.Height / scale));
            Bitmap newImage = new Bitmap(newSize.Width, newSize.Height);

            if(!source.PropertyItems.IsNullOrEmpty())
            {
                foreach (PropertyItem propertyItem in source.PropertyItems)
                {
                    newImage.SetPropertyItem(propertyItem);
                }
            }

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), newSize));
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                //對齊方式
                RectangleF destRect;
                switch (contentAlignment)
                {
                    case ContentAlignment.TopCenter:
                        destRect = new RectangleF(new PointF(-(float)((newSzie.Width - newSize.Width) * 0.5), 0), newSzie);
                        break;
                    case ContentAlignment.TopRight:
                        destRect = new RectangleF(new PointF(-(float)(newSzie.Width - newSize.Width), 0), newSzie);
                        break;
                    case ContentAlignment.MiddleLeft:
                        destRect = new RectangleF(new PointF(0, -(float)((newSzie.Height - newSize.Height) * 0.5)), newSzie);
                        break;
                    case ContentAlignment.MiddleCenter:
                        destRect = new RectangleF(new PointF(-(float)((newSzie.Width - newSize.Width) * 0.5), -(float)((newSzie.Height - newSize.Height) * 0.5)), newSzie);
                        break;
                    case ContentAlignment.MiddleRight:
                        destRect = new RectangleF(new PointF(-(float)(newSzie.Width - newSize.Width), -(float)((newSzie.Height - newSize.Height) * 0.5)), newSzie);
                        break;
                    case ContentAlignment.BottomLeft:
                        destRect = new RectangleF(new PointF(0, -(float)(newSzie.Height - newSize.Height)), newSzie);
                        break;
                    case ContentAlignment.BottomCenter:
                        destRect = new RectangleF(new PointF(-(float)((newSzie.Width - newSize.Width) * 0.5), -(float)(newSzie.Height - newSize.Height)), newSzie);
                        break;
                    case ContentAlignment.BottomRight:
                        destRect = new RectangleF(new PointF(-(float)(newSzie.Width - newSize.Width), -(float)(newSzie.Height - newSize.Height)), newSzie);
                        break;
                    default:
                        destRect = new RectangleF(new PointF(0, 0), newSzie);
                        break;
                }        
                g.DrawImage(source, destRect, new RectangleF(new PointF(0F, 0F), source.Size), GraphicsUnit.Pixel);
             
                //source.Dispose();
            }

            return newImage;
        }
       
    }

    public enum ThumbMode : byte
    {
        /// <summary>
        /// 完整模式
        /// </summary>
        Full = 1,
        /// <summary>
        /// 最大尺寸
        /// </summary>
        Max
    }
}


