### Week 8 code examples

**ProjectWithSecurity**

Based on the "Web API project v2" project template.  
Look at its Task List, and the "Attention" token.  The items on that list show you where to find the security components.  

**GenerateMachineKeyController**

Add this class to the Controllers folder of a project.  
Build and run.  
Request the resource /generatemachinekey  
Copy-then-paste the result to the "system.web" section of BOTH the Identity Server project, AND your Lab 7 project.  
This work will enable the projects to work when deployed to Azure.  

**LocalSecurity**

Builds upon ProjectWithSecurity above.  
Look at the "Attention" comments by using Task List in Visual Studio.  
Adds claims processing as a feature.  
Includes a Test controller, enabling you to test authentication, view claims, and test role claims.  
