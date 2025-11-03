using System.Text.RegularExpressions;
using System.Xml.Linq;
using MicrosoftExtensionsAiSample.Models;

namespace MicrosoftExtensionsAiSample.Services;

public static class BlogRetrievalService
{
    public static async Task<List<BlogPost>> GetAllBlogPostsAsync()
    {
        var blogPosts = new List<BlogPost>();
        
        // Define the base URL of the RSS feed
        string baseUrl = "https://trailheadtechnology.com/feed/?paged=";

        // Initialize HttpClient to make HTTP requests
        using (HttpClient client = new HttpClient())
        {
            int pageNumber = 1;
            bool isEmptyPage = false;

            while (!isEmptyPage)
            {
                // Construct the URL for the current page
                string url = baseUrl + pageNumber;

                try
                {
                    Console.WriteLine($"📡 Fetching page {pageNumber}...");
                    
                    // Fetch the RSS feed content for the current page
                    HttpResponseMessage response = await client.GetAsync(url);

                    // If we get a 404 (Not Found), break the loop
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Page {pageNumber} not found (404). Finished fetching.");
                        break;
                    }

                    // Ensure the request was successful (status code 2xx)
                    response.EnsureSuccessStatusCode();

                    // Read the content as a string
                    string feedContent = await response.Content.ReadAsStringAsync();

                    // Clean the XML content by removing invalid characters
                    feedContent = CleanInvalidXmlCharacters(feedContent);

                    // Parse the cleaned RSS feed content using XDocument
                    XDocument feedXml = XDocument.Parse(feedContent);

                    // Select all <item> elements in the RSS feed
                    var items = feedXml.Descendants("item").ToList();

                    // If there are no <item> elements, the page is empty, so stop
                    if (items.Count == 0)
                    {
                        isEmptyPage = true;
                    }
                    else
                    {
                        // Loop through each <item> and extract the title and other information
                        foreach (var item in items)
                        {
                            // Extract title, content, category, tags, and URL from each <item>
                            string title = item.Element("title")?.Value ?? "No Title";
                            
                            // Try to get full content first, fallback to description if not available
                            string content = item.Element(XName.Get("encoded", "http://purl.org/rss/1.0/modules/content/"))?.Value 
                                          ?? item.Element("description")?.Value 
                                          ?? "No Content";
                            
                            string urlItem = item.Element("link")?.Value ?? "";
                            var categories = item.Elements("category").Select(c => c.Value).ToList();

                            var blogPost = new BlogPost
                            {
                                Title = title,
                                Content = content,
                                Url = urlItem,
                                Categories = categories
                            };
                            
                            blogPost.GenerateCombinedText();
                            blogPosts.Add(blogPost);

                            Console.WriteLine($"  📝 {title}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network errors, XML parsing errors)
                    Console.WriteLine($"❌ Error fetching or parsing page {pageNumber}: {ex.Message}");
                    break;
                }

                // Increment the page number for the next loop
                pageNumber++;
            }

            Console.WriteLine($"✅ Retrieved {blogPosts.Count} blog posts total.");
        }
        
        return blogPosts;
    }

    public static async Task FetchRssFeed()
    {
        // Define the base URL of the RSS feed
        string baseUrl = "https://trailheadtechnology.com/feed/?paged=";

        // Initialize HttpClient to make HTTP requests
        using (HttpClient client = new HttpClient())
        {
            int pageNumber = 1;
            bool isEmptyPage = false;

            while (!isEmptyPage)
            {
                // Construct the URL for the current page
                string url = baseUrl + pageNumber;

                try
                {
                    // Fetch the RSS feed content for the current page
                    HttpResponseMessage response = await client.GetAsync(url);

                    // If we get a 404 (Not Found), break the loop
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Page {pageNumber} not found (404). Exiting...");
                        break;
                    }

                    // Ensure the request was successful (status code 2xx)
                    response.EnsureSuccessStatusCode();

                    // Read the content as a string
                    string feedContent = await response.Content.ReadAsStringAsync();

                    // Clean the XML content by removing invalid characters
                    feedContent = CleanInvalidXmlCharacters(feedContent);

                    // Parse the cleaned RSS feed content using XDocument
                    XDocument feedXml = XDocument.Parse(feedContent);

                    // Select all <item> elements in the RSS feed
                    var items = feedXml.Descendants("item").ToList();

                    // If there are no <item> elements, the page is empty, so stop
                    if (items.Count == 0)
                    {
                        isEmptyPage = true;
                    }
                    else
                    {
                        // Loop through each <item> and extract the title and other information
                        foreach (var item in items)
                        {
                            // Extract title, content, category, tags, and URL from each <item>
                            string title = item.Element("title")?.Value;
                            
                            // Try to get full content first, fallback to description if not available
                            string content = item.Element(XName.Get("encoded", "http://purl.org/rss/1.0/modules/content/"))?.Value 
                                          ?? item.Element("description")?.Value;
                            
                            string urlItem = item.Element("link")?.Value;
                            var categories = item.Elements("category").Select(c => c.Value).ToList();

                            // Write the title to the console
                            Console.WriteLine($"Title: {title} ({urlItem})");

                            // You can also write other data to the console if needed
                            // Console.WriteLine($"Content: {content}");
                            // Console.WriteLine($"URL: {urlItem}");
                            // Console.WriteLine($"Categories: {string.Join(", ", categories)}");
                            // Console.WriteLine($"Tags: {string.Join(", ", tags)}");
                            // Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network errors, XML parsing errors)
                    Console.WriteLine($"Error fetching or parsing page {pageNumber}: {ex.Message}");
                    break;
                }

                // Increment the page number for the next loop
                pageNumber++;
            }

            // End of program
            Console.WriteLine("End of RSS feed.");
        }
    }

    // Method to inspect RSS feed structure (for debugging)
    public static async Task InspectRssFeedStructure()
    {
        string url = "https://trailheadtechnology.com/feed/?paged=1";
        
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Console.WriteLine($"🔍 Inspecting RSS feed structure from: {url}");
                
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                string feedContent = await response.Content.ReadAsStringAsync();
                feedContent = CleanInvalidXmlCharacters(feedContent);
                
                XDocument feedXml = XDocument.Parse(feedContent);
                var firstItem = feedXml.Descendants("item").FirstOrDefault();
                
                if (firstItem != null)
                {
                    Console.WriteLine("📋 Available elements in first RSS item:");
                    foreach (var element in firstItem.Elements())
                    {
                        var content = element.Value;
                        var preview = content.Length > 100 ? content.Substring(0, 100) + "..." : content;
                        Console.WriteLine($"  - {element.Name}: {preview}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inspecting RSS feed: {ex.Message}");
            }
        }
    }

    // Method to clean up invalid XML characters
    private static string CleanInvalidXmlCharacters(string xmlContent)
    {
        // Replace invalid characters (specifically 0x1E) with an empty string
        string cleanedXml = Regex.Replace(xmlContent, @"\x1E", "");

        return cleanedXml;
    }
}