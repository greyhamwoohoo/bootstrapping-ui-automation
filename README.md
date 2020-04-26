# netcore-selenium-framework 
Bootstrapping of a bare minimum, opinionated .Net Core Selenium Framework using MsTest, the in-built .Net Core DI Container, Serilog, .runsettings and Visual Studio. 

This framework supports a 'hot-reload' workflow that allows you to run your tests against an existing browser instance: see the NetCore-Selenium/README.md file for more information.

Optional use of Zalenium (dockerized Selenium Grid) via docker or Kubernetes (Minikube only at the moment). 

NOTE: Appium is slowly being added, but is a work in progress. 

## What it is not
This framework is not trying to be a fully fledged, ready to use, Selenium framework with POM or a Fluent API. This implementation is mostly a personal reference for patterns to separate, configure and orchestrate browsers, selenium grid, environments, control settings and so forth via configuration files and environment variable overrides using .Net Core, DI and Visual Studio. That's pretty much it :)

If you are looking for a fully fledged Selenium / Automation framework implementation, that problem has already been solved: consider looking at [Atata Framework](https://github.com/atata-framework)

## Builds
A few example pipelines are included for reference. 

| Build | Result | Description |
| ----- | ------ | ----------- |
| internet.yml | [![Build Status](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_apis/build/status/netcore-selenium-framework-chrome-internet?branchName=master)](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_build/latest?definitionId=25&branchName=master) | Builds and runs the tests on a Windows VM and targets http://the-internet.herokuapp.com |
| internet-localhost.yml | [![Build Status](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_apis/build/status/netcore-selenium-framework-internet-localhost?branchName=master)](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_build/latest?definitionId=26&branchName=master) | Builds and runs the tests on an Ubuntu VM and targets 'the-internet' in a container started on the build system |
| zalenium-localhost-vm.yml | [![Build Status](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_apis/build/status/netcore-selenium-framework-zalenium-localhost-vm?branchName=master)](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_build/latest?definitionId=24&branchName=master) | Builds and runs the tests on an Ubuntu VM and targets http://the-internet.herokuapp.com via Zalenium (Dockerized Selenium Grid) started on the build system |

## Versions
The baseline framework is the same - but the attachable driver / hot-reload workflow implementation is very dependent on the version of Selenium in use. 

| Folder | Description | 
| ------ | ----------- |
| src/NetCore-Selenium | Reference Implementation: <br><br>Baseline framework and attachable drivers using Selenium 3.141  (IE, Chrome, Edge) |
| src/NetCore-Selenium-4.0-alpha3 | (OBSOLETE) Baseline framework and attachable drivers using Selenium 4.0.0.0-alpha-03  (IE, Chrome, Edge) <br><br>NOTE: This was created as a Spike, it works, but is out of date from the original implementation. NetCore-Selenium is the reference implementation. |

## Infrastructure
You can run 'the-internet' yourself in the following ways: 

| Target | Description |
| ------ | ----------- |
| infra/docker-localhost | Uses a docker-compose file to spin up the-internet locally |
| infra/minikube | Uses minikube / kubectl to start the-internet locally and expose it via an external 'LoadBalancer' port on your own machine |
| infra/zalenium-docker-localhost | Runs Zalenium (Dockerized Selenium Grid) - supports Firefox and Chrome browsers |
