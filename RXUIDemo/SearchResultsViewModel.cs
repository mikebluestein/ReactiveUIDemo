using System;
using ReactiveUI;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Json;
using System.Net;
using System.Linq;

namespace RXUIDemo
{
    public class SearchResultsViewModel : ReactiveObject
    {
        readonly ReactiveCommand searchCommand = new ReactiveCommand();

        public ReactiveCommand SomeCommand { get { return searchCommand; } }

        string query;

        public string Query
        {
            get { return query; }
            set { this.RaiseAndSetIfChanged(ref query, value); }
        }

        public ReactiveList<SearchResultViewModel> ReactiveData { get; protected set; }

        public SearchResultsViewModel()
        {
            ReactiveData = new ReactiveList<SearchResultViewModel>();

            var results = searchCommand.RegisterAsyncFunction(query => GetSearchResults((string)query));

            this.ObservableForProperty<SearchResultsViewModel, string>("Query")
                .Throttle(new TimeSpan(700))
                .Select(x => x.Value).DistinctUntilChanged()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Subscribe(searchCommand.Execute);

            results.Subscribe(list => {
                ReactiveData.Clear();
                ReactiveData.AddRange(list); 
            });
        }
            
        public static List<SearchResultViewModel> GetSearchResults(string query)
        {
            Console.WriteLine("searching for " + query);

//            return new List<SearchResultViewModel> { 
//                new SearchResultViewModel("test 1", DateTime.Now.Second.ToString()),
//                new SearchResultViewModel("test 2", DateTime.Now.Second.ToString()),
//                new SearchResultViewModel("test 3", DateTime.Now.Second.ToString()),
//                new SearchResultViewModel("test 4", DateTime.Now.Second.ToString())
//            };

            return Search(query);
        }

        static List<SearchResultViewModel> Search(string text)
        {
            List<SearchResultViewModel> res;

            string bingSearch = String.Format (
                "https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1/Web?Query=%27{0}%27&$top=10&$format=Json", text);

            var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (bingSearch));

            string api_key = "YOUR API KEY HERE";

            httpReq.Credentials = new NetworkCredential (api_key, api_key);

            using (var httpRes = (HttpWebResponse)httpReq.GetResponse())
            {
                var response = httpRes.GetResponseStream();
                var json = (JsonObject)JsonObject.Load(response);

                var results = (from result in (JsonArray)json["d"]["results"]
                               let jResult = result as JsonObject
                               select new SearchResultViewModel(jResult["Title"], jResult["Description"]));
                res = results.ToList();
            }
            return res;
        }
    }

    public class SearchResultViewModel : ReactiveObject
    {
        public string Title { get; protected set; }

        public string Description { get; protected set; }

        public SearchResultViewModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}