# Shift Redeeming Service

This windows service is designed to scrape Gearboxes twitter at [dgSHiFTCodesBL3](https://twitter.com/dgSHiFTCodesBL3) and automatically submit the codes to Gearboxes website. Currently, this will only work for this twitter account, but it is configurable. It is not recommended to not use the non-BL3 account, as the behavior may not work.

## Installation

Set up your app settings. This information MUST be set, as I'm certainly not giving out my information.
To use this, you must use obtain a Twitter API Key.

Open your appsettings.json and fill in the following information:

You MUST fill in:

```json
 "TwitterAuthentication": "API Key:API Secret Key",
 "Email": "ShiftEmail@example.com",
 "Password": "YourShiftPassword"
```
You can fill in:
```json
  "ScrapeIntervalInMinutes": 120,
  "TwitterAccount": "dgSHiFTCodesBL3",
```

With this information filled in, run the script ```install.sh```

Go to your services on windows and start ```Shift Redemption Service``` and optionally start the service on launch.

## Uninstalling

To uninstall the program, simply run the script ```uninstall.sh```

## Contributing
Feel free to submit any issues or pull requests (labelled with an appropriate issue)

## Future Additions
- Add emails for errors and successful code redemptions
- Add error handling for failed Twitter API calls, failed shift logins and code redemptions, and etc

## License
[MIT](https://choosealicense.com/licenses/mit/)

## Disclaimer
I do not work for Gearbox Software, and I have no affiliation with them.

© 2018 Gearbox Software, LLC. SHiFT is trademark of Gearbox Software, LLC.