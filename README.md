# bootstrapping-ui-automation 
Bootstrapping of a bare minimum, opinionated .Net Core Selenium Framework using MsTest, the in-built .Net Core DI Container, Serilog, .runsettings and Visual Studio.  

Optional use of Zalenium (dockerized Selenium Grid) via docker or Kubernetes (Minikube only at the moment). 

## Why?
My goal is to be up and running with Selenium across several browsers within 15 minutes on either Linux or Windows in .Net Core using Visual Studio. By tweaking a few settings I want to optionally target Selenium Grid, different environments and change my control settings (such as timeouts and so forth). This repository lets me do that. 

Browser selection, environment selection, timeouts/control management, parameterization, remote web driver configuration, environment variable overrides, multi-element eventual consistency, runsettings/IDE integration, simple logging, dependency injection/container initialization and so forth are taken care of. 

No reporting. No screenshots. No extra niceties. No page object model. Not a lot of helper methods. Always incomplete. Why? Because the more I add, the more opinionated and prescribed it becomes and the less flexible it will be to adapt. Single Page Applications vs form-based apps influence where assertions go. Page Object Model vs Screenplay pattern influence structure. Stakeholders influence the kind of reporting needed. The underlying test engine (MsTest, NUnit, xUnit) dictates how parallelization is performed. etc. etc. etc.  

More a reference for me. If it helps you too - win, win. If not: win :)

A few automated (raw, inline locator) tests are written against https://the-internet.herokuapp.com/ - that site contains all kinds of UI Automation Nastiness. 

The intention is to incorporate a lot more solutions to the-internet problems; I will chip away at this in the background. 

# .NetCore-Selenium
See the .NetCore-Selenium README.md for more information on how to orchestrate it. 

## Infrastructure
You can run 'the-internet' yourself in the following ways: 

| Target | Description |
| ------ | ----------- |
| infra/docker-localhost | Uses a docker-compose file to spin up the-internet locally |
| infra/minikube | Uses minikube / kubectl to start the-internet locally and expose it via an external 'LoadBalancer' port on your own machine |
