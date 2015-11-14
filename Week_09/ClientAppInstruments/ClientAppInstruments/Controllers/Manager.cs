using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace ClientAppInstruments.Controllers
{
    public class Manager
    {
        // Attention - The data-handling methods work with a web service, not a database
        // Instead of accessing data through a "data context", 
        // they create an HttpClient object, and use that to send a request
        // to a web service URI

        public Manager() { }

        // Attention - This is a factory, which creates an HttpClient object
        // that's configured with the base URI, and headers (accept and authorization)

        private HttpClient CreateRequest(string acceptValue = "application/json")
        {
            var request = new HttpClient();

            // Could also fetch the base address string from the app's global configuration
            // Base URI of the web service we are interacting with
            request.BaseAddress = new Uri("http://localhost:51688/api/");

            // Accept header configuration
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptValue));

            // Attempt to get the token from session state memory
            // Info: https://msdn.microsoft.com/en-us/library/system.web.httpcontext.session(v=vs.110).aspx

            var token = HttpContext.Current.Session["token"] as string;

            if (string.IsNullOrEmpty(token)) { token = "empty"; }

            // Authorization header configuration
            request.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Bearer", token);

            return request;
        }

        // Fetch an instruments collection
        public async Task<InstrumentsLinked> GetInstrumentsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Create an HttpClient object
            // Enclose it in a "using" statement
            // Info - https://msdn.microsoft.com/en-us/library/yh598w02.aspx
            // It ensures the correct use of an object (HttpClient)
            // that must be disposed of when it has completed its work

            using (HttpClient request = CreateRequest())
            {
                // Send the request
                var response = await request.GetAsync("instruments");

                if (response.IsSuccessStatusCode)
                {
                    // Read the response data, and return it
                    return (await response.Content.ReadAsAsync<InstrumentsLinked>());
                }
                else
                {
                    // This app does NOT include any security components (it does not have ASP.NET Identity)
                    // Therefore, we must handle security ourselves

                    // If the response is HTTP 401, we will redirect to our login page

                    if ((int)response.StatusCode == 401)
                    {
                        var returnUrl = HttpContext.Current.Request.Url.PathAndQuery;
                        HttpContext.Current.Response
                            .Redirect(string.Format("/home/login?returnUrl={0}", returnUrl));
                    }

                    return null;
                }
            }
        }

        // Fetch an instrument by its identifier
        public async Task<InstrumentLinked> GetInstrumentByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpClient request = CreateRequest())
            {
                // Send the request
                var response = await request.GetAsync(string.Format("instruments/{0}", id));

                if (response.IsSuccessStatusCode)
                {
                    // Read the response data, and return it
                    return (await response.Content.ReadAsAsync<InstrumentLinked>());
                }
                else
                {
                    return null;
                }
            }
        }

        // Fetch an instrument photo by its identifier
        public async Task<HttpResponseMessage> GetInstrumentPhotoByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Notice that we're passing a content type for the Accept header
            using (var request = CreateRequest("image/*"))
            {
                var response = await request.GetAsync(string.Format("instruments/{0}", id));

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
        }

        // Fetch an instrument sound clip by its identifier
        public async Task<HttpResponseMessage> GetInstrumentSoundClipByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Notice that we're passing a content type for the Accept header
            using (var request = CreateRequest("audio/*"))
            {
                var response = await request.GetAsync(string.Format("instruments/{0}", id));

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    return null;
                }
            }
        }

        // Add a new instrument
        public async Task<InstrumentLinked> AddInstrument(InstrumentAdd newItem)
        {
            using (var request = CreateRequest())
            {
                // Attempt to add the new item
                var response = await request.PostAsJsonAsync("instruments", newItem);

                if (response.IsSuccessStatusCode)
                {
                    var addedItem = await response.Content.ReadAsAsync<InstrumentLinked>();

                    // Attempt to add the photo
                    if (newItem.PhotoUpload != null)
                    {
                        await SetPhoto(addedItem.Item.Id, newItem.PhotoUpload);
                    }

                    return addedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        // Attention - Login by requesting an access token from the Identity Server
        public async Task<bool> Login(InstrumentAppCredentials credentials)
        {
            using (var request = new HttpClient())
            {
                // Package the data
                // We do NOT save the data in a persistent store
                // The data items are just passed through this app
                var data = new Dictionary<string, string>
                {
                    {"grant_type","password" },
                    {"username",credentials.Username.Trim() },
                    {"password",credentials.Password.Trim() }
                };
                var requestBody = new FormUrlEncodedContent(data);

                // Send the request
                // The request body data type will cause the correct
                // application/x-www-form-urlencoded
                // Content-Type header to be configured on the request
                var response = await request.PostAsync("http://localhost:32474/token", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    // Read the desired data from the response
                    var token = await response.Content.ReadAsAsync<AccessToken>();

                    // Configure in-memory session state storage
                    // The data items are not stored in a persistent store
                    // However, we need to keep these values in memory
                    // during the lifetime of the user's interactive session
                    HttpContext.Current.Session["token"] = token.access_token;
                    HttpContext.Current.Session["username"] = token.userName;

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Configure an instrument object with a sound clip
        internal async Task SetSoundClip(int itemId, HttpPostedFileBase soundClipUpload)
        {
            using (var request = CreateRequest())
            {
                // Get the sound clip bytes
                var soundClipBytes = new byte[soundClipUpload.ContentLength];
                soundClipUpload.InputStream.Read(soundClipBytes, 0, soundClipUpload.ContentLength);

                // Configure the request body content
                var content = new ByteArrayContent(soundClipBytes);
                content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(soundClipUpload.ContentType);

                // Configure the URI
                var uri = string.Format("instruments/{0}/setsoundclip", itemId);

                // Execute the request
                var resp = await request.PutAsync(uri, content);
            }
        }

        // Configure an instrument object with a photo
        internal async Task SetPhoto(int itemId, HttpPostedFileBase photoUpload)
        {
            using (var request = CreateRequest())
            {
                // Get the photo bytes
                var photoBytes = new byte[photoUpload.ContentLength];
                photoUpload.InputStream.Read(photoBytes, 0, photoUpload.ContentLength);

                // Configure the request body content
                var content = new ByteArrayContent(photoBytes);
                content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(photoUpload.ContentType);

                // Configure the URI
                var uri = string.Format("instruments/{0}/setphoto", itemId);

                // Execute the request
                var resp = await request.PutAsync(uri, content);
            }
        }
    }

    // Data format for the items we need in the access token from the identity server
    public class AccessToken
    {
        public string access_token { get; set; }
        public string userName { get; set; }
    }
}
