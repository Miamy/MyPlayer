using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IMediaBase 
    {
        string Name { get; set; }
        int Count { get; }

        IList<IMediaBase> Children { get; set; }

        string Container { get; set; }

        string GetUniqueName();
    }
}
