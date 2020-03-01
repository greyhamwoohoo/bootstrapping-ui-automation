# .Net Core - Selenium
Bootstrapping of a bare minimum, opinionated .Net Core Selenium Framework using MsTest, the in-built .Net Core DI Container, Serilog, .runsettings and Visual Studio.  

This framework optionally lets you attach to an already-running Selenium session by using the 'Attachable-Chrome-Localhost' .runsettings file (see below) - 
this vastly improves the development workflow. 

Optional use of Zalenium (dockerized Selenium Grid) via docker or Kubernetes (Minikube only at the moment). 


## Why?
My goal is to be up and running with Selenium across several browsers within 15 minutes on either Linux or Windows in .Net Core using Visual Studio. By tweaking a few settings I want to optionally target Selenium Grid, different environments and change my control settings (such as timeouts and so forth). This repository lets me do that. 

Browser selection, environment selection, timeouts/control management, parameterization, remote web driver configuration, environment variable overrides, multi-element eventual consistency, runsettings/IDE integration, simple logging, dependency injection/container initialization and so forth are taken care of. 

No reporting. No screenshots. No extra niceties. No page object model. Not a lot of helper methods. Always incomplete. Why? Because the more I add, the more opinionated and prescribed it becomes and the less flexible it will be to adapt. Single Page Applications vs form-based apps influence where assertions go. Page Object Model vs Screenplay pattern influence structure. Stakeholders influence the kind of reporting needed. The underlying test engine (MsTest, NUnit, xUnit) dictates how parallelization is performed. etc. etc. etc.  

A few automated (raw, inline locator) tests are written against https://the-internet.herokuapp.com/ - that site contains all kinds of UI Automation Nastiness. 

The intention is to incorporate a lot more solutions to the-internet problems; I will chip away at this in the background. 

## Framework Parameters
By default, the tests will run using the Chrome browser against http://the-internet.herokuapp.com

| Environment Variable | Default | Description |
| -------------------- | ------- | ----------- |
| THEINTERNET_BROWSERSETTINGS_FILES | chrome-default.json | Launches an incognito Chrome |
| THEINTERNET_REMOTEWEBDRIVERSETTINGS_FILES | localhost.json | Does not use a remote webdriver - launches locally |
| THEINTERNET_ENVIRONMENTSETTINGS_FILES | internet.json | The target environment for the tests. ie: where the application (baseUrl) will point to |
| THEINTERNET_CONTROLSETTINGS_FILES | default.json | Element timeouts, polling frequency etc. |

The following browsers (and configurations) are supported - choose the browser by setting THEINTERNET_BROWSERSETTINGS_FILES to one of these values:

| Browser | Value | Description |
| ------- | ----- | ----------- |
| Chrome | default-chrome.json | Launches Chrome with sensible default settings |
| Chrome | headless-chrome.json | Launches Chrome in headless mode (use this to run tests in Docker containers) |
| Chrome | default-performance-chrome.json | Launches Chrome and collects performance information |
| Edge | default-edge.json | Incognito / Private Browsing for Edge |
| FireFox | default-firefox.json | Launches FireFox with sensible default settings |

The following Remote Web Driver Settings files are provided:

| Setting | Description |
| ------- | ----------- |
| localhost.json | The default. Means: do not use a Remote Web Driver |
| zalenium-docker-localhost.json | Targets Zalenium running on localhost via Docker. See the infra/the-internet/zalenium-docker-localhost folder for more information |

### Advanced Parameters
The optional xxx_FILES environment variables support either a fully qualified path; or the name of a file that is expected to exist under the Runtime folder when the tests are executed; or a combination of both. 

This implementation uses the .Net Core ConfigurationBuilder() - this means configuration files can be 'overlayed' and specialized like appsettings. For example, to specify a base file and then overload just one or two properties, specify the original file and then a Json file containing the variations:

```
SET THEINTERNET_BROWSERSETTINGS_FILES=chrome-default.json;chrome-captureLogFile.json;otherspecializations
```

The .Net Core conventions for environment variable overrides are also supported. For example: to change the RemoteWebDriverSettings RemoteUri property in the JSON, do something like this:

```
SET THEINTERNET_REMOTEWEBDRIVERSETTINGS:REMOTEURI="https://localhost.com/overriddenUri"

REM Use the .Net Core __ notation for overriding nested values in the testsettings files: see testsettings.attachable-chrome-localhost.json for an example
SET THEINTERNET_REMOTEWEBDRIVERSETTINGS__REMOTEURI="https://localhost.com/overriddenUri"
```

## Test Execution Contexts (run settings)
It is convenient within Visual Studio to quickly switch between - say - 'default-firefox' and 'default-chrome' and/or different environments depending on the work you are doing. This can be done using the .runsettings files in the TestExecutionContexts folder. 

To choose a .runsettings file:

1. Select Test / Configure Run Settings / Select Solution Wide runsettings file from the menu
2. Choose one of the .runsettings files
3. Run your tests

The 'real' settings are stored in the testsettings.*.json files - at the moment, these settings are the environment variables that need to be set for that test run. 

If executing the tests from the command line, you can specify the name of the test execution context by setting this variable:

```
THEINTERNET_TEST_EXECUTION_CONTEXT=default-chrome
```

| RunSettings | Test Execution Context | Full filename | Description | 
| ----------- | ---------------------- | ------------- | ----------- |
| Default-Chrome-Localhost.runsettings | default-chrome-localhost | testsettings.default-chrome-localhost.json | The default. Launches Chrome |
| Attachable-Chrome-Localhost.runsettings | attachable-chrome-localhost | testsettings.attachable-chrome-localhost.json | Will attach to an already-running Selenium Driver instance if it exists. |
| Default-Edge-Localhost.runsettings | default-edge-localhost | testsettings.default-edge-localhost.json | Runs the tests in Edge |
| Default-FireFox-Localhost.runsettings | default-firefox-localhost | testsettings.default-firefox-localhost.json | Runs the tests in Firerfox |
| Default-FireFox-Zalenium-Localhost.runsettings | default-firefox-zalenium-localhost | testsettings.default-firefox-zalenium-localhost.json | Runs the tests in Firefox against Zalenium |

## How to attach to an existing Selenium Browser Instance
To speed up the development workflow, it is often useful to attach to an existing Selenium Browser Session. This framework supports this in Chrome. 

1. Use the Attachable-Chrome-Localhost .runsettings file *FIRST*
2. Launch your test as normal
3. The browser will NOT be terminated on shutdown (or test failure etc. )
4. Re-run any test
5. The existing browser will be reused

If anything goes wrong, just switch to another .runsettings profile *OR* manually delete the .selenium.session file in your output folder. 

Of course: it goes without saying that if there is no browser session to attach to at the given port, a new one will be created. 
If you have closed the browser manually, but left the orphaned chromedriver.exe around, this implementation will still recover. 

Implementation Notes:
1. This only works with Chrome at the moment (you can probably extend it to other browsers)
2. The implementatation is in Drivers/AttachableChromeDriver.cs
3. The architecture and implementation of the WebDrivers makes it difficult / impossible to inject DI-resolved services. 
   As a result, there's a lot of new-ing up going on. See AttachableChromeDriver.cs for more information. 

### Reference
| Reference | Link |
| --------- | ---- |
| '437' Encoding Error: Allowing FireFox WebDriver to run under .Net Core | https://github.com/SeleniumHQ/selenium/issues/4816 |
| ChromeDriver.exe not copied to output folder | https://stackoverflow.com/questions/55007311/selenium-webdriver-chromedriver-chromedriver-exe-is-not-being-publishing-for-n |
