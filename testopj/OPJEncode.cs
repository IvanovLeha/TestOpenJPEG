using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenJPEG;

namespace testopj
{
    public class OPJEncode
    {
        private readonly OpenJPEGFunctions.opj_msg_callback _cbInfo;
        private readonly OpenJPEGFunctions.opj_msg_callback _cbWarning;
        private readonly OpenJPEGFunctions.opj_msg_callback _cbError;
        private readonly OpenJPEGFunctions.opj_stream_write_fn _writeFn;
        private readonly OpenJPEGFunctions.opj_stream_skip_fn _skipFn;
        private readonly OpenJPEGFunctions.opj_stream_seek_fn _seekFn;
        
        private MemoryStream _s;
        
        public OPJEncode()
        {
            _cbInfo = new OpenJPEGFunctions.opj_msg_callback(MsgInfo);
            _cbWarning = new OpenJPEGFunctions.opj_msg_callback(MsgWarning);
            _cbError = new OpenJPEGFunctions.opj_msg_callback(MsgError);
            _writeFn = new OpenJPEGFunctions.opj_stream_write_fn(Write);
            _skipFn = new OpenJPEGFunctions.opj_stream_skip_fn(Skip);
            _seekFn = new OpenJPEGFunctions.opj_stream_seek_fn(Seek);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="r">quality 0-100</param>
        /// <returns></returns>
        public byte[] Encode(BMPGray16 bmp, float r)
        {
            _s = new MemoryStream();
            
            var opjImage = IntPtr.Zero;
            var opjCodec = IntPtr.Zero;
            var opjStream = IntPtr.Zero;
            
            var encoderParametersPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(OpenJPEGEncoderParameters)));
            var imageCreateParamPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(OpenJPEGImageCompParam)));
            
            try
            {
                var createImageParam = new OpenJPEGImageCompParam
                {
                    w = (uint)bmp.W,
                    h = (uint)bmp.H,
                    prec = (uint)bmp.ActualBPP,
                    dx = 1,
                    dy = 1
                };
                
                Marshal.StructureToPtr(createImageParam, imageCreateParamPtr, false);
                opjImage = OpenJPEGFunctions.opj_image_create(1, imageCreateParamPtr, (int)OpenJPEGColorSpace.Gray);
                if (opjImage == IntPtr.Zero)
                    throw new OPJException();
                
                var img = Marshal.PtrToStructure<OpenJPEGImage>(opjImage);
                var imgComp = Marshal.PtrToStructure<OpenJPEGImageComp>(img.comps);
                    
                img.x1 = createImageParam.w;
                img.y1 = createImageParam.h;
                Marshal.StructureToPtr(img, opjImage, false);
                    
                unsafe
                {
                    var p = (byte*)imgComp.data;
                    for (var n = 0; n < bmp.Pixels.Length; ++n)
                    {
                        var v = bmp.Pixels[n] >> (16 - bmp.ActualBPP);
                            
                        *p = (byte)(v & 0x00ff);
                        ++p;
                            
                        *p = (byte)(v >> 8);
                        p += 3;
                    }
                }
                
                opjCodec = OpenJPEGFunctions.opj_create_compress((int)OpenJPEGCodecFormat.JP2);
                OpenJPEGFunctions.opj_set_info_handler(opjCodec, _cbInfo, IntPtr.Zero);
                OpenJPEGFunctions.opj_set_warning_handler(opjCodec, _cbWarning, IntPtr.Zero);
                OpenJPEGFunctions.opj_set_error_handler(opjCodec, _cbError, IntPtr.Zero);
                
                OpenJPEGFunctions.opj_set_default_encoder_parameters(encoderParametersPtr);
                var ep = Marshal.PtrToStructure<OpenJPEGEncoderParameters>(encoderParametersPtr);
                ep.cp_disto_alloc = 1;
                ep.tcp_numlayers = 1;
                ep.tcp_rates[0] = r;
                
                Marshal.StructureToPtr(ep, encoderParametersPtr, false);
                if (OpenJPEGFunctions.opj_setup_encoder(opjCodec, encoderParametersPtr, opjImage) != (int)OpenJPEGBool.True)
                    throw new OPJException();
                
                opjStream = OpenJPEGFunctions.opj_stream_default_create((int)OpenJPEGBool.False);
                //opjStream = OpenJPEGFunctions.opj_stream_create_default_file_stream("111.j2k", (int)OpenJPEGBool.False);
                if (opjStream == IntPtr.Zero)
                    throw new OPJException();
                    
                OpenJPEGFunctions.opj_stream_set_write_function(opjStream, _writeFn);
                OpenJPEGFunctions.opj_stream_set_skip_function(opjStream, _skipFn);
                OpenJPEGFunctions.opj_stream_set_seek_function(opjStream, _seekFn);
                
                if (OpenJPEGFunctions.opj_start_compress(opjCodec, opjImage, opjStream) != (int)OpenJPEGBool.True)
                    throw new OPJException();
                    
                if (OpenJPEGFunctions.opj_encode(opjCodec, opjStream) != (int)OpenJPEGBool.True)
                    throw new OPJException();
                    
                if (OpenJPEGFunctions.opj_end_compress(opjCodec, opjStream) != (int)OpenJPEGBool.True)
                    throw new OPJException();
                    
                return _s.ToArray();
            }
            finally
            {
                _s = null;
                Marshal.FreeHGlobal(imageCreateParamPtr);
                Marshal.FreeHGlobal(encoderParametersPtr);
                
                if (opjImage != IntPtr.Zero)
                    OpenJPEGFunctions.opj_image_destroy(opjImage);
                
                if (opjCodec != IntPtr.Zero)
                    OpenJPEGFunctions.opj_destroy_codec(opjCodec);
                
                if (opjStream != IntPtr.Zero)
                    OpenJPEGFunctions.opj_stream_destroy(opjStream);
            }
        }
        
        private void MsgInfo(string msg, IntPtr clientData)
        {
            Console.Write("info {0}", msg);
        }
        
        private void MsgWarning(string msg, IntPtr clientData)
        {
            Console.Write("warning {0}", msg);
        }
        
        private void MsgError(string msg, IntPtr clientData)
        {
            Console.Write("error {0}", msg);
        }
        
        private int Write(IntPtr buffer, int bytes, IntPtr userData)
        {
            unsafe
            {
                var src = new UnmanagedMemoryStream((byte*)buffer.ToPointer(), bytes, bytes, FileAccess.Read);
                src.CopyTo(_s);
            }
            
            return bytes;
        }
        
        private long Skip(long bytes, IntPtr userData)
        {
            _s.Position += bytes;
            return bytes;
        }
        
        private int Seek(long bytes, IntPtr userData)
        {
            _s.Position = bytes;
            return 1;
        }
    }
}
