using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ESAPIX.Common.Args;
using ESAPIX.Interfaces;
using System.Threading.Tasks;

namespace ESAPX_StarterUI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IESAPIService _es;

        [ObservableProperty]
        private string title = "ESAPIX Starter Application";

        [ObservableProperty]
        private string selectedPlanId;

        public MainViewModel(IESAPIService es)
        {
            _es = es;
            //Fire method when plan setup is changed
            _es.Execute(sac => sac.PlanSetupChanged += Sac_PlanSetupChanged);
        }

        private void Sac_PlanSetupChanged(VMS.TPS.Common.Model.API.PlanSetup ps)
        {
            //When working with multithread you have to convert types to plan types (non-VMS classes)
            //Never touch a VMS object on UI thread (or it will crash
            SelectedPlanId = _es.GetValue(sac => sac.PlanSetup?.Id);
        }

        [RelayCommand]
        private Task SaveContext()
        {
            return _es.ExecuteAsync(sac =>
            {
                ContextIO.SaveToFile(sac);
            });
        }
    }
}
