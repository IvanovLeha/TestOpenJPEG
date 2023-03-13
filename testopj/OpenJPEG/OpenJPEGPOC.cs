using System;
using System.Runtime.InteropServices;

namespace OpenJPEG
{
    public struct OpenJPEGPOC
    {
        /** Resolution num start, Component num start, given by POC */
        public uint resno0, compno0;
    
        /** Layer num end,Resolution num end, Component num end, given by POC */
        public uint layno1, resno1, compno1;
    
        /** Layer num start,Precinct num start, Precinct num end */
        public uint layno0, precno0, precno1;
    
        /** Progression order enum*/
        public int prg1, prg;
    
        /** Progression order string*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] progorder;
    
        /** Tile number (starting at 1) */
        public uint tile;
    
        /** Start and end values for Tile width and height*/
        public int tx0, tx1, ty0, ty1;
    
        /** Start value, initialised in pi_initialise_encode*/
        public uint layS, resS, compS, prcS;
    
        /** End value, initialised in pi_initialise_encode */
        public uint layE, resE, compE, prcE;
    
        /** Start and end values of Tile width and height, initialised in pi_initialise_encode*/
        public uint txS, txE, tyS, tyE, dx, dy;
    
        /** Temporary values for Tile parts, initialised in pi_create_encode */
        public uint lay_t, res_t, comp_t, prc_t, tx0_t, ty0_t;
    }
}
