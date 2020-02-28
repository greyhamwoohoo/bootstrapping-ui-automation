# NetCore-Selenium
A raw implementation - using inline locators/selectors - to drive the pages on the http://the-internet.herokuapp.com website. 

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
| Default-Edge-Localhost.runsettings | default-edge-localhost | testsettings.default-edge-localhost.json | Runs the tests in Edge |
| Default-FireFox-Localhost.runsettings | default-firefox-localhost | testsettings.default-firefox-localhost.json | Runs the tests in Firerfox |
| Default-FireFox-Zalenium-Localhost.runsettings | default-firefox-zalenium-localhost | testsettings.default-firefox-zalenium-localhost.json | Runs the tests in Firefox against Zalenium |

### Reference
| Reference | Link |
| --------- | ---- |
| '437' Encoding Error: Allowing FireFox WebDriver to run under .Net Core | https://github.com/SeleniumHQ/selenium/issues/4816 |
| ChromeDriver.exe not copied to output folder | https://stackoverflow.com/questions/55007311/selenium-webdriver-chromedriver-chromedriver-exe-is-not-being-publishing-for-n |
