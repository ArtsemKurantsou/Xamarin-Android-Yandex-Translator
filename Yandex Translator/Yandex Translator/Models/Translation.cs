using Realms;

namespace Yandex_Translator.Models
{
    class Translation : RealmObject
    {

        public Translation()
        {
            IsInFavorits = false;
        }
        public Translation(string engText, string ruText)
        {
            EngText = engText;
            RuText = ruText;
            IsInFavorits = false;
        }
        public Translation(string engText, string ruText, bool isInFavarits)
        {
            EngText = engText;
            RuText = ruText;
            IsInFavorits = isInFavarits;
        }
        public string EngText { get; set; }
        public string RuText { get; set; }
        public bool IsInFavorits { get; set; }
        [Ignored]
        public bool IsSelected { get; set; }
        public override string ToString()
        {
            return "Ru -> " + RuText + " Eng -> " + EngText + " isFavorite -> " + IsInFavorits;
        }
    }
}