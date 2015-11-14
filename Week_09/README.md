### Week 9 code examples

**IdentityServerV2**

Based on the "IdentityServerV1" project that was used in Lab 7.  
This app adds the ability to handle claims.  

**SolutionForLab7**

As its name suggests, it's a solution for the recent Lab 7 assignment.  
Look for the "Attention" comment tokens, as they explain the implementation details.  
This app uses security, and therefore must be used with the Identity Server (version 2) app.  

**ClientAppInstruments**

This is an ASP.NET MVC web app. No security. Therefore, no local database (or any form of persistence).  
Designed to interact with a web service - SolutionForLab7.  
How? The URI of the web service is programmed into a "factory" method in the Manager class.  

The three apps above must be used together, and loaded/run in the sequence listed above.  
