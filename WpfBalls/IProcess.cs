using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfBalls
{
    interface IProcess
    {
        string  DisplayName;
        KeyValuePair<string,double>[] GetParameters;
        Emgu.CV.IImage    Call(  KeyValuePair<string,double> [] parameters);
        int     ImageChannels;
        int     Depth;
    }
}
