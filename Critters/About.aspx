<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="About.aspx.vb" Inherits="Critters.About" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
        <meta name="viewport" content="width=device-width,initial-scale=1.0" />
    <form id="form1" runat="server">
        <div>
            Developed by Chris Rhode
            <br />
            Raw data from <a href="http://icebr.art">AC:NH Helper</a> by icebearacnh@gmail.com
            <ol>
                <li>V1.0 initial release</li>
                <li>V1.1 05/17/2020</li>
                <ul>
                    <li>Reengineered filters and sorts; it is now possible to sort (pretty well) on Hours/When, but for now at least the Month Column has been removed and "This month or next month" selector has been changed to "next month"</li>
                    <li>Fixed a bug with Evening/Early morning filter which was incorrectly omitting some critters</li>
                    <li>Added ability to enter a search word and display all critters whose name contains that search word</li>
                    <li>Fixed sort so you cannot add the same sort term more than once</li>
                </ul>
                <li>V1.2 05/18/2020</li>
                <ul>
                    <li>Redid radio buttons as RadioButtonLists to avoid wrapping issues in layout</li>
                </ul>
                <li>V1.3 05/19/2020</li>
                <ul>
                    <li>Restore Months Available in table by using STUFF/XML in SQL</li>
                    <li>Replace Next Month again with This Month or Next Month</li>
                    <li>Allow text search to be a phrase</li>
                    <li>Add Critter Type as possible sort key</li>
                    <li>Additional sanity checking on manual Sell Price range entry</li>
                    <li>Set default sort by Name</li>
                </ul>
            </ol>
            <p></p>
            <asp:Button ID="Button1" runat="server" Text="Go back to Lookup" />
        </div>      
    </form>
</body>
</html>
