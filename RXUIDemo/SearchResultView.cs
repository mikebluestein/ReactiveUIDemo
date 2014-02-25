using System;
using System.Drawing;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI.Cocoa;
using ReactiveUI;

namespace RXUIDemo
{
    public partial class SearchResultView : ReactiveTableViewCell, IViewFor<SearchResultViewModel>
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

        public static SearchResultView Create()
        {
            return (SearchResultView)Nib.Instantiate(null, null)[0];
        }

        SearchResultViewModel viewModel;

        public SearchResultViewModel ViewModel {
            get { return viewModel; }
            set { this.RaiseAndSetIfChanged(ref viewModel, value); }
        }

        object IViewFor.ViewModel {
            get { return viewModel; }
            set { ViewModel = (SearchResultViewModel)value; }
        }
    }
}

