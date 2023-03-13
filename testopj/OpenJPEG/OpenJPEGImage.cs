using System;

namespace OpenJPEG
{
    public struct OpenJPEGImage
    {
        public uint x0;

        public uint y0;

        public uint x1;

        public uint y1;

        public uint numcomps;

        public OpenJPEGColorSpace color_space;

        public IntPtr comps;

        public IntPtr icc_profile_buf;

        public uint icc_profile_len;
    }
}
