using ViewCommon.Utils;

namespace Daedalus.ViewModels
{
    public class InitTestViewModel : ViewModelBase
    {
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public string Name { get; set; }
        public string Amounts { get; set; }

        private string Spinner { get; set; } = "|";
        private string Funky { get; set; } = "**";
        private void _spinner()
        {
            if (Spinner == "|") Spinner = "/";
            else if (Spinner == "/") Spinner = "--";
            else if(Spinner == "--") Spinner = "\\";
            else if(Spinner == "\\") Spinner = "|";
            //else if(Spinner == "|") Spinner = "/";
            //else if(Spinner == "/") Spinner = "--";
            //else if(Spinner == "\\") Spinner = "|";
        }

        private void _funky()
        {
            if (Funky == "**") Funky = "F";
            else if (Funky == "F") Funky = "F U";
            else if (Funky == "F U") Funky = "F U N";
            else if (Funky == "F U N") Funky = "F U N K";
            else if (Funky == "F U N K") Funky = "F U N K Y";
            else if (Funky == "F U N K Y") Funky = "F U N K Y <";
            else if (Funky == "F U N K Y <") Funky = "F U N K Y < -";
            else if (Funky == "F U N K Y < -") Funky = "F U N K Y < - -";
            else if (Funky == "F U N K Y < - -") Funky = "F U N K Y < - - >";
            else if (Funky == "F U N K Y < - - >") Funky = "F U N K Y < - - > S";
            else if (Funky == "F U N K Y < - - > S") Funky = "F U N K Y < - - > S T";
            else if (Funky == "F U N K Y < - - > S T") Funky = "F U N K Y < - - > S T O";
            else if (Funky == "F U N K Y < - - > S T O") Funky = "F U N K Y < - - > S T O C";
            else if (Funky == "F U N K Y < - - > S T O C") Funky = "F U N K Y < - - > S T O C K";
            else if (Funky == "F U N K Y < - - > S T O C K") Funky = "F U N K Y < - - > S T O C K S";
            else if (Funky == "F U N K Y < - - > S T O C K S") Funky = "**";
        }

        public void UpdateCount() {
            
            Count++;
            _spinner();
            _funky();
            Amounts = $"{Funky} | | | {Count} {Spinner} {TotalCount} | | | {Funky}";
            NotifyPropertyChanged($"Count");
            NotifyPropertyChanged($"Amounts");
        }

        public void UpdateNameAndTotal(string name, int total) {
            TotalCount = total;
            Name = name;
            Count= 0;
            NotifyPropertyChanged("TotalCount");
            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("Name");
        }

    }
}
