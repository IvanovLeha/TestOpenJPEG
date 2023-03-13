using System;
using System.Runtime.InteropServices;

namespace OpenJPEG
{
    public static class OpenJPEGFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int opj_stream_read_fn(IntPtr buffer, int bytes, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int opj_stream_write_fn(IntPtr buffer, int bytes, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate long opj_stream_skip_fn(long bytes, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int opj_stream_seek_fn(long bytes, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void opj_stream_free_user_data_fn(IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void opj_msg_callback(string msg, IntPtr clientData);
		
		
        [DllImport("openjp2.dll")]
        public static extern IntPtr opj_version();
		
        [DllImport("openjp2.dll")]
        public static extern IntPtr opj_image_create(uint numcmpts, IntPtr cmptparms, int clrspc);
		
        [DllImport("openjp2.dll")]
        public static extern IntPtr opj_stream_default_create(int isInput);
        
        [DllImport("openjp2.dll")]
        public static extern IntPtr opj_stream_create_default_file_stream([MarshalAs(UnmanagedType.LPStr)]string fname, int isInput);
		
        [DllImport("openjp2.dll")]
        public static extern void opj_stream_destroy(IntPtr stream);
		
        [DllImport("openjp2.dll")]
        public static extern void opj_stream_set_read_function(IntPtr stream, opj_stream_read_fn fn);

        [DllImport("openjp2.dll")]
        public static extern void opj_stream_set_write_function(IntPtr stream, opj_stream_write_fn fn);

        [DllImport("openjp2.dll")]
        public static extern void opj_stream_set_skip_function(IntPtr stream, opj_stream_skip_fn fn);

        [DllImport("openjp2.dll")]
        public static extern void opj_stream_set_seek_function(IntPtr stream, opj_stream_seek_fn fn);

        [DllImport("openjp2.dll")]
        public static extern void opj_stream_set_user_data(IntPtr stream, IntPtr userData, opj_stream_free_user_data_fn fn);

        [DllImport("openjp2.dll")]
        public static extern void opj_stream_set_user_data_length(IntPtr stream, ulong length);

        [DllImport("openjp2.dll")]
        public static extern IntPtr opj_create_decompress(int format);
        
        [DllImport("openjp2.dll")]
        public static extern IntPtr opj_create_compress(int format);

        [DllImport("openjp2.dll")]
        public static extern void opj_destroy_codec(IntPtr codec);

        [DllImport("openjp2.dll")]
        public static extern int opj_end_decompress(IntPtr codec, IntPtr stream);

        [DllImport("openjp2.dll")]
        public static extern void opj_set_default_decoder_parameters(IntPtr parameters);
        
        [DllImport("openjp2.dll")]
        public static extern void opj_set_default_encoder_parameters(IntPtr parameters);

        [DllImport("openjp2.dll")]
        public static extern int opj_setup_decoder(IntPtr codec, IntPtr parameters);
        
        [DllImport("openjp2.dll")]
        public static extern int opj_setup_encoder(IntPtr codec, IntPtr parameters, IntPtr image);

        [DllImport("openjp2.dll")]
        public static extern int opj_read_header(IntPtr stream, IntPtr codec, IntPtr image);

        [DllImport("openjp2.dll")]
        public static extern int opj_decode(IntPtr codec, IntPtr stream, IntPtr image);
        
        [DllImport("openjp2.dll")]
        public static extern int opj_start_compress(IntPtr codec, IntPtr image, IntPtr stream);
        
        [DllImport("openjp2.dll")]
        public static extern int opj_end_compress(IntPtr codec, IntPtr stream);
        
        [DllImport("openjp2.dll")]
        public static extern int opj_encode(IntPtr codec, IntPtr stream);

        [DllImport("openjp2.dll")]
        public static extern void opj_image_destroy(IntPtr image);

        [DllImport("openjp2.dll")]
        public static extern int opj_set_info_handler(IntPtr codec, opj_msg_callback cb, IntPtr userData);

        [DllImport("openjp2.dll")]
        public static extern int opj_set_warning_handler(IntPtr codec, opj_msg_callback cb, IntPtr userData);

        [DllImport("openjp2.dll")]
        public static extern int opj_set_error_handler(IntPtr codec, opj_msg_callback cb, IntPtr userData);
    }
}
