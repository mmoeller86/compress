using System;

public class ArithCoding : ICompressor
{
    private const int Code_value_bits = 16;
    private const int Top_value = ((1 << Code_value_bits) -1);
    private const int First_qtr = (Top_value/4 +1);
    private const int Half = (2 * First_qtr);
    private const int Third_qtr = (3 * First_qtr);
    private const int Max_frequency = 16383;
    Byte [] bytes;
    int N;
    Byte buffer;
    int bits_to_go;
    int total_bits;
    int fbits;
    int low, high;
    AcModel acm;

    public ArithCoding ()
    {
        bits_to_go = 8;
        low = 0;
        high = Top_value;
        fbits = 0;
        buffer = 0;
        total_bits = 0;
        buffer = 0;
        N = 0;

        acm = new AcModel (256, null, 1);
    }

    private void put_c (Byte ch)
    {
        Byte [] nbytes;

        N++;
        nbytes = new Byte [N];
        if (bytes != null)
            Array.Copy (bytes, nbytes, N -1);

        nbytes [N -1] = ch;
        bytes = nbytes;
    }

    private void
    output_bit (int bit)
    {
      buffer >>= 1;
      bit &= 1;
      if (bit == 1)
        buffer |= 0x80;
      bits_to_go -= 1;
      total_bits += 1;
      if (bits_to_go==0) {
          put_c (buffer);
          bits_to_go = 8;
      }

      return;
    }

    private void
    bit_plus_follow (int bit)
    {
      output_bit (bit);
      while (fbits > 0)  {
        output_bit (~bit);
        fbits -= 1;
      }

      return;
    }

    private void
    update_model (AcModel acm, int sym)
    {
      int i;

      if (acm.getCFreq (0) == Max_frequency)
      {
        int cum = 0;

        acm.setCFreq (acm.getNSym (), 0);
        for (i = acm.getNSym () -1; i >= 0; i--)
        {
            acm.setFreq (i, (acm.getFreq (i) +1) / 2);
            cum += acm.getFreq (i);
            acm.setCFreq (i, cum);
        }
      }

      acm.setFreq (sym, acm.getFreq (sym) +1);
      for (i=sym; i >= 0; i-- )
        acm.setCFreq (i, acm.getCFreq (i) +1);

      return;
    }

    public override void done ()
    {
        fbits += 1;
      if (low < First_qtr)
        bit_plus_follow (0);
      else
        bit_plus_follow (1);

      put_c ((Byte)(buffer >> bits_to_go));
    }

    private int
    encoder_bits ()
    {
      return total_bits;
    }

    private void
    encode_symbol (AcModel acm, int sym)
    {
      int range;

      range = (high-low)+1;
      high = low + (range*acm.getCFreq (sym))/acm.getCFreq (0)-1;
      low = low + (range*acm.getCFreq (sym+1))/acm.getCFreq (0);

      for (;;)  {
        if (high<Half)  {
          bit_plus_follow (0);
        }  else if (low>=Half)  {
          bit_plus_follow (1);
          low -= Half;
          high -= Half;
        }  else if (low>=First_qtr && high<Third_qtr)  {
          fbits += 1;
          low -= First_qtr;
          high -= First_qtr;
        }  else
          break;
        low = 2*low;
        high = 2*high+1;
      }

      if (acm.getAdapt () == 1)
        update_model (acm, sym);

      return;
    }

    public override Byte
    getByte (int pos)
    {
    	return bytes [pos];
    }

    public override int
    getSize ()
    {
        return N;
    }

    public override void
    encode (char ch)
    {
        encode_symbol (acm, (int)ch);
    }
}
