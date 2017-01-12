<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebPlotter.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:DropDownList ID="dropDownListTimestamp" runat="server" onselectedindexchanged="dropDownListTimestamp_SelectedIndexChanged" AutoPostBack = "true"/>
    <hr />
        <asp:Chart ID="Chart1" runat="server" Width="1005px" Visible = "False" Height="581px">
            <Titles>
                <asp:Title ShadowOffset="3" Name="Items" />
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
            </Legends>
            <Series>
                <asp:Series Name="Default" ChartType="StepLine" />
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
            </ChartAreas>
        </asp:Chart>
   
    </form>
</body>
</html>
