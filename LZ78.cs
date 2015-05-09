public class LZ78 : ICompressor
{
    System.String P;
    System.Collections.Hashtable dict;
    BitBuffer b;
    int code_len;
    int next_code;

    public LZ78 (int cl)
    {
        b = new BitBuffer ();
        dict = new System.Collections.Hashtable ();
        P = "";
        code_len = cl;
        next_code = 1;
    }

    ~LZ78 ()
    {

    }

    public override void encode (char by)
    {
        System.String PC;

        PC = P + by;
        if (dict.ContainsKey (PC) == true)
            P = PC;
        else {
            if (P == "")
            {
                b.writeBits (0, code_len);
            } else {
                b.writeBits ((int)dict [P], code_len);
            }

            b.writeBits (by, 8);
            dict.Add (PC, next_code++);
            P = "";
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
