# UrlShortner

This is a simple exercise to create an Url Shorterner API developer in .Net Core 3.1 and C#.

There are three data storage options:
1) Azure Table Storage.
2) SQL Server.
3) In memory.


## Assumptions

1) No authentication and authorization.
2) No Admininstration page.
3) No caching.
4) Final URL Path (after `/`) must be 6-12 chars.
5) Don't checks if original URL is shorter than what the API returns. 
6) Returned URLs are alphanumeric.
7) No analytics.
