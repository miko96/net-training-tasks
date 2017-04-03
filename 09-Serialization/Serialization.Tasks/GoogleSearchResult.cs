
using System.Runtime.Serialization;

namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt



    [DataContract]
    public class GoogleSearchResult
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "url")]
        public Url Url { get; set; }
        [DataMember(Name = "queries")]
        public Queries Queries { get; set; }
        [DataMember(Name = "context")]
        public Context Context { get; set; }
        [DataMember(Name = "items")]
        public Item[] Items { get; set; }
    }

    [DataContract]
    public class Url
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "template")]
        public string Template { get; set; }
    }

    [DataContract]
    public class Queries
    {
        [DataMember(Name = "nextPage")]
        public Nextpage[] NextPage { get; set; }

        [DataMember(Name = "previousPage")]
        public PreviousPage[] PreviousPage { get; set; }

        [DataMember(Name = "request")]
        public Request[] Request { get; set; }
    }

    [DataContract]
    public class Nextpage
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "totalResults")]
        public long TotalResults { get; set; }

        [DataMember(Name = "searchTerms")]
        public string SearchTerms { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }

        [DataMember(Name = "inputEncoding")]
        public string InputEncoding { get; set; }

        [DataMember(Name = "outputEncoding")]
        public string OutputEncoding { get; set; }

        [DataMember(Name = "cx")]
        public string Cx { get; set; }
    }

    [DataContract]
    public class PreviousPage
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "totalResults")]
        public long TotalResults { get; set; }

        [DataMember(Name = "searchTerms")]
        public string SearchTerms { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }

        [DataMember(Name = "inputEncoding")]
        public string InputEncoding { get; set; }

        [DataMember(Name = "outputEncoding")]
        public string OutputEncoding { get; set; }

        [DataMember(Name = "cx")]
        public string Cx { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "totalResults")]
        public long TotalResults { get; set; }

        [DataMember(Name = "searchTerms")]
        public string SearchTerms { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }

        [DataMember(Name = "inputEncoding")]
        public string InputEncoding { get; set; }

        [DataMember(Name = "outputEncoding")]
        public string OutputEncoding { get; set; }

        [DataMember(Name = "cx")]
        public string Cx { get; set; }
    }

    [DataContract(Name = "context")]
    public class Context
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "htmlTitle")]
        public string HtmlTitle { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "displayLink")]
        public string DisplayLink { get; set; }

        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }

        [DataMember(Name = "htmlSnippet")]
        public string HtmlSnippet { get; set; }

        [DataMember(Name = "pagemap")]
        public Pagemap Pagemap { get; set; }
    }

    [DataContract(Name = "pagemap")]
    public class Pagemap
    {
        [DataMember]
        public RTO[] RTO { get; set; }
    }

    [DataContract]
    public class RTO
    {
        public string format { get; set; }
        public string group_impression_tag { get; set; }
        public string Optmax_rank_top { get; set; }
        public string Optthreshold_override { get; set; }
        public string Optdisallow_same_domain { get; set; }
        public string Outputtitle { get; set; }
        public string Outputwant_title_on_right { get; set; }
        public string Outputnum_lines1 { get; set; }
        public string Outputtext1 { get; set; }
        public string Outputgray1b { get; set; }
        public string Outputno_clip1b { get; set; }
        public string UrlOutputurl2 { get; set; }
        public string Outputlink2 { get; set; }
        public string Outputtext2b { get; set; }
        public string UrlOutputurl2c { get; set; }
        public string Outputlink2c { get; set; }
        public string result_group_header { get; set; }
        public string Outputimage_url { get; set; }
        public string image_size { get; set; }
        public string Outputinline_image_width { get; set; }
        public string Outputinline_image_height { get; set; }
        public string Outputimage_border { get; set; }
    }

}