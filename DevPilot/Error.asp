<%
Response.Write "<h1>Oops! Something went wrong.</h1>"
Response.Write "<p>An unexpected error occurred. Please try again later.</p>"
 
' Optionally display the error for debugging (not recommended for production)
' Response.Write "Error Number: " & Server.HTMLEncode(Err.Number)
' Response.Write "<br>Error Description: " & Server.HTMLEncode(Err.Description)
%>