using System;
using System.IO;

public class FileInput : ICompressor
{
    FileStream s;

    public FileInput (String filename)
    {
        s = new FileStream (filename, FileMode.Open, FileAccess.Read);
    }

    ~FileInput ()
    {
        s.Close ();
    }

    public override Byte getByte (int pos)
    {
        return (Byte)s.ReadByte ();
    }

    public override int getSize ()
    {
        return (int)s.Length;
    }

    public override void done ()
    {

    }
}
