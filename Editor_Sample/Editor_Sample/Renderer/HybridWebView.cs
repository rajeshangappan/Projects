using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Editor_Sample.Renderer
{
    public class HybridWebView : WebView
    {
        Action<string> action;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public void RegisterAction(Action<string> callback)
        {
            action = callback;
        }

        public void Cleanup()
        {
            action = null;
        }

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }

        //Trigger Javascript from xamarin
        public static BindableProperty EvaluateJavascriptProperty =
                 BindableProperty.Create(nameof(EvaluateJavascript), typeof(Func<string, Task<string>>), typeof(HybridWebView), null, BindingMode.OneWayToSource);
        public Func<string, Task<string>> EvaluateJavascript
        {
            get
            {
                return (Func<string, Task<string>>)GetValue(EvaluateJavascriptProperty);
            }
            set
            {
                SetValue(EvaluateJavascriptProperty, value);
            }
        }

        public static BindableProperty EvalHTMLJavascriptProperty =
                 BindableProperty.Create(nameof(EvalHTMLJavascript), typeof(string), typeof(HybridWebView), null, BindingMode.TwoWay);
        public string EvalHTMLJavascript
        {
            get
            {
                return (string)GetValue(EvalHTMLJavascriptProperty);
            }
            set
            {
                SetValue(EvalHTMLJavascriptProperty, value);
            }
        }
    }
}
