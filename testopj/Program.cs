using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenJPEG;

namespace testopj
{
    /// <summary>
    /// Simple example of interaction with OpenJPEG library
    /// Converting tiff gray 9...16 bit to jp2 and vice versa
    /// tested with OpenJPEG v2.5.0 x86-32
    /// requires openjp2.dll
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("OpenJPEG version: " + Marshal.PtrToStringAnsi(OpenJPEGFunctions.opj_version()));
            
            ParallelConvertDirectoryTiffToJP2("tiffIn", "jp2", 16);
            
            ParallelConvertDirectoryJP2ToTiff("jp2", "tiffOut");
            
            Console.Write("Press return to continue . . . ");
            Console.ReadLine();
        }
        
        private static void ParallelConvertDirectoryJP2ToTiff(string dirIn, string dirOut)
        {
            Directory.CreateDirectory(dirOut);
            Parallel.ForEach(Directory.EnumerateFiles(dirIn, "*.jp2"), f => ConvertFileJP2ToTiff(f, dirOut));
        }
        
        private static void ParallelConvertDirectoryTiffToJP2(string dirIn, string dirOut, int actualBPP)
        {
            Directory.CreateDirectory(dirOut);
            Parallel.ForEach(Directory.EnumerateFiles(dirIn, "*.tif*"), f => ConvertFileTiffToJP2(f, dirOut, actualBPP, 10));
        }
        
        private static void ConvertFileJP2ToTiff(string fname, string dirOut)
        {
            var fOut = Path.GetFileNameWithoutExtension(fname) + ".tiff";
            Console.WriteLine("Convert {0} -> {1}", Path.GetFileName(fname), fOut);
            
            var od = new OPJDecode();
            var bmp = od.DecodeGray16(File.ReadAllBytes(fname));
            if (bmp.ActualBPP < 9 || bmp.ActualBPP > 16)
                throw new FormatException("unsupported pixel format");
            
            using (var s = new FileStream(Path.Combine(dirOut, fOut), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var bs = BitmapSource.Create(bmp.W, bmp.H, 0, 0, PixelFormats.Gray16, null, bmp.Pixels, 2 * bmp.W);
                var encoder = new TiffBitmapEncoder();
                encoder.Compression = TiffCompressOption.None;
                encoder.Frames.Add(BitmapFrame.Create(bs));
                encoder.Save(s);
            }
        }
        
        private static void ConvertFileTiffToJP2(string fname, string dirOut, int actualBPP, int r)
        {
            var fOut = Path.GetFileNameWithoutExtension(fname) + ".jp2";
            Console.WriteLine("Convert {0} -> {1}", Path.GetFileName(fname), fOut);
            var bmp = LoadTiff(fname, actualBPP);
            var oe = new OPJEncode();
            var jp2 = oe.Encode(bmp, r);
            File.WriteAllBytes(Path.Combine(dirOut, fOut), jp2);
        }
        
        private static BMPGray16 LoadTiff(string fname, int actualBPP)
        {
            using (var s = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var d = new TiffBitmapDecoder(s, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                var f = d.Frames[0];
                
                if (f.Format != PixelFormats.Gray16)
                    throw new FormatException("unsupported pixel format");
                
                var px = new ushort[f.PixelWidth * f.PixelHeight];
                f.CopyPixels(px, 2 * f.PixelWidth, 0);
                
                return new BMPGray16(f.PixelWidth, f.PixelHeight, actualBPP, px);
            }
        }
    }
}
