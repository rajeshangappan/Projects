using Syncfusion.XForms.RichTextEditor;
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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            SfRichTextEditor editor = new SfRichTextEditor
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Text = "The rich text editor component is WYSIWYG editor that provides the best user experience to create and update the content"
            };
            //rtfstack.Children.Add(editor);
            //var browser = new WebView();
            //var htmlSource = new HtmlWebViewSource();
            //htmlSource.Html = @"<html><body><button onclick=""myFunction()"">Click me</button><p id=""demo""></p><p>A function is triggered when the button is clicked. The function outputs some text in a p element with id=""demo"".</p><script>function myFunction() {document.getElementById(""demo"").innerHTML = ""Hello World"";}</script><table><tr><td><div contenteditable>I'm editable</div></td><td><div contenteditable>I'm also editable</div></td></tr><tr><td>I'm not editable</td></tr></table><style>table, th, td {border: 1px solid black;}</style></body></html>";
            //browser.Source = htmlSource;
            //browser.HeightRequest = 500;
            //browser.WidthRequest = 500;
            //stack.Children.Add(browser);
            BindingContext = new MainPageViewModel();
        }
    }
}
