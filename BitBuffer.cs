using System;

public class BitBuffer
{
    System.Byte [] bytes;
    int N;
    int bitsOut;
    System.Byte byt;

    public BitBuffer ()
    {
        bitsOut = 0;
        byt = 0;
        N = 0;
    }

    ~BitBuffer ()
    {

    }

    private void putByte (System.Byte b)
    {
        System.Byte [] nbytes;

        N++;
        nbytes = new System.Byte [N];
        if (bytes != null)
            System.Array.Copy (bytes, nbytes, N-1);

        bytes = nbytes;
    }

    public void writeBit (int bit)
    {
        if (bitsOut == 8)
        {
            putByte (byt);
            bitsOut = 0;
            byt = 0;
        }

        byt <<= 1;
        byt |= (System.Byte)(bit & 1);
        bitsOut++;
    }

    public void writeBits (int bits, int n)
    {
        int i;

        //System.Console.WriteLine ("Writing " + n + " Bits...");
        for (i = 0; i < n; i++)
        {
            writeBit ((bits >> i) & 1);
        }
    }

    public System.Byte [] getBytes ()
    {
        return bytes;
    }

    public int getSize ()
    {
        return N;
    }

    public System.Byte getByte (int pos)
    {
        if (pos < N)
            return bytes [pos];

        return 0;
    }

    public void done ()
    {
        if (bitsOut > 0)
        {
            byt <<= (8 - bitsOut);
            putByte (byt);
        }
    }
}
