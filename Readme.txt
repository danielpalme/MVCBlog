=======================================================================
=                                                                     =
=                                                                     =
=                               MVCBlog                               =
=     Blog engine based on ASP.NET MVC 4 and Twitter Bootstrap 3      =
=                                                                     =
=                       http://www.palmmedia.de                       =
=                                                                     =
=                                                                     =
=======================================================================
FEATURES

-RSS-Feed
-Embedding of images
-File attachments
-Comments
-Tags
-Search function
-Receiving and sending pingbacks
-TinyMCE, Lightbox and SyntaxHighlighter integration
-Dependency Injection using SimpleInjector

=======================================================================

OPEN WORK ITEMS

-Implement deleting images
-Implement comments feed
-Refactor deleting posts in a POST instead of a GET request (not 
 critical, since only authenticated users are allowed to delete)
-Refactor deleting files in a POST instead of a GET request (not 
 critical, since only authenticated users are allowed to delete)

=======================================================================

REQUIREMENTS

.NET Framework 4.5
ASP.NET MVC 4
MSSQL

=======================================================================

LICENSE

This program is licensed under the Apache License 2.0.
This means you may use this program in any project.
You are allowed to modify the program as you like.

For further details take a look at LICENSE.txt.

=======================================================================

CHANGELOG

0.5.0.0

    * New: Updated to ASP.NET MVC 4, Twitter Bootstrap

0.4.0.0

    * New: Using Unity.MVC3
    * New: Using Yui Compressor for combined JS and CSS
    * Fix: Updated dependencies
    
0.3.0.0

    * New: Dependencies now managed with NuGet

0.2.0.0

    * New: ASP.NET MVC 3 with RazorViewEngine
    * New: Replaced 'MSSQL Express' with 'MSSQL CE 4.0 RTM'

0.1.0.0

    * New: Initial release