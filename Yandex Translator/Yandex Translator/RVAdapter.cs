using Android.Support.V7.Widget;
using Android.Views;
using Realms;
using System.Collections.Generic;
using System.Linq;
using Yandex_Translator.Db;
using Yandex_Translator.Models;

namespace Yandex_Translator
{
    class RVAdapter : RecyclerView.Adapter
    {
        List<Translation> translations;

        public RVAdapter()
        {
            FillList(); 
        }

        public override int ItemCount => translations.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            FavoriteVH viewHolder = holder as FavoriteVH;
            if (viewHolder == null) return;
            Translation translation = translations[position];
            viewHolder.RuText = translation.RuText;
            viewHolder.EngText = translation.EngText;
        }
        private void FillList()
        {
            Realm realm = DbManager.GetRealm();
            translations = (from t in realm.All<Translation>() where t.IsInFavorits select t).ToList();
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_favorite, parent, false);

            return new FavoriteVH(v);
        }
    }
}