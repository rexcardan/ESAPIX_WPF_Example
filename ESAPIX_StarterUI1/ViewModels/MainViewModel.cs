using ESAPIX.Interfaces;
using Prism.Mvvm;
using System.Threading;

namespace ESAPX_StarterUI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "ESAPIX Starter Application";


        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        //Store in VM
        private IESAPIService _es;
        public MainViewModel(IESAPIService es)
        {
            _es = es;
            //Fire method when plan setup is changed
            _es.Execute(sac=> sac.PlanSetupChanged += Sac_PlanSetupChanged);
        }

        private void Sac_PlanSetupChanged(VMS.TPS.Common.Model.API.PlanSetup ps)
        {
            //When working with multithread you have to convert types to plan types (non-VMS classes)
            //Never touch a VMS object on UI thread (or it will crash
            SelectedPlanId = _es.GetValue(sac => sac.PlanSetup?.Id);
        }

        private string _selectedPlanId;
        public string SelectedPlanId
        {
            get { return _selectedPlanId; }
            set { SetProperty(ref _selectedPlanId, value); }
        }
    }
}
