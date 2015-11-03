﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.IO;

namespace MediaUpload.ServiceLayer
{
    // Attention - Formatter to handle byte-oriented content, such as images, audio, video, documents

    public class ByteFormatter : BufferedMediaTypeFormatter
    {
        // We need five methods:
        // 1. Constructor
        // 2. Can read?
        // 3. Reader
        // 4. Can write?
        // 5. Writer

        public ByteFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/gif"));
        }

        // Can this formatter read?
        // Enables a client/user/requestor to "upload" an image
        // Example usage - PUT a new/updated image
        public override bool CanReadType(Type type)
        {
            return (type == typeof(byte[]));
        }

        // This method reads bytes from an input stream (i.e. an "upload")
        public override object ReadFromStream(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            // Create an in-memory buffer
            var ms = new MemoryStream();
            // Copy the request message body content to the in-memory buffer
            readStream.CopyTo(ms);
            // Deliver a byte array to the controller
            return ms.ToArray();
        }

        // Can this formatter write? 
        // Delivers an image to the client/user/requestor
        // Example usage - GET an image
        public override bool CanWriteType(Type type)
        {
            return (type == typeof(byte[]));
        }

        // This method writes bytes to the output stream
        public override void WriteToStream(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content)
        {
            // Extract the image bytes to an in-memory buffer
            MemoryStream ms = new MemoryStream(value as byte[]);

            // Deliver the bytes
            ms.CopyTo(writeStream);
        }

    }

}