using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;

namespace Yandex_Translator
{
    [Activity(Label = "Yandex_Translator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnClickListener
    {

        EditText textFrom;
        EditText textTo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            Button btnTranslate = FindViewById<Button>(Resource.Id.btnTranslate);

            textFrom = FindViewById<EditText>(Resource.Id.etTextFrom);
            textTo = FindViewById<EditText>(Resource.Id.etTextTo);

            btnTranslate.SetOnClickListener(this);

        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btnTranslate:
                    if (string.IsNullOrEmpty(textFrom.Text)) return;
                    textTo.Text = Translator.Translate(textFrom.Text);
                    break;
            }
        }
    }
}

