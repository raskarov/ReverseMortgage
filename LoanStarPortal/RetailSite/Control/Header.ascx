<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="LoanStarPortal.RetailSite.Control.Header" %>
<div id="header">
    <div class="bg">
        <div class="indent">
            <div class="container">
                <div class="left">
                    <h1 contenteditable="">
                        Mainstreet Loans, Inc.</h1>
                </div>
                <div class="right">
                    <ul>
                        <li><a href="#">Sign In</a></li>
                        <li><a href="#">Site Map</a></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
            </div>
            <img alt="" class="imgindent" height="278" src="images/SeniorsCropped.jpg" width="384" /><img
                alt="" src="images/slogan_TrustIntegrityValue.jpg" /><br />
            <a href="Reverse_Mortgage_Basics.ppt">
                <img id="IMG1" alt="" src="images/question1.jpg" /></a><br />
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../images/question2.jpg" BorderWidth="0px" Height="64px" OnClick="ImageButton1_Click" />
            <div class="menu">
                <ul>
                    <li><a href="#"></a></li>
                    <li><a href="#"></a></li>
                    <li><a href="#"></a></li>
                    <li><a href="#"></a></li>
                    <li><a href="#"></a></li>
                    <li><a href="#"></a></li>
                    <li><a class="last" href="#"></a></li>
                </ul>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
</div>
