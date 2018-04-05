using LibGenerator.Dungeon;

namespace DMPHelperWPF.ViewModels
{
    public class DungeonViewModel : NotifyChangedBase
    {
        private AdventureData data;
        private string hasBoss = " with a boss monster";

        public DungeonViewModel(AdventureData d)
        {
            data = d;
            OnPropertyChanged(nameof(AdventureType));
            OnPropertyChanged(nameof(Region));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(PrimaryMonster));
        }

        public AdventureData RawData { get => data; }

        public string AdventureType { get => $"Level {data.Level} {data.AdventureType} ({data.SubType})"; }
        public string Region { get => data.Region; }
        public string Size { get => $"A {data.Scale} site with {data.Size} areas located in {Region}"; }
        public string PrimaryMonster { get => $"Dominated by {data.PrimaryMonster} creatures{(data.HasBoss ? hasBoss: "")}."; }

    }
}
