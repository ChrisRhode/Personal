<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Lookup.aspx.vb" Inherits="Critters.Lookup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Animal Crossing NH - Critter Lookup</title>
</head>
<body>
    <meta name="viewport" content="width=device-width,initial-scale=1.0" />
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblVersion" runat="server" Text="Label"></asp:Label>&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton1" runat="server">About/Version History</asp:LinkButton>
            <p></p>
            <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Critter Type"></asp:Label>
            <asp:RadioButtonList ID="rblCritterType" runat="server">
                <asp:ListItem>Bugs</asp:ListItem>
                <asp:ListItem>Fish</asp:ListItem>
                <asp:ListItem>Sea</asp:ListItem>
                <asp:ListItem>All</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Availability"></asp:Label>
            <asp:RadioButtonList ID="rblAvailability" runat="server">
                <asp:ListItem>No filter</asp:ListItem>
                <asp:ListItem>Current Month</asp:ListItem>
                <asp:ListItem>This Month or Next Month</asp:ListItem>
                <asp:ListItem>Leaving This Month</asp:ListItem>
                <asp:ListItem Value="Now">Right Now</asp:ListItem>
                <asp:ListItem>Evening + Early Morning</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Sell Price"></asp:Label>
            <asp:RadioButtonList ID="rblSellPrice" runat="server" AutoPostBack="True">
                <asp:ListItem>No filter</asp:ListItem>
                <asp:ListItem Value="GE500">&gt;= 500</asp:ListItem>
                <asp:ListItem Value="LT250">&lt; 250</asp:ListItem>
                <asp:ListItem Value="Midrange">250-2499</asp:ListItem>
                <asp:ListItem Value="GE2500">&gt;= 2500</asp:ListItem>
                <asp:ListItem>Manual</asp:ListItem>
            </asp:RadioButtonList>
            <asp:TextBox ID="tbLow" runat="server" style="margin-right: 0px" Width="80px"></asp:TextBox>&nbsp;to&nbsp;
            <asp:TextBox ID="tbHigh" runat="server" Width="80px"></asp:TextBox>
            <br /><br />
            <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Sort Order"></asp:Label>&nbsp;&nbsp;
            <asp:TextBox ID="tbSortTerms" runat="server" ReadOnly="True" style="margin-bottom: 0px"></asp:TextBox>&nbsp;&nbsp;
            <asp:Button ID="btnSortClear" runat="server" Text="Clear" />&nbsp;&nbsp;
            <asp:Button ID="btnSortName" runat="server" Text="by Name" />&nbsp;&nbsp;
            <asp:Button ID="btnSortType" runat="server" Text="by Type" />&nbsp;&nbsp;
            <asp:Button ID="btnSortPrice" runat="server" Text="by Sell Price" />&nbsp;&nbsp;
            <asp:Button ID="btnSortLocation" runat="server" Text="by Location" />&nbsp;&nbsp;
            <asp:Button ID="btnSortSize" runat="server" Text="by Shadow Size" />&nbsp;&nbsp;
            <asp:Button ID="btnSortTime" runat="server" Text="by When" />&nbsp;<i>(Use Clear to clear terms, then buttons to enter sort terms in desired order)</i>
            <br /><br />
            <asp:Label ID="Label5" runat="server" Text="Search for critter containing word/phrase: "></asp:Label>&nbsp;&nbsp;
            <asp:TextBox ID="tbWordSearch" runat="server"></asp:TextBox>&nbsp;&nbsp;
            <asp:CheckBox ID="cbIgnoreFilters" runat="server" Text="Ignore all set filters for text search" />
            <br />
            <asp:Button ID="btnUpdate" runat="server" Text="Update Table With These Settings" />           
            <br />
            <br />
            <asp:Table ID="tblResults" runat="server"></asp:Table>          
        </div>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </form>
</body>
</html>
