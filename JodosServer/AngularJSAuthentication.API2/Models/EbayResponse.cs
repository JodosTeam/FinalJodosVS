using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JodosServer.Models
{
    public class PrimaryCategory
    {
        public IList<string> categoryId { get; set; }
        public IList<string> categoryName { get; set; }
    }

    public class ShippingServiceCost
    {
        public string @currencyId { get; set; }
        public string __value__ { get; set; }
    }

    public class ShippingInfo
    {
        public IList<string> shippingType { get; set; }
        public IList<string> shipToLocations { get; set; }
        public IList<string> expeditedShipping { get; set; }
        public IList<string> oneDayShippingAvailable { get; set; }
        public IList<string> handlingTime { get; set; }
        public IList<ShippingServiceCost> shippingServiceCost { get; set; }
    }

    public class CurrentPrice
    {
        public string @currencyId { get; set; }
        public string __value__ { get; set; }
    }

    public class ConvertedCurrentPrice
    {
        public string @currencyId { get; set; }
        public string __value__ { get; set; }
    }

    public class SellingStatu
    {
        public IList<CurrentPrice> currentPrice { get; set; }
        public IList<ConvertedCurrentPrice> convertedCurrentPrice { get; set; }
        public IList<string> bidCount { get; set; }
        public IList<string> sellingState { get; set; }
        public IList<string> timeLeft { get; set; }
    }

    public class ListingInfo
    {
        public IList<string> bestOfferEnabled { get; set; }
        public IList<string> buyItNowAvailable { get; set; }
        public IList<DateTime> startTime { get; set; }
        public IList<DateTime> endTime { get; set; }
        public IList<string> listingType { get; set; }
        public IList<string> gift { get; set; }
    }

    public class Condition
    {
        public IList<string> conditionId { get; set; }
        public IList<string> conditionDisplayName { get; set; }
    }

    public class ProductId
    {
        public string @type { get; set; }
        public string __value__ { get; set; }
    }

    public class Item
    {
        public IList<string> itemId { get; set; }
        public IList<string> title { get; set; }
        public IList<string> globalId { get; set; }
        public IList<PrimaryCategory> primaryCategory { get; set; }
        public IList<string> galleryURL { get; set; }
        public IList<string> viewItemURL { get; set; }
        public IList<string> paymentMethod { get; set; }
        public IList<string> autoPay { get; set; }
        public IList<string> postalCode { get; set; }
        public IList<string> location { get; set; }
        public IList<string> country { get; set; }
        public IList<ShippingInfo> shippingInfo { get; set; }
        public IList<SellingStatu> sellingStatus { get; set; }
        public IList<ListingInfo> listingInfo { get; set; }
        public IList<string> returnsAccepted { get; set; }
        public IList<Condition> condition { get; set; }
        public IList<string> isMultiVariationListing { get; set; }
        public IList<string> topRatedListing { get; set; }
        public IList<string> subtitle { get; set; }
        public IList<ProductId> productId { get; set; }
        public IList<string> galleryPlusPictureURL { get; set; }
    }

    public class SearchResult
    {
        public string @count { get; set; }
        public IList<Item> item { get; set; }
    }

    public class PaginationOutput
    {
        public IList<string> pageNumber { get; set; }
        public IList<string> entriesPerPage { get; set; }
        public IList<string> totalPages { get; set; }
        public IList<string> totalEntries { get; set; }
    }

    public class FindItemsByKeywordsResponse
    {
        public IList<string> ack { get; set; }
        public IList<string> version { get; set; }
        public IList<DateTime> timestamp { get; set; }
        public IList<SearchResult> searchResult { get; set; }
        public IList<PaginationOutput> paginationOutput { get; set; }
        public IList<string> itemSearchURL { get; set; }
    }

    public class EbayResponse
    {
        public IList<FindItemsByKeywordsResponse> findItemsByKeywordsResponse { get; set; }
    }

}