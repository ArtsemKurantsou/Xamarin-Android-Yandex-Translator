
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;

namespace Yandex_Translator
{
    [Activity(Label = "@string/dictionary")]
    public class FavoritsList : Activity
    {
        RVAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FavoritesLayout);

            adapter = new RVAdapter();
            RecyclerView rvFavoritesList = FindViewById<RecyclerView>(Resource.Id.rvFavorites);
            rvFavoritesList.SetAdapter(adapter);

            rvFavoritesList.SetLayoutManager(new LinearLayoutManager(this));
            
            

        }
    }
}