using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Editor_Sample
{
    public interface IEditorBaseService
    {
        Task<string> GetEditorDefaultHTML();
    }
}
