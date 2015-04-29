using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using System.Net;
using System.Json;
using RXUIDemo.ViewModels;

namespace RXUIDemo
{
    public partial class ClassicSearchViewController : UITableViewController
    {
        static NSString cellId = new NSString ("ResultCell");
        List<SearchResult> searchResults;
        UISearchBar searchBar;

        public ClassicSearchViewController ()
        {
            searchResults = new List<SearchResult> ();
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.TableView.ContentInset = new UIEdgeInsets (20, 0, 0, 0);

            TableView.Source = new TableSource (this);

            searchBar = new UISearchBar ();
            searchBar.Placeholder = "Enter Search Text";
            searchBar.SizeToFit ();
            searchBar.AutocorrectionType = UITextAutocorrectionType.No;
            searchBar.AutocapitalizationType = UITextAutocapitalizationType.None;
            searchBar.SearchButtonClicked += (sender, e) => {
                Search (searchBar.Text);
            };

            TableView.TableHeaderView = searchBar;
        }

        async void Search (string text)
        {
            string bingSearch = String.Format (
                                    "https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1/Web?Query=%27{0}%27&$top=10&$format=Json", text);

            var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (bingSearch));

            string api_key = "YOUR API KEY HERE";

            httpReq.Credentials = new NetworkCredential (api_key, api_key);

            using (var httpRes = (HttpWebResponse) await httpReq.GetResponseAsync ()) {
                var response = httpRes.GetResponseStream ();
                var json = (JsonObject)JsonObject.Load (response);

                var results = (from result in (JsonArray)json ["d"] ["results"]
                               let jResult = result as JsonObject
                               select new SearchResult (jResult ["Title"], jResult ["Description"]));
                searchResults = results.ToList ();

                TableView.ReloadData ();
            }
        }

        class TableSource : UITableViewSource
        {
            ClassicSearchViewController controller;

            public TableSource (ClassicSearchViewController controller)
            {
                this.controller = controller;
            }

            public override nint RowsInSection (UITableView tableView, nint section)
            {
                return controller.searchResults.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (cellId);

                if (cell == null)
                    cell = new UITableViewCell (
                        UITableViewCellStyle.Default,
                        cellId
                    );

                cell.TextLabel.Text = controller.searchResults [indexPath.Row].Title;

                return cell;
            }
        }
    }
}