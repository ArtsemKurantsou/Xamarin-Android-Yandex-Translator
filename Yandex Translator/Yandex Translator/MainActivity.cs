using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Realms;
using System.Linq;
using Yandex_Translator.Db;
using Yandex_Translator.Models;

[assembly: Application(Theme = "@android:style/Theme.Material.Light")]
namespace Yandex_Translator
{
    [Activity(Label = "@string/translator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnClickListener
    {

        private const string TAG = "MainActivity";
        private const string KEY_LANG = "lang";
        private const string KEY_TEXT_CHANGED = "text_changed";


        private EditText etFrom;
        private TextView tvToText;
        private TextView tvFrom;
        private TextView tvTo;
        private Realm realm;

        Button btnTranslate;
        Button btnAddToFav;
        Button btnSwap;


        private bool isRuToEng = true;
        private bool isTextChanged = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            realm = DbManager.GetRealm();

            if (bundle != null)
            {
                isRuToEng = bundle.GetBoolean(KEY_LANG, true);
                isTextChanged = bundle.GetBoolean(KEY_TEXT_CHANGED, false);
            }

            btnTranslate = FindViewById<Button>(Resource.Id.btnTranslate);
            btnSwap = FindViewById<Button>(Resource.Id.btnSwapLngs);
            btnAddToFav = FindViewById<Button>(Resource.Id.btnAddToFav);
            Button btnShow = FindViewById<Button>(Resource.Id.btnShowFavs);
            etFrom = FindViewById<EditText>(Resource.Id.etTextFrom);
            tvToText = FindViewById<TextView>(Resource.Id.tvTextTo);
            tvFrom = FindViewById<TextView>(Resource.Id.tvLngFrom);
            tvTo = FindViewById<TextView>(Resource.Id.tvLngTo);

            etFrom.TextChanged += (o, t) => isTextChanged = true;
            

            btnTranslate.SetOnClickListener(this);
            btnSwap.SetOnClickListener(this);
            btnAddToFav.SetOnClickListener(this);
            btnShow.SetOnClickListener(this);

            InitTexts();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean(KEY_LANG, isRuToEng);
            outState.PutBoolean(KEY_TEXT_CHANGED, isTextChanged);
        }

        private void InitTexts()
        {
            tvToText.Text = null;
            etFrom.Text = null;
            if (isRuToEng)
            {
                etFrom.SetHint(Resource.String.from_hint_ru);
                tvToText.SetHint(Resource.String.to_hint_eng);
                tvFrom.SetText(Resource.String.from_text_ru);
                tvTo.SetText(Resource.String.to_text_eng);
            }
            else
            {
                etFrom.SetHint(Resource.String.from_hint_eng);
                tvToText.SetHint(Resource.String.to_hint_ru);
                tvFrom.SetText(Resource.String.from_text_eng);
                tvTo.SetText(Resource.String.to_text_ru);
            }
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btnTranslate:
                    if (string.IsNullOrEmpty(etFrom.Text)) return;
                    TranslateTask translateTask = new TranslateTask(this);
                    translateTask.Execute(new TranslateParams() { TextToTranslate = etFrom.Text, IsRuToEng = isRuToEng });
                    break;
                case Resource.Id.btnSwapLngs:
                    isRuToEng = !isRuToEng;
                    InitTexts();
                    break;
                case Resource.Id.btnAddToFav:
                    if (isTextChanged)
                        Toast.MakeText(this, Resource.String.need_to_translate, ToastLength.Long).Show();
                    else realm.Write(() => {
                        Translation translation;
                        if (isRuToEng)
                            translation = realm.All<Translation>().Where(t => t.RuText.Equals(etFrom.Text.ToLower().Trim())).FirstOrDefault();
                        else
                            translation = realm.All<Translation>().Where(t => t.EngText.Equals(etFrom.Text.ToLower().Trim())).FirstOrDefault();
                        if (translation != null)
                        {
                            translation.IsInFavorits = true;
                            Toast.MakeText(this, Resource.String.traslation_added, ToastLength.Long).Show();
                        }
                        realm.Refresh();
                    });
                    break;
                case Resource.Id.btnShowFavs:
                    Intent intent = new Intent(this, typeof(FavoritsList));
                    StartActivity(intent);
                    break;
            }
        }

        public void SetTranslating(bool isTranslating)
        {
            btnTranslate.Enabled = btnAddToFav.Enabled = btnSwap.Enabled = !isTranslating;
        }
        public void SetTranslatedText(string translatesText)
        {
            tvToText.Text = translatesText;
            isTextChanged = false;
        }

        class TranslateTask : AsyncTask<TranslateParams, bool, string>
        {
            private MainActivity activity;

            public TranslateTask(MainActivity activity)
            {
                this.activity = activity;
            }

            protected override string RunInBackground(params TranslateParams[] translateParams)
            {
                string translateResult = null;
                if (translateParams == null || translateParams.Length < 1) return translateResult;
                TranslateParams tparam = translateParams[0];
                tparam.TextToTranslate = tparam.TextToTranslate.ToLower().Trim();

                if (string.IsNullOrEmpty(tparam.TextToTranslate)) return translateResult;

                Realm realm = DbManager.GetRealm();
                bool isFound = false;
                if (tparam.IsRuToEng) {
                    Translation translation = realm.All<Translation>().Where(t => t.RuText.Equals(tparam.TextToTranslate)).FirstOrDefault();
                    if (translation != null)
                    {
                        translateResult = translation.EngText;
                        isFound = true;
                    }
                }
                else
                {
                    Translation translation = realm.All<Translation>().Where(t => t.EngText.Equals(tparam.TextToTranslate)).FirstOrDefault();
                    if (translation != null)
                    {
                        translateResult = translation.RuText;
                        isFound = true;
                    }
                }
                if (!isFound)
                {
                    translateResult =  Translator.Translate(tparam.TextToTranslate, tparam.IsRuToEng);
                    Translation translation = new Translation();
                    translation.EngText = (tparam.IsRuToEng ? translateResult : tparam.TextToTranslate);
                    translation.RuText = (tparam.IsRuToEng ? tparam.TextToTranslate : translateResult);
                    realm.Write(() =>  realm.Add<Translation>(translation));
                }
                return translateResult;
            }
            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                activity.SetTranslating(true);
            }
            protected override void OnPostExecute(string result)
            {
                base.OnPostExecute(result);
                activity.SetTranslatedText(result);
                activity.SetTranslating(false);
            }
        }
    }
}

