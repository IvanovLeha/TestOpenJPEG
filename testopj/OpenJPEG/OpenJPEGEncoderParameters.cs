using System;
using System.Runtime.InteropServices;

namespace OpenJPEG
{
    public struct OpenJPEGEncoderParameters
    {
        /** size of tile: tile_size_on = false (not in argument) or = true (in argument) */
        public int tile_size_on;
    
        /** XTOsiz */
        public int cp_tx0;
    
        /** YTOsiz */
        public int cp_ty0;
    
        /** XTsiz */
        public int cp_tdx;
    
        /** YTsiz */
        public int cp_tdy;
    
        /** allocation by rate/distortion */
        public int cp_disto_alloc;
    
        /** allocation by fixed layer */
        public int cp_fixed_alloc;
    
        /** add fixed_quality */
        public int cp_fixed_quality;
    
        /** fixed layer */
        public IntPtr cp_matrice;
    
        /** comment for coding */
        public IntPtr cp_comment;
    
        /** csty : coding style */
        public int csty;
    
        /** progression order (default OPJ_LRCP) */
        public int prog_order;
    
        /** progression order changes */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public OpenJPEGPOC[] POC;
    
        /** number of progression order changes (POC), default to 0 */
        public uint numpocs;
    
        /** number of layers */
        public int tcp_numlayers;
    
        /** rates of layers - might be subsequently limited by the max_cs_size field.
     * Should be decreasing. 1 can be
     * used as last value to indicate the last layer is lossless. */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public float[] tcp_rates;
    
        /** different psnr for successive layers. Should be increasing. 0 can be
     * used as last value to indicate the last layer is lossless. */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public float[] tcp_distoratio;
    
        /** number of resolutions */
        public int numresolution;
    
        /** initial code block width, default to 64 */
        public int cblockw_init;
    
        /** initial code block height, default to 64 */
        public int cblockh_init;
    
        /** mode switch (cblk_style) */
        public int mode;
    
        /** 1 : use the irreversible DWT 9-7, 0 : use lossless compression (default) */
        public int irreversible;
    
        /** region of interest: affected component in [0..3], -1 means no ROI */
        public int roi_compno;
    
        /** region of interest: upshift value */
        public int roi_shift;
    
        /* number of precinct size specifications */
        public int res_spec;
    
        /** initial precinct width */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public int[] prcw_init;
    
        /** initial precinct height */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public int[] prch_init;

    
        /**@name command line encoder parameters (not used inside the library) */
        /*@{*/
        /** input file name */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] infile;
    
        /** output file name */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] outfile;
    
        /** DEPRECATED. Index generation is now handled with the opj_encode_with_info() function. Set to NULL */
        public int index_on;
    
        /** DEPRECATED. Index generation is now handled with the opj_encode_with_info() function. Set to NULL */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] index;
    
        /** subimage encoding: origin image offset in x direction */
        public int image_offset_x0;
    
        /** subimage encoding: origin image offset in y direction */
        public int image_offset_y0;
    
        /** subsampling value for dx */
        public int subsampling_dx;
    
        /** subsampling value for dy */
        public int subsampling_dy;
    
        /** input file format 0: PGX, 1: PxM, 2: BMP 3:TIF*/
        public int decod_format;
    
        /** output file format 0: J2K, 1: JP2, 2: JPT */
        public int cod_format;
    
        /*@}*/
        /* UniPG>> */
        /* NOT YET USED IN THE V2 VERSION OF OPENJPEG */
        /**@name JPWL encoding parameters */
        /*@{*/
        /** enables writing of EPC in MH, thus activating JPWL */
        public int jpwl_epc_on;
    
        /** error protection method for MH (0,1,16,32,37-128) */
        public int jpwl_hprot_MH;
    
        /** tile number of header protection specification (>=0) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_hprot_TPH_tileno;
    
        /** error protection methods for TPHs (0,1,16,32,37-128) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_hprot_TPH;
    
        /** tile number of packet protection specification (>=0) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_pprot_tileno;
    
        /** packet number of packet protection specification (>=0) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_pprot_packno;
    
        /** error protection methods for packets (0,1,16,32,37-128) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_pprot;
    
        /** enables writing of ESD, (0=no/1/2 bytes) */
        public int jpwl_sens_size;
    
        /** sensitivity addressing size (0=auto/2/4 bytes) */
        public int jpwl_sens_addr;
    
        /** sensitivity range (0-3) */
        public int jpwl_sens_range;
    
        /** sensitivity method for MH (-1=no,0-7) */
        public int jpwl_sens_MH;
    
        /** tile number of sensitivity specification (>=0) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_sens_TPH_tileno;
    
        /** sensitivity methods for TPHs (-1=no,0-7) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] jpwl_sens_TPH;
    
        /*@}*/
        /* <<UniPG */
        /**
     * DEPRECATED: use RSIZ, OPJ_PROFILE_* and MAX_COMP_SIZE instead
     * Digital Cinema compliance 0-not compliant, 1-compliant
     * */
        public int cp_cinema;
    
        /**
     * Maximum size (in bytes) for each component.
     * If == 0, component size limitation is not considered
     * */
        public int max_comp_size;
    
        /**
     * DEPRECATED: use RSIZ, OPJ_PROFILE_* and OPJ_EXTENSION_* instead
     * Profile name
     * */
        public int cp_rsiz;
    
        /** Tile part generation*/
        public byte tp_on;
    
        /** Flag for Tile part generation*/
        public byte tp_flag;
    
        /** MCT (multiple component transform) */
        public byte tcp_mct;
    
        /** Enable JPIP indexing*/
        public int jpip_on;
    
        /** Naive implementation of MCT restricted to a single reversible array based
        encoding without offset concerning all the components. */
        public IntPtr mct_data;
    
        /**
     * Maximum size (in bytes) for the whole codestream.
     * If == 0, codestream size limitation is not considered
     * If it does not comply with tcp_rates, max_cs_size prevails
     * and a warning is issued.
     * */
        public int max_cs_size;
    
        /** RSIZ value
        To be used to combine OPJ_PROFILE_*, OPJ_EXTENSION_* and (sub)levels values. */
        public ushort rsiz;
    }
}
