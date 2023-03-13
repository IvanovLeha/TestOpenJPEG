using System;

namespace testopj
{
    public class BMPGray16
    {
        public BMPGray16(int w, int h, int actualBPP, ushort[] pixels)
        {
            W = w;
            H = h;
            ActualBPP = actualBPP;
            Pixels = pixels;
        }
        
        public int W { get; private set; }
        
        public int H { get; private set; }
        
        public int ActualBPP { get; private set; }
        
        public ushort[] Pixels { get; private set; }
    }
}
