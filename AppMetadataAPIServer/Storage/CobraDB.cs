using System.Collections;
using System.Collections.Generic;

namespace AppMetadataAPIServer.Storage
{
    public interface ICobraDB<T>
    {
        void Create(T entry);
    }
    
    /// <summary>
    /// A fabulous in-memory database!
    /// </summary>
    public class CobraDB<T>: ICobraDB<T>
    {
        private readonly IList core = new List<T>();

        public void Create(T entry)
        {
            this.core.Add(entry);
        }
        
    }
}