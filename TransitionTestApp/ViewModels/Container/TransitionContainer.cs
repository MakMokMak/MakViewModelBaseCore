using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels.Container
{
    public class TransitionContainer : TransitionContainerBase
    {
        public TransitionContainer(string key, TransitionViewModelBase viewModel)
            : base(key, viewModel)
        {
        }

        public string Transition1Text { get; set; }
        public string Transition2Text { get; set; }
        public string Transition3Text { get; set; }
    }
}
