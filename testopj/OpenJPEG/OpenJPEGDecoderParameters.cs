using System;
using System.Runtime.InteropServices;

namespace OpenJPEG
{
    public struct OpenJPEGDecoderParameters
    {
        public uint cp_reduce;
    
        public uint cp_layer;
    
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] infile;
    
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] outfile;
    
        public int decod_format;
    
        public int cod_format;

        public uint DA_x0;
    
        public uint DA_x1;
    
        public uint DA_y0;
    
        public uint DA_y1;
    
        public int m_verbose;

        public uint tile_index;
    
        public uint nb_tile_to_decode;

        public int jpwl_correct;
   
        public int jpwl_exp_comps;
    
        public int jpwl_max_tiles;
    
        public uint flags;
    }
}
