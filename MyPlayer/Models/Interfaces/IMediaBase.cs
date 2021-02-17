using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IMediaBase 
    {
        string Name { get; set; }
        int Count { get; }

        TimeSpan Duration { get; set; }

        IList<IMediaBase> Children { get; set; }

        string Container { get; set; }

        string GetUniqueName();
        bool HasChildren { get; }

        bool IsSelected{ get; set; }

    }
}
