using MyPlayer.Models.Interfaces;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyPlayer.CommonClasses
{
    internal class IDReferenceResolver : IReferenceResolver
    {
        private readonly IDictionary<int, IMediaBase> _data = new Dictionary<int, IMediaBase>();

        public object ResolveReference(object context, string reference)
        {
            int id = int.Parse(reference);

            IMediaBase p;
            _data.TryGetValue(id, out p);

            return p;
        }

        public string GetReference(object context, object value)
        {
            IMediaBase p = (IMediaBase)value;
            _data[p.Id] = p;

            return p.Id.ToString();
        }

        public bool IsReferenced(object context, object value)
        {
            IMediaBase p = (IMediaBase)value;

            return _data.ContainsKey(p.Id);
        }

        public void AddReference(object context, string reference, object value)
        {
            int id = int.Parse(reference);

            _data[id] = (IMediaBase)value;
        }
    }
}