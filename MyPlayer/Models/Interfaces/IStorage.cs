using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IStorage
    {
        void SaveSettings(ISettings settings);
        void SaveQueue(IQueue queue);

        void LoadSettings(ISettings settings);
        void LoadQueue(IQueue queue);
    }
}
