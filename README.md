# DocCover
[![GitHub release](https://img.shields.io/github/release/Artees/DocCover.svg?style=flat-square)](https://github.com/Artees/DocCover/releases)
[![NuGet](https://img.shields.io/nuget/v/DocCover.svg?style=flat-square)](https://www.nuget.org/packages/DocCover/)

DocCover calculates the percentage of public members and types in your .NET assembly that have 
XML comments. I just wanted this badge for my repos :3 
[![Documented](report_example/badge.svg)](report_example/index.html)  
It uses [Shields.io](https://shields.io) to generate a badge.

## Usage
`path\to\DocCover.exe path\to\docs.xml`

Command line arguments:

| Argument                  	| Description                                                                                                                                     	|
|---------------------------	|-------------------------------------------------------------------------------------------------------------------------------------------------	|
| --help                    	| Display the help screen.                                                                                                                        	|
| --version                 	| Display version information.                                                                                                                    	|
| -x, --xml (pos. 0)        	| The XML document to be analyzed.                                                                                                                	|
| -d, --dll (pos. 1)        	| The assembly file to be analyzed. If not specified, the path of the XML document will be used.                                                  	|
| -o, --outputdir (pos. 2)  	| The directory where the generated report should be saved.                                                                                       	|
| -s, --badgestyle (pos. 3) 	| The style of the generated badge. The following styles are available: plastic, flat, flat-square, for-the-badge, popout, popout-square, social. 	|