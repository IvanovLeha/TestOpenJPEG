using System;
using System.Runtime.InteropServices;
using OpenJPEG;

namespace testopj
{
    public class OPJDecode
    {
        private readonly OpenJPEGFunctions.opj_msg_callback _cbInfo;
        private readonly OpenJPEGFunctions.opj_msg_callback _cbWarning;
        private readonly OpenJPEGFunctions.opj_msg_callback _cbError;
        private readonly OpenJPEGFunctions.opj_stream_free_user_data_fn _freeUserDataFn;
        private readonly OpenJPEGFunctions.opj_stream_read_fn _readFn;
        private readonly OpenJPEGFunctions.opj_stream_skip_fn _skipFn;
        private readonly OpenJPEGFunctions.opj_stream_seek_fn _seekFn;
        
        private byte[] _image;
        private int _offset;
        
        public OPJDecode()
        {
            _cbInfo = new OpenJPEGFunctions.opj_msg_callback(MsgInfo);
            _cbWarning = new OpenJPEGFunctions.opj_msg_callback(MsgWarning);
            _cbError = new OpenJPEGFunctions.opj_msg_callback(MsgError);
            _freeUserDataFn = new OpenJPEGFunctions.opj_stream_free_user_data_fn(FreeUserData);
            _readFn = new OpenJPEGFunctions.opj_stream_read_fn(Read);
            _skipFn = new OpenJPEGFunctions.opj_stream_skip_fn(Skip);
            _seekFn = new OpenJPEGFunctions.opj_stream_seek_fn(Seek);
        }
        
        public BMPGray16 DecodeGray16(byte[] bin)
        {
            _image = bin;
            _offset = 0;
            
            var opjStream = OpenJPEGFunctions.opj_stream_default_create((int)OpenJPEGBool.True);
            var opjCodec = OpenJPEGFunctions.opj_create_decompress((int)OpenJPEGCodecFormat.JP2);
            var headerPtrMem = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(headerPtrMem, IntPtr.Zero);
            
            try
            {
                OpenJPEGFunctions.opj_set_info_handler(opjCodec, _cbInfo, IntPtr.Zero);
                OpenJPEGFunctions.opj_set_warning_handler(opjCodec, _cbWarning, IntPtr.Zero);
                OpenJPEGFunctions.opj_set_error_handler(opjCodec, _cbError, IntPtr.Zero);
                OpenJPEGFunctions.opj_stream_set_read_function(opjStream, _readFn);
                OpenJPEGFunctions.opj_stream_set_skip_function(opjStream, _skipFn);
                OpenJPEGFunctions.opj_stream_set_seek_function(opjStream, _seekFn);
                
                OpenJPEGFunctions.opj_stream_set_user_data(opjStream, IntPtr.Zero, _freeUserDataFn);
                OpenJPEGFunctions.opj_stream_set_user_data_length(opjStream, (ulong)_image.Length);
                
                if (OpenJPEGFunctions.opj_read_header(opjStream, opjCodec, headerPtrMem) != (int)OpenJPEGBool.True)
                    throw new OPJException();
                
                var headerPtr = Marshal.ReadIntPtr(headerPtrMem);
                
                try
                {
                    if (OpenJPEGFunctions.opj_decode(opjCodec, opjStream, headerPtr) != (int)OpenJPEGBool.True)
                        throw new OPJException();
                        
                    var img = Marshal.PtrToStructure<OpenJPEGImage>(headerPtr);
                    var imgComp = Marshal.PtrToStructure<OpenJPEGImageComp>(img.comps);
                    
                    var imgData16 = new ushort[imgComp.w * imgComp.h];
                   
                    unsafe
                    {
                        var p = (byte*)imgComp.data;
                        for (var n = 0; n < imgData16.Length; ++n)
                        {
                            int v = *p;
                            ++p;
                            v |= *p << 8;
                            p += 3;
                                    
                            v <<= (16 - (int)imgComp.prec);
                            imgData16[n] = (ushort)v;
                        }
                    }
                            
                    return new BMPGray16((int)imgComp.w, (int)imgComp.h, (int)imgComp.prec, imgData16);
                }
                finally
                {
                    OpenJPEGFunctions.opj_image_destroy(headerPtr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(headerPtrMem);
                OpenJPEGFunctions.opj_destroy_codec(opjCodec);
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
        
        private void FreeUserData(IntPtr data)
        {
            _image = null;
            _offset = 0;
        }
        
        private int Read(IntPtr buffer, int bytes, IntPtr userData)
        {
            if (_offset == _image.Length)
                return -1;
            
            var l = Math.Min(bytes, _image.Length - _offset);
            Marshal.Copy(_image, _offset, buffer, l);
            _offset += l;
            return l;
        }
        
        private long Skip(long bytes, IntPtr userData)
        {
            var l = Math.Min((int)bytes, _image.Length - _offset);
            _offset += l;
            return l;
        }
        
        private int Seek(long bytes, IntPtr userData)
        {
            if (bytes < 0 || bytes >= _image.Length)
                return -1;
            
            _offset = (int)bytes;
            return 1;
        }
    }
}
