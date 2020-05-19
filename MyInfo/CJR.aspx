<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CJR.aspx.cs" Inherits="MyInfo.CJR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Personal Project Page - Chris Rhode</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Recent Personal Projects</h1>
            I am doing these projects to refresh/challenge my skills and develop applications that
            help me in my personal life.<br />
            Any enhancements I've thought of for these projects are tracked as enhancement issues in GitHub.
            <hr />
            <h1>iOS App: To-Do List</h1>
            <i>Objective C / Swift / SQLITE</i>
                <ul>
                    <li>Implements a To-Do list with features that I find essential, such as hierarchical sub-lists,
                        automatic search for similar existing items when adding a new item, and a reviewable transaction log so if you
                        accidentally swipe or type you can see what actually changed in the list.  I have been actively using this app on my personal
                        iPhone for several weeks as my primary To-Do list app.
                    </li>
                    <li><a href="https://github.com/ChrisRhode/ToDo2">Source code for Objective C version on GitHub (primary version, highly functional)</a></li>
                    <li><a href="https://github.com/ChrisRhode/ToDoSwift">Source code for Swift version on GitHub (work in progress)</a></li>
                </ul>
            <hr />
            <h1>iOS App: UPC Barcode Lookup</h1>
            <i>Swift (client) / C#.NET (server)</i>
                <ul>
                    <li>Currently implements a UPC barcode scanner in Swift (using the iDevice camera) that interacts with a C#.NET web service
                        to lookup descriptions of each product scanned via a public lookup web service (api.upcitemdb.com). During the time that the product lookup descriptions are taking place, a status/progress message sequence is displayed. The status message implementation is generic and implemented in the StatusMessage class.
                    </li>
                     <li><a href="https://github.com/ChrisRhode/SwiftCodingWork">Source code for Swift app on GitHub</a></li>
                    <li><a href="https://github.com/ChrisRhode/MyWebServices/tree/master/UPCSvcs">Source code for C#.NET web service on GitHub</a></li>    
                </ul>
            <hr />
            <h1>BART API</h1>
            <i>ASP.NET (C#.NET)</i>
                <ul>
                    <li>Implements an ASP.NET website in C#.NET, doing simple lookup of the current day's scheduled BART departure times for a chosen route from a chosen station.
                        Station and schedule data is pulled from BART dynamically using the BART API (<a href="https://www.bart.gov/schedules/developers/api">https://www.bart.gov/schedules/developers/api</a>).
                        The data is pulled in JSON format and deserialized into classes using NewtonSoft.JSON and JSONUtils.com</li>
                    <li><a href="http://www.zoggoth2.com/BARTAPI/ScheduleForStation.aspx">Demonstration website</a></li>
                    <li><a href="https://github.com/ChrisRhode/BARTAPI">Source code on GitHub</a></li>
                </ul>
            <hr />
            <h1>Humans Database</h1>
             <i>Microsoft MVC v5 (C#.NET) / Microsoft SQL Server</i>
                <ul>
                    <li>Implements a list of Humans (which can have Children), with full editing capability, using the Model View Controller technique
                        and Razor for rendering the page content
                    </li>
                    <li><a href="http://www.zoggoth2.com/HumansDataMVC/Start.aspx">Demonstration website</a></li>
                    <li><a href="https://github.com/ChrisRhode/MyWebServices/tree/master/HumansDataMVC">Source code on GitHub</a></li>
                </ul>
            <hr />
            <h1>Game implementation</h1>
             <i>ASP.NET (VB.NET) / Microsoft SQL Server</i>
                <ul>
                    <li>Implements a text-only version of <a href="https://en.wikipedia.org/wiki/Las_Vegas_(board_game)">Las Vegas Dice Board Game</a>, a dice manipulation game.
                        Currently implements the base game, and the "house dice" (white dice) expansion.  It provides some different player interface
                        behaviors than existing online implementations, that I find more acceptable (e.g. if you only have one move, it still shows that move
                        and lets you make it manually, instead of quickly making it automatically).
                        Each player plays from their own computer or mobile device by accessing the website with their browser.
                        The code maintains the game/player/turn status in SQL Server, allowing all players to join the game in a clean fashion before the game commences.
                        Each player is continually shown the current game status (auto refresh) until it is their turn.
                    </li>
                    <li>To run the demonstration on two different physical devices, see the hints <a href="TwoDevices.html" target="_blank">here</a></li>
                    <li>To run the demonstration on one physical device in one browser window, see the hints <a href="OneDevice.html" target="_blank">here</a></li>
                    <li><a href="http://zoggoth2.com/AddOn2019Site/DiceNewGame.aspx">Demonstration website - Initialize A New Game</a></li>
                     <li><a href="http://zoggoth2.com/AddOn2019Site/Dice.aspx">Demonstration website - Individual Player Access Page</a></li>
                    <li><a href="https://github.com/ChrisRhode/ASPNET">Source code on GitHub</a></li>
                </ul>
            <hr />
            <h1>CMDlet implementation</h1>
             <i>Microsoft PowerShell</i>
            <ul>
                    <li>Implements a custom Cmdlet in VB.NET that returns how many days ago a file was created.
                    </li>
                    <li><a href="https://github.com/ChrisRhode/CJRCmdlets/tree/master/CJRCmdlets">Source code on GitHub</a></li>
                </ul>
        </div>
    </form>
</body>
</html>
