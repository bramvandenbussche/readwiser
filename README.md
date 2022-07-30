# ReadWiser

Tooling to sync book highlights from [MoonReader](https://play.google.com/store/apps/details?id=com.flyersoft.moonreaderp&hl=nl&gl=US) (the best e-reader for Android) to [Calibre](https://calibre-ebook.com/)

It uses the fact that MoonReader integrates with Readwise and allows you to change the API endpoint.

This repository contains the API that mimicks the Readwise API and stores the received notes & highlights. It also provides some endpoints to retrieve that data. These are implemented by a custom extension for the [Calibre Annotations plugin](https://www.mobileread.com/forums/showthread.php?t=241206), the code for which can be found [here](https://github.com/bramvandenbussche/calibre-annotations/blob/feature/readwiser-importer/readers/ReadWiserApp.py).

The following schematic shows how the whole system works:

![Schematic](docs/readwiser.svg)
