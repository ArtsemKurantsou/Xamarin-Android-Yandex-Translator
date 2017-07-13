using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Yandex_Translator
{
    public class FavoriteVH : RecyclerView.ViewHolder
    {
        TextView tvRuText, tvEngText;

        public FavoriteVH(View itemView) : base(itemView)
        {
            tvRuText = itemView.FindViewById<TextView>(Resource.Id.tvRu);
            tvEngText = itemView.FindViewById<TextView>(Resource.Id.tvEng);
        }

        public string RuText
        {
            get { return tvRuText.Text; }
            set { tvRuText.Text = value; }
        }

        public string EngText
        {
            get { return tvEngText.Text; }
            set { tvEngText.Text = value; }
        }
    }
}