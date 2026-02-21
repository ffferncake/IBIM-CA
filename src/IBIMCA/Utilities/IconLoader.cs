using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IBIMCA.Utilities
{
    internal static class IconLoader
    {
        public static ImageSource LoadPng(string folder, string fileName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var asmName = asm.GetName().Name;
            var resourcePath = $"{asmName}.Resources.{folder}.{fileName}";

            using Stream s = asm.GetManifestResourceStream(resourcePath)
                ?? throw new FileNotFoundException($"Embedded resource not found: {resourcePath}");

            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = s;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bmp.EndInit();
            bmp.Freeze();

            // 🔥 72dpi -> 96dpi 로 "픽셀 크기 유지" 정규화
            if (Math.Abs(bmp.DpiX - 96.0) > 0.1 || Math.Abs(bmp.DpiY - 96.0) > 0.1)
            {
                int w = bmp.PixelWidth;
                int h = bmp.PixelHeight;

                var dv = new DrawingVisual();
                using (var dc = dv.RenderOpen())
                {
                    dc.DrawImage(bmp, new System.Windows.Rect(0, 0, w, h));
                }

                var rtb = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(dv);
                rtb.Freeze();
                return rtb;
            }

            return bmp;
        }
    }
}