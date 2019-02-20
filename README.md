# SmartSheetTest
A test application for use of the SmartSheet API. 

When navigating to the page Home/Form/, an exception will be purposely triggered. A form will open which contains information 
about this exception, as well as a field for the user to fill out and a button to upload an image. When submit is pressed, 
This information is automatically added to a SmartSheets spreadsheet, with the image as an attachment to the row.

Important files:

[HomeController.cs](https://github.com/cjohnson57/SmartSheetTest/blob/master/SmartSheetTest/Controllers/HomeController.cs) (Controller)

[Form.cshtlm](https://github.com/cjohnson57/SmartSheetTest/blob/master/SmartSheetTest/Views/Home/Form.cshtml) (View)

[ErrorReport.cs](https://github.com/cjohnson57/SmartSheetTest/blob/master/SmartSheetTest/Models/ErrorReport.cs) (Model)
