using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class MediaBase : IMediaBase
    {
        private string _name;

        public virtual string Name 
        { 
            get => _name;
            set
            {
                _name = value;
            }
        }
        public virtual int Count => Children == null ? 1 : Children.Count;
        public IList<IMediaBase> Children { get; set; } = null;
        public string Container { get; set; }

        public MediaBase(string name, string container)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual string GetUniqueName()
        {
            return Container;
        }
    }
}
