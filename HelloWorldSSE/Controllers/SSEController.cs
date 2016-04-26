using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Collections.Concurrent;

namespace HelloWorldSSE.Controllers
{

    

    public class SSEController : ApiController
    {
        static Timer _timer = default(Timer);
        static readonly ConcurrentQueue<StreamWriter> _clients = new ConcurrentQueue<StreamWriter>();

        // konstruktor
        public SSEController()
        {
            _timer = _timer ?? new Timer(OnTimerEvent, null, 0, 1000);
        }

       // [Route("/SSE")]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)streamAvailableHandler, "text/event-stream");
            
            

            
            return response;
        }

        private void streamAvailableHandler(Stream stream, HttpContent content, TransportContext context)
        {
            var clientStream = new StreamWriter(stream);
            _clients.Enqueue(clientStream);

        }

        static void OnTimerEvent(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            try
                {
                    var message = "data:david är en gnome"+"\n\n";
                    foreach (StreamWriter cs in _clients)
                    {
                        try
                        {
                            // måste vara WriteLine och "data: "
                            cs.WriteLine(message);
                            cs.Flush();
                        }
                        catch (Exception e) {

                        }
                    }
                } catch(Exception e) { }
            finally
            {
                _timer.Change(1000, 1000);
            }
        }
    }
}
