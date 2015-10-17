### Week 7 code examples

**MediaUpload**

Shows how to handle internet media types in an app.  
This app defines an entity with a "photo" property.  
It enables the requestor to create objects, and also to configure an object with a "photo".  
Introduces you to a "media type formatter" module that we add to the pipeline.  
This app does NOT deliver media to the requestor. Upload/accept only.  

**MediaUploadAndDeliver**

Based on the MediaUpload code example (above).  
Adds the ability to deliver media to the requestor.  
Introduces you to the coding style for "content negotiation".  

**ByteFormatter**

The folder has only the media type formatter class.  
It can be added to any existing project.  

**Web API project v2**

Based on the Web API project v1 project template.  
Adds a media type formatter (to the ServiceLayer folder).  
Save this file to the following folder:  
%userprofile%\Documents\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\Web\  
-or-  
%userprofile%\Documents\Visual Studio 2013\Templates\ProjectTemplates\Visual C#\Web\  
