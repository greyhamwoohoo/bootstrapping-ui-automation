# .Net Core - Selenium
Bootstrapping of a bare minimum, opinionated .Net Core Selenium Framework using MsTest, the in-built .Net Core DI Container, Serilog, .runsettings and Visual Studio.  

This framework supports a 'hot-reload' workflow that allows you to run your tests against an existing browser instance: see below for more information. 

Optional use of Zalenium (dockerized Selenium Grid) via docker or Kubernetes (Minikube only at the moment). 

## Why?
My goal is to be up and running with Selenium across several browsers within 15 minutes on either Linux or Windows in .Net Core using Visual Studio. By tweaking a few settings I want to optionally target Selenium Grid, different environments and change my control settings (such as timeouts and so forth). This repository lets me do that. 

Browser selection, hot-reload, environment selection, timeouts/control management, remote web driver configuration, environment variable overrides, multi-element eventual consistency, runsettings/IDE integration, simple logging, dependency injection/container initialization and so forth are taken care of. 

No reporting. No screenshots. No extra niceties. No page object model. Not a lot of helper methods. Always incomplete. Why? Because the more I add, the more opinionated and prescribed it becomes and the less flexible it will be to adapt. Single Page Applications vs form-based apps influence where assertions go. Page Object Model vs Screenplay pattern influence structure. Stakeholders influence the kind of reporting needed. The underlying test engine (MsTest, NUnit, xUnit) dictates how parallelization is performed. etc. etc. etc.  

A few automated (raw, inline locator) tests are written against https://the-internet.herokuapp.com/ - that site contains all kinds of UI Automation Nastiness. 

## Framework Parameters
By default, the tests will run using the Chrome browser against http://the-internet.herokuapp.com.

Environment variables, test execution contexts or .runsettings can be used to change execution parameters:

| Environment Variable | Default | Description |
| -------------------- | ------- | ----------- |
| THEINTERNET_BROWSERSETTINGS_FILES | chrome-default.json | Launches an incognito Chrome |
| THEINTERNET_REMOTEWEBDRIVERSETTINGS_FILES | localhost.json | Does not use a remote webdriver - launches locally |
| THEINTERNET_ENVIRONMENTSETTINGS_FILES | internet.json | The target environment for the tests. ie: where the application (baseUrl) will point to |
| THEINTERNET_CONTROLSETTINGS_FILES | default.json | Element timeouts, polling frequency etc. |

The following browsers (and configurations) are supported - choose the browser by setting THEINTERNET_BROWSERSETTINGS_FILES environment variable to one of these values:

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

## Test Execution Contexts (.runsettings)
It is convenient within Visual Studio to quickly switch between - say - 'default-firefox' and 'default-chrome' and/or different environments depending on the work you are doing. This can be done using the .runsettings files in the TestExecutionContexts folder. 

To choose a .runsettings file:

1. Select Test / Configure Run Settings / Select Solution Wide runsettings file from the menu
2. Choose one of the .runsettings files
3. Run your tests

The 'real' settings are stored in the testsettings.*.json files - at the moment, these settings are the environment variables that need to be set for that test run. The 'TestRunInitialization.cs' file contains the logic to set this up. 

If executing the tests from the command line, you can specify the name of the test execution context by setting this variable:

```
THEINTERNET_TEST_EXECUTION_CONTEXT=default-chrome-localhost
```

| RunSettings | Test Execution Context | Full filename | Description | 
| ----------- | ---------------------- | ------------- | ----------- |
| Default-Chrome-Localhost.runsettings | default-chrome-localhost | testsettings.default-chrome-localhost.json | The default. Launches Chrome |
| Attachable-Chrome-Localhost.runsettings | attachable-chrome-localhost | testsettings.attachable-chrome-localhost.json | Will attach to an already-running Selenium Driver instance if it exists and was started when this .runsettings was active. |
| Default-Edge-Localhost.runsettings | default-edge-localhost | testsettings.default-edge-localhost.json | Runs the tests in Edge |
| Default-FireFox-Localhost.runsettings | default-firefox-localhost | testsettings.default-firefox-localhost.json | Runs the tests in Firerfox |
| Default-FireFox-Zalenium-Localhost.runsettings | default-firefox-zalenium-localhost | testsettings.default-firefox-zalenium-localhost.json | Runs the tests in Firefox against Zalenium |

## Hot Reload Functionality / Scratchpad
The problem I want to solve is this:

I want a 'scratchpad' where I can write a few lines of Selenium and have them executed against an existing broweser - on whichever page and state that browser might be. I want to do this within my testing framework and be able to leverage any functionality that my framework might provide. 

For simplicity in this case: the 'scratchpad' takes the form of a single test  - it's just a normmal test - called 'HotReloadScratchpad'. 

The first time the test is run with a test execution context of 'attachable-chrome-localhost' (via Attachable-Chrome-Localhost.runsettings) it will start a browser instance BUT NOT CLOSE IT. Subsequent test runs using the same test execution context will reuse the same browser instance. This allows you experiment with one or two lines of Selenium at a time for faster feedback before moving that code into your main test workflow; you are of course able to manually interact and mutate that browser state as you wish. 

There are two viable workflows:

### Automatically
Open  up a command prompt in TheInternet.SystemTests.Raw folder and execute the following command:

```
SET THEINTERNET_TEST_EXECUTION_CONTEXT=attachable-chrome-localhost
dotnet watch test --filter "Name="HotReloadWorkflow"
```

Whenever we save anything in the TheInternet.SystemTests.Raw project, that single test will be run. On my machine it takes around 6 seconds for the build and test exection to occur; perhaps 'warm-reload' is a better description :)

### Within Visual Studio
1. Select the Attachable-Chrome-Localhost .runsettings file *FIRST*
2. Run the 'HotReloadWorkflow' test (or: ANY test!)
3. The browser will NOT be terminated on shutdown (or test failure etc. )
4. Run a few lines of Selenium in the 'HotReloadWorkflow' test
5. The existing browser will be reused

### Implementation
The persisted session state is persisted in the test run folder as '.selenium.session'. If anything catastrophic should happen that it cannot recover from, manually delete that file. 

If you have closed the browser manually between test runs, the implementation will start a new browser session automatically. 

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
