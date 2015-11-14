using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Net;
using System.Net.Http;
using AutoMapper;

namespace Lab6.ServiceLayer
{
    // This is the customized error handler and logger
    // Two classes are needed - an exception HANDLER, and an exception LOGGER
    // The logger is executed before the handler

    public class HandleError : ExceptionHandler
    {
        // Packaging for the error info
        private class ErrorInfo
        {
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
        }

        // Create the error info object to be returned to the requestor
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result =
                new ResponseMessageResult
                (context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new ErrorInfo { Message = context.Exception.Message, Timestamp = DateTime.Now }));
        }

    }

    public class LogError : ExceptionLogger
    {
        Worker w = new Worker();

        // Save error info to the persistent store
        public override void Log(ExceptionLoggerContext context)
        {
            Controllers.ExceptionInfoAdd ex = new Controllers.ExceptionInfoAdd();

            // Extract the error info that will be useful
            ex.Source = context.Exception.Source;
            ex.Method = context.Request.Method.Method;
            ex.Message = context.Exception.Message;
            ex.UserName = context.RequestContext.Principal.Identity.Name;
            ex.StackTrace = context.Exception.StackTrace;

            w.Exceptions.Add(ex);
        }
    }

}
