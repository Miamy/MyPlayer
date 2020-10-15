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
        int Id { get; set; }
    }
}
