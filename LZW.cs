using System;

public class LZW : ICompressor
{
    System.String P;
    System.Collections.Hashtable dict;
    BitBuffer b;
    int code_len;
    int next_code;

    public LZW (int cl)
    {
        b = new BitBuffer ();
        dict = new System.Collections.Hashtable ();
        P = "";
        code_len = cl;
        next_code = 0;

        initChars ();
    }

    ~LZW ()
    {

    }

    public void initChars ()
    {
        int i;

        for (i = 0; i < 256; i++ )
        {
            String s;

            s = "" + (char)i;
            dict.Add (s, next_code++);
        }
    }

    public override void encode (char by)
    {
        System.String PC;

        PC = P + by;
        if (dict.ContainsKey (PC) == true)
            P = PC;
        else {
            b.writeBits ((int)dict [P], code_len);
            dict.Add (PC, next_code++);
            P = "" + by;
        }
    }

    public override void done ()
    {
        if (P != "")
        {
            b.writeBits ((int)dict [P], code_len);
        }

    }

    public override int getSize ()
    {
        return b.getSize ();
    }

    public override System.Byte getByte (int pos)
    {
        return b.getByte (pos);
    }
}
