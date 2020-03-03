# bootstrapping-ui-automation 
Automation frameworks and workflows across multiple languages. 

And a few solutions to the problems on  https://the-internet.herokuapp.com/

## .Net Core - Selenium
Bootstrapping of a bare minimum, opinionated .Net Core Selenium Framework using MsTest, the in-built .Net Core DI Container, Serilog, .runsettings and Visual Studio.  

This framework supports a 'hot-reload' workflow that allows you to run your tests against an existing browser instance: see the NetCore-Selenium/README.md file for more information.

Optional use of Zalenium (dockerized Selenium Grid) via docker or Kubernetes (Minikube only at the moment). 

### Why?
My goal is to be up and running with Selenium across several browsers within 15 minutes on either Linux or Windows in .Net Core using Visual Studio. By tweaking a few settings I want to optionally target Selenium Grid, different environments and change my control settings (such as timeouts and so forth). This repository lets me do that. 

## Infrastructure
You can run 'the-internet' yourself in the following ways: 

| Target | Description |
| ------ | ----------- |
| infra/docker-localhost | Uses a docker-compose file to spin up the-internet locally |
| infra/minikube | Uses minikube / kubectl to start the-internet locally and expose it via an external 'LoadBalancer' port on your own machine |
