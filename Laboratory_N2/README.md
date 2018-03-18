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



In this laboratory work, I integrated my first laboratory work into my application, in order to accomplish this task, I loaded all my .css 
files into Content folder and all .js into Scripts folder, then I added BundleConfig.cs(a tehnique used to improve the request load time, by reducing the number of request to the server). Next step was configuring the layouts, I have a _Layout.cshtml file,
that contains the pages common elemments(navbar and footer) and also a layout for each other pages(Index, About, Contacts etc)


Reference-style: 
![alt text][logo]

[logo]: https://github.com/gzaharia/WebTechnologiesLabs/blob/master/Laboratory_N2/MVC.png "my image"
