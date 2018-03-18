# *Report*

#### Objectives ####
* Get familiar with MVC pattern;
#### Framework: ASP.NET
ASP.NET MVC is a framework for building web application that the general hierarchical Model-View-Controller pattern.  
So, let's describe the components of MVC pattern , which are vey useful for building WebApplication: 

* Model - is the central component of pattern. It directly manages the data,
logic and rules of application.Also another approach for this component si relation with BusinessLogic(),
which is very useful for interaction with database. The model is responsible for managing the data of the application.
It receives user input from the controller.
* View - output of information for user(UI), such as a chart or a diagram.
* Controller - accepts input and converts it to commands for the model or view.



In this laboratory work I got experience work with MVC pattern and how integrate the previous project into this pattern using ASp.NET Framework. So, I understood how load all my .css files and scripts .js using bundle(storing in vritual path the .css,.js files). Also , I built a plan how to start viewing my project in MVC: begining with static forms , which are navbar and footer, through _ _Layout.cshtml_ set for all pages. Body evident is particular for each page.  
MVC pattern is a great tool to structure the project in vey clear mode, which simplify the work with Web-Apllications. 
For starting integrated the project in MVC Programmer after installing all packages , should to create some initial files : _BundleConfig.cs_ , _RouteConfig.cs_ and simple _Index.cshtml_.


![alt text][logo]

[logo]: MVC.png "my image"


### References: 
1.Presentation from WebTechnology courses. _Antohi Ionel_  
2.[Bundle](https://docs.microsoft.com/en-us/aspnet/mvc/overview/performance/bundling-and-minification)
