using System;

namespace OpenJPEG
{
    public struct OpenJPEGImageComp
    {
        public uint dx;

        public uint dy;

        public uint w;

        public uint h;

        public uint x0;

        public uint y0;

        public uint prec;

        public uint bpp;

        public uint sgnd;

        public uint resno_decoded;

        public uint factor;

        public IntPtr data;

        public uint alpha;
    }
}
