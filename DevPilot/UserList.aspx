<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="DevPilot.UserList" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>User List</title> 
     <!-- Include jQuery library -->
 <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="Scripts/UserList.js"></script>
</head>
<body>
    <h1>User List</h1>
    <table id="userListTable">
        <thead>
            <tr>
                <th>User Name</th>
                <th>Access Level</th>
            </tr>
        </thead>
        <tbody id="userListBody">
        </tbody>
    </table>
</body>
</html>
