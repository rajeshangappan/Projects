using Editor_Sample.Renderer;
using Editor_Sample.UWP.Renderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace Editor_Sample.UWP.Renderer
{
    public class HybridWebViewRenderer : WebViewRenderer
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.external.notify(data);}";
        HybridWebView _hybridWebView;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);


            var webView = e.NewElement as HybridWebView;
            if (webView != null)
            {
                _hybridWebView = webView;
                _hybridWebView.PropertyChanged += _hybridWebView_PropertyChanged;
                webView.EvaluateJavascript = async (js) =>
                {
                    return await Control.InvokeScriptAsync("eval", new[] { js });
                };
            }
        }

        private void _hybridWebView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EvalHTMLJavascript")
            {
                //var tt = string.Format("printMultiplicationTable({0}, {1})", 10, 20);
                //_hybridWebView.Eval(tt);
                var tftf = _hybridWebView.EvalHTMLJavascript;
                //var tgtg = string.Format("addHTML({0})", tftf);
                //var tt = "addHTML('<div>kjnjkn</div>')";
                _hybridWebView.Eval(tftf);
            }
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((HybridWebView)Element).Cleanup();
            }
            base.Dispose(disposing);
        }
    }
}
