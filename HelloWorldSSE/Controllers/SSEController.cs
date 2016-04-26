using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Threading;

namespace HelloWorldSSE.Controllers
{

    

    public class SSEController : ApiController
    {
        static Timer _timer = default(Timer);
        private static readonly Queue<StreamWriter> _clients = new Queue<StreamWriter>();

        // konstruktor
        public SSEController()
        {
            _timer = _timer ?? new Timer(OnTimerEvent, null, 0, 1000);
        }

        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)streamAvailableHandler, "text/event-stream");
            return response;
        }

        private void streamAvailableHandler(Stream stream, HttpContent content, TransportContext context)
        {
            StreamWriter clientStream = new StreamWriter(stream);
            _clients.Enqueue(clientStream);
        }

        static void OnTimerEvent(object state)
        {   
                if(_clients.Count != 0)
                {
                    string message = "david är en gnome";
                    foreach (StreamWriter cs in _clients)
                    {
                        try
                        {
                            // måste vara WriteLine och "data: "
                            cs.WriteLine("data:" + message);
                            cs.Flush();
                        }
                        catch (Exception e) { throw e; }
                    }
                }
        }
    }
}
