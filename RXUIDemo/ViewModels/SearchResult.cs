using System;
using System.Json;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using ReactiveUI;

namespace RXUIDemo.ViewModels
{
	public class SearchResult : ReactiveObject
	{
		public string Title { get; protected set; }

		public string Description { get; protected set; }

		public SearchResult (string title, string description)
		{
			Title = title;
			Description = description;
		}
	}

	public class BingSearch : ReactiveObject
	{
		const string API_KEY = "+SnJvxO1rQNUjbw2SqPcxQBIJ5MsWh0YXCtS8gbP0L8=";

		string query;

		public string Query {
			get { return query; }
			set { this.RaiseAndSetIfChanged (ref query, value); }
		}

		ReactiveList<SearchResult> resultList;

		public ReactiveList<SearchResult> ResultList {
			get { 
				return resultList;
			}
			set { this.RaiseAndSetIfChanged (ref resultList, value); }
		}

		public ReactiveCommand<SearchResult> Search { get; set; }

		public BingSearch ()
		{
			ResultList = new ReactiveList<SearchResult> ();

			var canSearch = this.WhenAny (x => x.Query, y => !String.IsNullOrWhiteSpace (y.Value));

			Search = ReactiveCommand.CreateAsyncObservable(
				canSearch,
				query => {
					ResultList.Clear ();

					var results = GetSearchResults ((string)query);
	
					return results;
				}
			);
				
			Search.Subscribe (result => ResultList.Add (result));

			this.WhenAnyValue (x => x.Query)
				.Throttle (TimeSpan.FromMilliseconds (500), RxApp.MainThreadScheduler)
				.InvokeCommand (this, x => x.Search);
		}

		IObservable<SearchResult> GetSearchResults (string query)
		{
			Console.WriteLine ("Searching Bing for: " + query);
		
			var bingSearch = String.Format ("https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1/Web?Query=%27{0}%27&$top=10&$format=Json", query);
		
			var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (bingSearch));
		
			httpReq.Credentials = new NetworkCredential (API_KEY, API_KEY);
		
			using (var httpRes = (HttpWebResponse)httpReq.GetResponse ()) {
				var response = httpRes.GetResponseStream ();
				var json = (JsonObject)JsonObject.Load (response);
		
				var results = (from result in (JsonArray)json ["d"] ["results"]
				                 let jResult = result as JsonObject
				                 select new SearchResult (jResult ["Title"], jResult ["Description"])).ToObservable ();
				return results;
			}
		}

	}
}