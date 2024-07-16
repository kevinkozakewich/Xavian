using System.Windows.Input;
using Xavian.Core;

namespace Xavian.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        private object _currentView;

        public HomeViewModel _homeVM { get; set; }
        public DiscoveryViewModel _discoveryVM { get; set; }
        public FeaturedViewModel _featuredVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand FeaturedViewCommand { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                _currentView = value; 
                OnPropertyChanged(); 
            }
        }


        public MainViewModel()
        {
            _homeVM = new HomeViewModel();
            _discoveryVM = new DiscoveryViewModel();
            _featuredVM = new FeaturedViewModel();
            CurrentView = _homeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = _homeVM;
            });

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = _discoveryVM;
            });

            FeaturedViewCommand = new RelayCommand(o =>
            {
                CurrentView = _featuredVM;
            });
        }
    }
}
