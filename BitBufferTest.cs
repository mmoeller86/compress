using System;
using System.IO;

public class BitBufferTest
{
    public static void Main (System.String [] args)
    {
        FileInput c;
        ArithCoding lz;
        LZW lz78;
        int i, j;
        Byte [] blk;
        int abs;

        c = new FileInput (args [1]);
        blk = new Byte [64*1024];
        abs = 0;
        for (j = 0; j < c.getSize (); j++)
        {
            int k;
            int len;
            int rem;

            len = 64*1024;
            rem = c.getSize () - j;
            len = (len < rem ? len : rem);
            for (k = 0; k < len; k++ )
            {
                blk [k] = c.getByte (k);
            }

            j += len;

            lz78 = new LZW (13);
            for (i = 0; i < len; i++) {
                //System.Console.Write ("\r" + (i * 100 / c.getSize ()) + ", " + lz78.getSize ());
                lz78.encode ((char)blk [i]);
            }

            lz78.done ();
            //System.Console.WriteLine ("Compressed " + lz78.getSize () + " Bytes");

            //System.Console.WriteLine ("LZ78...");
            lz = new ArithCoding ();
            for (i = 0; i < lz78.getSize (); i++) {
                //System.Console.Write ("\r" + (i * 100 / lz78.getSize ()) + ", " + lz.getSize ());
                lz.encode ((char)lz78.getByte (i));
            }

            lz.done ();
            abs += lz.getSize ();

            System.Console.Write ("\r" + (j * 100 / c.getSize ()) + "% (" + (float)len / lz.getSize () + ")");
        }

        System.Console.WriteLine ("Compressed to " + abs + " bytes...");
    }
}
