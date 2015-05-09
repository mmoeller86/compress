using System;

public class AcModel
{
    private const int Max_frequency = 16383;
    int nsym;
    int adapt;
    int [] freq;
    int [] cfreq;

    public void setNSym (int n)
    {
        nsym = n;
    }

    public int getNSym ()
    {
        return nsym;
    }

    public void allocFreq (int n)
    {
        freq = new int [n];
    }

    public void allocCFreq (int n)
    {
        cfreq = new int [n];
    }

    public void setAdapt (int a)
    {
        adapt = a;
    }

    public int getAdapt ()
    {
        return adapt;
    }

    public int getFreq (int i)
    {
        return freq [i];
    }

    public void setFreq (int i, int x)
    {
        freq [i] = x;
    }

    public int getCFreq (int i)
    {
        return cfreq [i];
    }

    public void setCFreq (int i, int x)
    {
        cfreq [i] = x;
    }

    public AcModel (int nsym, int [] ifreq, int adapt)
    {
      int i;

      setNSym (nsym);
      allocFreq (nsym);
      allocCFreq (nsym +1);
      setAdapt (adapt);

      if (ifreq != null)  {
        setCFreq (getNSym (), 0);

        for (i=getNSym ()-1; i>=0; i--)  {
          setFreq (i, ifreq [i]);
          setCFreq (i, getCFreq (i +1) + getFreq (i));
        }

        if (getCFreq (0) > Max_frequency)
          ; //error ("arithmetic coder model max frequency exceeded");
      }  else  {
          for (i=0; i< getNSym (); i++) {
            setFreq (i, 1);
            setCFreq (i, getNSym () -i);
          }

          setCFreq (getNSym (), 0);
      }

      return;
    }

}
