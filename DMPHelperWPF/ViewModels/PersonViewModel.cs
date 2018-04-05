using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGenerator.NPC;

namespace DMPHelperWPF.ViewModels
{
    public class PersonViewModel : NotifyChangedBase
    {
        private PersonData npc;

        public PersonViewModel()
        {
            npc = new PersonData();
        }

        public PersonData RawData { get => npc; }

        public PersonViewModel(PersonData data)
        {
            npc = data;
            OnPropertyChanged(Name);
            OnPropertyChanged(Demographic);
            OnPropertyChanged(Personality);
            OnPropertyChanged(Religion);
            OnPropertyChanged(Profession);
        }

        public string Name { get => npc.Name ?? ""; }
        
        public string Culture
        {
            get
            {
                if (npc.Nation == null || npc.Culture == null) return "";
                return $"{npc.Nation} ({npc.Culture})";
            }
            
        }

        public string Demographic
        {
            get
            {
                if (npc.Age == null || npc.Gender == null || npc.Race == null) return "";
                if (String.IsNullOrEmpty(npc.Subrace))
                {
                    return $"{npc.Age} {npc.Gender} {npc.Race}";
                }
                else
                {
                    return $"{npc.Age} {npc.Gender} {npc.Race} ({npc.Subrace})";
                }
            }
        }

        public string Personality { get => npc.Personality ?? ""; }
        public string Religion { get => npc.Religion ?? ""; }
        public string Profession { get => npc.Profession ?? ""; }
    }
}
