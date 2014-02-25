using System;
using System.Drawing;

using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ReactiveUI.Cocoa;
using ReactiveUI;

namespace RXUIDemo
{
    public partial class SearchViewController : ReactiveTableViewController, IViewFor<SearchResultsViewModel>
    {
        UITextField searchTextField;
        UIActivityIndicatorView activityView;

        public SearchViewController()
        {
        }
       
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = new SearchResultsViewModel();

            TableView.RegisterNibForCellReuse(SearchResultView.Nib, SearchResultView.Key);
            TableView.Source = new ReactiveTableViewSource(TableView, ViewModel.ReactiveData, SearchResultView.Key, 100.0f, cell =>
            {
                Console.WriteLine(cell);
            });

            searchTextField = new UITextField(new RectangleF(0, 0, 320, 50));
            searchTextField.BackgroundColor = UIColor.FromRGB (164, 196, 219);
            searchTextField.Placeholder = "<Enter a search string>";
            searchTextField.TextColor = UIColor.Black;
            TableView.TableHeaderView = searchTextField;

            this.Bind(ViewModel, vm => vm.Query, vc => vc.searchTextField.Text);

            InitActivityIndicator();

            this.TableView.ContentInset = new UIEdgeInsets (20, 0, 0, 0);
        }

        void InitActivityIndicator()
        {
            activityView = new UIActivityIndicatorView();
            activityView.Frame = new RectangleF(0, 0, 50, 50);
            activityView.Center = TableView.Center;
            activityView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            TableView.AddSubview(activityView);

            ViewModel.SomeCommand.IsExecuting.Subscribe(b =>
            {
                if (b)
                    activityView.StartAnimating();
                else
                    activityView.StopAnimating();
            });
        }

        SearchResultsViewModel viewModel;

        public SearchResultsViewModel ViewModel
        {
            get { return viewModel; }
            set { this.RaiseAndSetIfChanged(ref viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return viewModel; }
            set { ViewModel = (SearchResultsViewModel)value; }
        }
    }
}