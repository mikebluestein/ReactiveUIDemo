using System;
using System.Reactive.Linq;
using Foundation;
using ReactiveUI;
using UIKit;

namespace RXUIDemo
{
	public partial class SearchResultView : ReactiveTableViewCell, IViewFor<RXUIDemo.ViewModels.SearchResult>
    {
        public static readonly UINib Nib = UINib.FromName("SearchResultView", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("SearchResultView");

        public SearchResultView(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib ();

            this.WhenAny (x => x.ViewModel, x => x.Value)
                .Where (x => x != null)
                .Subscribe (x => { 
                	title.Text = x.Title; 
                	description.Text = x.Description;
            	});
        }

		RXUIDemo.ViewModels.SearchResult viewModel;

		public RXUIDemo.ViewModels.SearchResult ViewModel {
            get { return viewModel; }
            set { this.RaiseAndSetIfChanged(ref viewModel, value); }
        }

        object IViewFor.ViewModel {
            get { return viewModel; }
			set { ViewModel = (RXUIDemo.ViewModels.SearchResult)value; }
        }
    }
}