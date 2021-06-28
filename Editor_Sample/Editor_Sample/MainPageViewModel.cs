using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Editor_Sample
{
    public class MainPageViewModel : BaseViewModel
    {
        string text;
        ImageSource image;
        bool isImageVisible;
        string rtfcontent = "";
        string EndHTML = "</body></html>";
        string defaultString = "";
        string evalHTMLJavascript = "";

        public Func<string, Task<string>> EvaluateJavascript { get; set; }

        public MainPageViewModel()
        {
            PickFileCommand = new Command(() => DoPickFile());
            PickImageCommand = new Command(() => DoPickImage());
            CreateTableCommand = new Command(() => CreateTable());
            ExportToRTFCommand = new Command(() => ExportToRTF());
            var task = Task.Run(async () => await ReadFile());
        }



        public ICommand PickFileCommand { get; }

        public ICommand PickImageCommand { get; }

        public ICommand CreateTableCommand { get; }

        public ICommand ExportToRTFCommand { get; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string RTFContent
        {
            get => rtfcontent;
            set => SetProperty(ref rtfcontent, value);
        }

        public string EvalHTMLJavascript
        {
            get => evalHTMLJavascript;
            set => SetProperty(ref evalHTMLJavascript, value);
        }

        public ImageSource Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public bool IsImageVisible
        {
            get => isImageVisible;
            set => SetProperty(ref isImageVisible, value);
        }

        async Task ReadFile()
        {
            defaultString = await DependencyService.Get<IEditorBaseService>().GetEditorDefaultHTML();
            RTFContent = defaultString;
        }

        async void DoPickFile()
        {
            await PickAndShow(PickOptions.Default);
        }

        async void DoPickImage()
        {
            var options = new PickOptions
            {
                PickerTitle = "Please select an image",
                FileTypes = FilePickerFileType.Images,
            };

            await PickAndShow(options);
        }

        async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);

                if (result != null)
                {
                    if (result.FileName.Contains(".xls") || result.FileName.Contains(".xlsx"))
                        await InsertExcelTable(result);
                    if (options.FileTypes == FilePickerFileType.Images)
                        await InsertImage(result);
                }
                else
                {
                    Text = $"Pick cancelled.";
                }

                return result;
            }
            catch (Exception ex)
            {
                Text = ex.ToString();
                IsImageVisible = false;
                return null;
            }
        }

        async Task InsertImage(FileResult result)
        {
            var stream = await result.OpenReadAsync();
            var logoimage = "<img width=100 src=" + "\\'" + "data:image/png;base64," + ImageToBase64(stream) + "\\'><p><br></p>";
            // webView.Eval(string.Format("printMultiplicationTable({0}, {1})", number, end)
            var uhubehu = $"addHTML(" + "'" + logoimage + "')";
            EvalHTMLJavascript = uhubehu;
            //var result1 = await EvaluateJavascript("printMultiplicationTable()");
        }

        async void ExportToRTF()
        {
            var result1 = await EvaluateJavascript("document.body.innerHTML");
        }


        #region Helper

        private void CreateTable()
        {
            int row = 6;
            int column = 5;
            var tablestring = GetTable(0, row, 0, column, null);
            var tt = $"addHTML(" + "'" + tablestring + "')"; ;
            EvalHTMLJavascript = tt;
        }
        async Task<bool> AppendImage(FileResult result)
        {
            var stream = await result.OpenReadAsync();
            var logoimage = "<img width=100 src=\'data:image/png;base64," + ImageToBase64(stream) + "\'><p><br></p>";
            HTMLAppend(logoimage);
            return true;
        }
        public string ImageToBase64(Stream sourceStream)
        {
            var bytes = getbytes(sourceStream);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        byte[] getbytes(Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public void HTMLAppend(string htmlstr)
        {
            RTFContent = RTFContent.Replace(EndHTML, "");
            RTFContent += htmlstr;
            RTFContent += EndHTML;
        }

        async Task<bool> InsertExcelTable(FileResult result)
        {
            var stream = await result.OpenReadAsync();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                var workbook = excelEngine.Excel.Workbooks.Open(stream);
                var worksheet = workbook.Worksheets[0];
                var result1 = worksheet.Cells;
                var minrow = result1.Min(X => X.Row);
                var maxrow = result1.Max(X => X.Row);
                var mincol = result1.Min(X => X.Column);
                var maxcol = result1.Max(X => X.Column);
                string tablestring = GetTable(minrow, maxrow, mincol, maxcol, result1);
                var tablehtml = $"addHTML(" + "'" + tablestring + "')"; ;
                EvalHTMLJavascript = tablehtml;
                //htmlcontent += "<table>";
                //for (int i = minrow; i <= maxrow; i++)
                //{
                //    htmlcontent += "<tr>";
                //    for (int j = mincol; j <= maxcol; j++)
                //    {
                //        var range = result1.FirstOrDefault(y => y.Row == i && y.Column == j);
                //        htmlcontent += $"<td contenteditable>{range.DisplayText}</td>";
                //    }
                //    htmlcontent += "</tr>";
                //}
                //htmlcontent += "</table>";
                //htmlcontent += "</body></html>";
                //Text += worksheet.Name;
                //var listobj = worksheet.ListObjects[0];
                //RTFContent = htmlcontent;
            }
            return true;
        }

        public string GetTable(int minrow, int maxrow, int mincol, int maxcol, IRange[] result1)
        {
            var htmlcontent = "<table>";
            for (int i = minrow; i <= maxrow; i++)
            {
                htmlcontent += "<tr>";
                for (int j = mincol; j <= maxcol; j++)
                {
                    string tdstring = "";
                    if (result1 != null)
                    {
                        var range = result1.FirstOrDefault(y => y.Row == i && y.Column == j);
                        tdstring = range.DisplayText;
                    }
                    htmlcontent += $"<td contenteditable>{tdstring}<br></div></td>";
                }
                htmlcontent += "</tr>";
            }
            htmlcontent += "</table><p><br></p>";
            return htmlcontent;
        }

        string gethtmlcontent()
        {
            return @"<html><body><button onclick=""myFunction()"">Click me</button><p id=""demo""></p><p>A function is triggered when the button is clicked. The function outputs some text in a p element with id=""demo"".</p><script>function myFunction() {document.getElementById(""demo"").innerHTML = ""Hello World"";}</script><table><tr><td><div contenteditable>I'm editable</div></td><td><div contenteditable>I'm also editable</div></td></tr><tr><td>I'm not editable</td></tr></table><style>table, th, td {border: 1px solid black;border-collapse: collapse;}</style>";

        }
        #endregion
    }
}
