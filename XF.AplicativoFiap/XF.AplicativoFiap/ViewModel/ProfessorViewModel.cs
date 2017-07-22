using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.AplicativoFiap.Model;

namespace XF.AplicativoFiap.ViewModel
{
    public class ProfessorViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Professor> Professores { get; set; } = new ObservableCollection<Professor>();
        public List<Professor> ListProfessores = new List<Professor>();

        private Professor selecionado;        
        public Professor Selecionado
        {
            get { return selecionado; }
            set
            {
                selecionado = value as Professor;
                EventPropertyChanged();
            }
        }

        public ICommand OnDeleteProfessorCMD { get; private set; }
        public ICommand OnAdicionarProfessorCMD { get; private set; }
        public ICommand OnNovoCMD { get; private set; }
        public ICommand OnEditProfessorCMD { get; private set; }

        public ProfessorViewModel()
        {
            OnDeleteProfessorCMD = new Command<Professor>(OnDelete);
            OnAdicionarProfessorCMD = new Command<Professor>(OnAdicionar);
            OnNovoCMD = new Command(OnNovo);
            OnEditProfessorCMD = new Command<Professor>(OnEdit);
            Carregar();
        }

        private async void Carregar()
        {
            //ListProfessores = await ProfessorRepository.GetProfessoresSqlAzureAsync();


            await ProfessorRepository.GetProfessoresSqlAzureAsync().ContinueWith(t =>
            {
                ListProfessores = t.Result.ToList();
            });

            AplicaFiltro();
        }

        void OnDelete(Professor professor)
        {
            
        }

        private async void OnAdicionar(Professor professor)
        {
            if(await ProfessorRepository.PostProfessorSqlAzureAsync(professor))
                await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void OnNovo()
        {
            App.ProfessorVM.Selecionado = new Professor();
            await Application.Current.MainPage.Navigation.PushAsync
                (new View.Professor.NovoProfessorView { BindingContext = App.ProfessorVM });
        }

        private async void OnEdit(Professor professor)
        {
            App.ProfessorVM.Selecionado = professor;

            await Application.Current.MainPage.Navigation.PushAsync
                (new View.Professor.NovoProfessorView { BindingContext = App.ProfessorVM });
        }

        private void AplicaFiltro()
        {
            for (int i = 0; i < ListProfessores.Count; i++)
            {
                Professor professor = ListProfessores[i];
                Professores.Insert(i, professor);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
