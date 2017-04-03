using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncIO
{
    public static class Tasks
    {


        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches. 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris) 
        {
            //return Enumerable.Empty<string>();
            return uris.Select(uri => new WebClient().DownloadString(uri));
        }



        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the asynchronous way and can be used to compare the performace of sync \ async approaches. 
        /// 
        /// maxConcurrentStreams parameter should control the maximum of concurrent streams that are running at the same time (throttling). 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <param name="maxConcurrentStreams">Max count of concurrent request streams</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
        {
            //var tasks = uris.Take(maxConcurrentStreams)
            //    .Select(async uri => await new WebClient().DownloadStringTaskAsync(uri))
            //    .ToArray();

            //foreach (var uri in uris.Skip(5))
            //{
            //    var i = Task.WaitAny(tasks);
            //    var res = tasks[i].Result;
            //    tasks[i] = new WebClient().DownloadStringTaskAsync(uri);
            //    yield return res;
            //}
            //Task.WaitAll(tasks);
            //foreach (var t in tasks)
            //    yield return t.Result;
            var a = uris.Select(uri => new WebClient().DownloadString(uri)).ToArray();
            throw new  NotImplementedException();
        }


        /// <summary>
        /// Calculates MD5 hash of required resource.
        /// 
        /// Method has to run asynchronous. 
        /// Resource can be any of type: http page, ftp file or local file.
        /// </summary>
        /// <param name="resource">Uri of resource</param>
        /// <returns>MD5 hash</returns>
        public static async Task<string> GetMD5Async(this Uri resource)
        {
            var data = await new WebClient().DownloadDataTaskAsync(resource);
            return await Task.Run(() =>
            {
                var byteHash = new MD5CryptoServiceProvider().ComputeHash(data);
                var hashChars = byteHash.Select(b => b.ToString("X2"));
                return string.Join("", hashChars);
            });
        }

    }



}
