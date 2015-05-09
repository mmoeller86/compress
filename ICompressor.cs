using System;

public class ICompressor
{
    public ICompressor ()
    {
    }

    ~ICompressor ()
    {

    }

    virtual public void encode (char ch) { }
    virtual public int getPos () { return 0; }
    virtual public Byte getByte (int pos) { return 0; }
    virtual public int getSize () { return 0; }
    virtual public void done () { }
}
