using System;

namespace Love4AnimalsApi.Services;

public static class CacheConstants
{
    public const int DefaultTtlMinutes = 5;

    public const string PostsAll = "posts:all";
    public static string PostItem(int id) => $"posts:{id}";

    public const string DonationsAll = "donations:all";
    public static string DonationItem(int id) => $"donations:{id}";

    public const string CampaignsAll = "campaigns:all";
    public static string CampaignItem(int id) => $"campaigns:{id}";
}
