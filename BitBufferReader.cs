using System;

public class BitBufferReader
{
    Byte [] bytes;
    int bitsIn;
    Byte byt;
    int pos;
    int N;

    public BitBufferReader ()
    {
        N = 0;
        pos = 0;
        bitsIn = 0;
    }

    ~BitBufferReader ()
    {

    }

    public void putByte (Byte b)
    {
        Byte [] nbytes;

        N++;
        nbytes = new Byte [N];
        if (bytes != null)
            Array.Copy (bytes, nbytes, N -1);

        bytes = nbytes;
    }

    public int readBit ()
    {
        if (bitsIn == 0)
        {
            byt = bytes [pos++];
            bitsIn = 8;
        }

        return (byt >> --bitsIn) & 1;
    }

    public int readBits (int n)
    {
        int i;
        int y;

        y = 0;
        for (i = 0; i < n; i++)
        {
            y = y | (readBit () << i);
        }

        return y;
    }
}
