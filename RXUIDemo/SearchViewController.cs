using System;
using CoreGraphics;
using ReactiveUI;
using RXUIDemo.ViewModels;
using UIKit;

namespace RXUIDemo
{
	public partial class SearchViewController : ReactiveTableViewController, IViewFor<BingSearch>
	{
		UITextField searchTextField;
		UIActivityIndicatorView activityView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			ViewModel = new BingSearch ();

			TableView.RegisterNibForCellReuse (SearchResultView.Nib, SearchResultView.Key);
			TableView.Source = new ReactiveTableViewSource<SearchResult> (
				TableView, 
				ViewModel.ResultList, 
				SearchResultView.Key, 
				100.0f, 
				cell => {
					Console.WriteLine (cell);
				}
			);

			searchTextField = new UITextField (new CGRect (0, 0, 320, 50));
			searchTextField.BackgroundColor = UIColor.FromRGB (164, 196, 219);
			searchTextField.Placeholder = "<Enter a search string>";
			searchTextField.TextColor = UIColor.Black;

			TableView.TableHeaderView = searchTextField;

			Add (searchTextField);
			this.Bind (ViewModel, vm => vm.Query, vc => vc.searchTextField.Text);

			InitActivityIndicator ();

			TableView.ContentInset = new UIEdgeInsets (20, 0, 0, 0);
		}

		void InitActivityIndicator ()
		{
			activityView = new UIActivityIndicatorView ();
			activityView.Frame = new CGRect (0, 0, 50, 50);
			activityView.Center = TableView.Center;
			activityView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
			TableView.AddSubview (activityView);

			ViewModel.Search.IsExecuting.Subscribe (b => {
				if (b)
					activityView.StartAnimating ();
				else
					activityView.StopAnimating ();
			});
		}

		BingSearch viewModel;

		public BingSearch ViewModel {
			get { return viewModel; }
			set { this.RaiseAndSetIfChanged (ref viewModel, value); }
		}

		object IViewFor.ViewModel {
			get { return viewModel; }
			set { ViewModel = (BingSearch)value; }
		}
	}
}