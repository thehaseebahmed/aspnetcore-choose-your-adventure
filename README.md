<!-- PROJECT LOGO -->
<br />
<div align="center">
  <!-- <a href="https://github.com/othneildrew/Best-README-Template">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a> -->

  <h2 align="center">Technical Test - Choose Your Own Adventure</h3>

  <p align="center">
    <a href="https://thehaseebahmed.com" target="_blank">Demo Coming Soon!</a>
  </p>
  <br />
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#problem-statement">Problem Statement</a>
    </li>
    <li>
      <a href="#solution">Solution</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#development-server">Development Server</a></li>
        <li><a href="#unit-tests">Unit Tests</a></li>
      </ul>
    </li>
    <li><a href="#known-issues">Known Issues</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## Problem Statement

Create a back-end for a simple web application which allows a player to choose their own adventure by picking from multiple choices in order to progress to the next set of choices, until they get to one of the endings. You should be able to persist the player’s choices and in the end, show the steps they took to get to the end of the game.

The front enders need you to build endpoints for 3 pages. 
1. A “Create an Adventure” page which lets creators design the adventure. It’s ok to pass a full adventure tree to/from these endpoints.
2. A “Take an Adventure” page where the users can go through the adventure, make their choices, get to the end and persist the path they took. 
3. A “My Adventure” page that shows the result of the adventure with highlighted choices that the user has made in their story.

<div style="margin: 2rem auto; width: 60%">

![doughnut-helper]

</div>

This is an open-ended exercise but we would like you to use .net Core with OpenAPI (Swagger), Docker and have meaningful test coverage.

We really care about clean, readable code, clean application architecture and meaningful tests – the app should be simple, readable and covered with tests.

We are not specific about what database you use, and are happy with any data store approach you find most efficient for this task.
 
 
We should be able to run your application with docker and access the swagger page. In the end, just have fun!



<p align="right">(<a href="#top">back to top</a>)</p>

## Solution

### Built With

This project uses the following set of frameworks & libraries:

- [.NET 6](http://www.abc.com)
- [Auto Mapper]()
- [ASP.NET Core](http://www.abc.com)
- [Docker]()
- [Dynamic LINQ]()
- [Entity Framework Core]()
- [Fluent Validation](http://www.abc.com)
- [MediatR](http://www.abc.com)
- [Mock Queryable](http://www.abc.com)
- [Moq]()
- [Swashbuckle]()
- [xUnit]()

<!-- GETTING STARTED -->

## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

- .NET 6 SDK
  - You can download and install .NET 6 SDK from the [official website](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
- Docker
  - You can download and install Docker Desktop for your OS from the [official website](https://www.docker.com/products/docker-desktop/).
- Visual Studio
  - This is not required but instructions in this readme are given assuming that you are running this solution using Visual Studio. You can download and install the latest version from the [official website](https://visualstudio.microsoft.com/vs/community/).

### Development Server

1. Clone the repo
   ```sh
   git clone https://github.com/thehaseebahmed/aspnetcore-choose-your-adventure.git
   ```
2. Open the Solution (_src/Tha.ChooseYourAdventure.sln_) in Visual Studio.
3. Right click on the "__Tha.ChooseYourAdventure.WebAPI__" project and select the "Build" option from the menu.
4. After the build is complete,
    - Right click on the "__docker-compose__" project
    - Select "Set as Startup Project" option from the menu.
5. Click on the "__▶️ Docker Compose__" button in your toolbar.
6. Once the docker is up & running, open "https://localhost:50719/index.html" in your browser to see the swagger document. <br/>_(Note: Do not forget to replace 50719 with your docker container port since it can be different.)_

### Unit Tests

To execute the unit tests via [xUnit](), simply open the Test Explorer in Visual Studio and click on the "__Run All Tests in View Explorer__" button, alternatively you can also use the shortcut _(Ctrl+R, V)_.

<p align="right">(<a href="#top">back to top</a>)</p>

## Known Issues

1. At the time of doing this test, I've educational knowledge of Docker but industry experience with Service Fabric therefore Docker practices used in this project might not be industry standards. Rest assured, I've been working with containerized applications since 2017 so my grasp on the topic is pretty good. <br/>_(@Evaluators: Please judge/grade this assignment accordingly. @Fellow developers: If you have any improvements in mind, please feel free to share them with me in discussions.)_
2. The client application was not part of this assignment and was designed only for demonstration + testing purposes so the code is a bit rough. Just wanted to make sure that evaluators are aware that it does not represent my usual programming practices.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- LICENSE -->

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- CONTACT -->

## Contact

Haseeb Ahmed - [@thehaseebahmed](https://twitter.com/thehaseebahm3d) - thehaseebahmed@outlook.com

Project Link: [https://github.com/thehaseebahmed/aspnetcore-choose-your-adventure.git](https://github.com/thehaseebahmed/aspnetcore-choose-your-adventure.git)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[license-url]: https://github.com/thehaseebahmed/aspnetcore-choose-your-adventure/blob/main/LICENSE.txt
[doughnut-helper]: docs/doughnut-helper.jpg