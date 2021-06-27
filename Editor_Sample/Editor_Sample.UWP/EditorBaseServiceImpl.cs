using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(Editor_Sample.UWP.EditorBaseServiceImpl))]
namespace Editor_Sample.UWP
{
    public class EditorBaseServiceImpl : IEditorBaseService
    {
        public async Task<string> GetEditorDefaultHTML()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets//TextEditor.html"));
            string text = await Windows.Storage.FileIO.ReadTextAsync(file);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
            return text;
        }
    }
}
