using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels.Container
{
    public class SelectContainer : TransitionContainerBase
    {
        public SelectContainer(string key, TransitionViewModelBase viewModel)
            : base(key, viewModel)
        {
        }

        public string ItemName { get; set; }
    }
}
