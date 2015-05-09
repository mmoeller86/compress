using System;
using System.Collections;

public class LZWDecompr : IDecompress
{
    Hashtable dict;
    BitBufferReader r;
    Byte [] bytes;
    int N;
    String P;
    int cW, pW;
    int next_code;
    int code_len;

    public LZWDecompr (int cl)
    {
        code_len = cl;
        dict = new Hashtable ();
        r = new BitBufferReader ();
        N = 0;
        next_code = 0;

        initChars ();
    }

    private void initChars ()
    {
        int i;

        for (i = 0; i < 256; i++)
        {
            String s;

            s = "" + (char)i;
            dict.Add (s, next_code++);
        }
    }

    public void putByte (Byte b)
    {
        r.putByte (b);
    }

    private void add (String s)
    {
        int i;

        for (i = 0; i < s.Length; i++)
        {
            Byte [] nbytes;

            N++;
            nbytes = new Byte [N];
            if (bytes != null)
                Array.Copy (bytes, nbytes, N -1);

            nbytes [N -1] = (Byte)s [i];
            bytes = nbytes;
        }
    }

    public override void start ()
    {
        cW = r.readBits (code_len);
        add ((String)dict [cW]);
    }

    public override void decode ()
    {
        char C;

        pW = cW;
        cW = r.readBits (code_len);
        if (dict [cW] != null)
        {
            add ((String)dict [cW]);
            P = (String)dict [pW];
            C = P [0];
            dict.Add (P + C, next_code++);
        } else {
            P = (String)dict [pW];
            C = P [0];
            add (P + C);
            dict.Add (P + C, next_code++);
        }
    }
}
