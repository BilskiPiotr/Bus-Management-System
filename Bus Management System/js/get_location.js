var wartosc1 = "";
var wartosc2 = "";
var wartosc3 = "";
var error = "";
var dane = new Array();

setInterval(getLocation, 5000);

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(KonstruujArray, function () { }, { enableHighAccuracy: true });
    }
    else {
        error.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
    }
}

function KonstruujArray(coordinates) {
    wartosc1 = coordinates.coords.latitude;
    wartosc2 = coordinates.coords.longitude;
    wartosc3 = coordinates.coords.accuracy;
    document.getElementById("<%=HiddenField1.ClientID%>").value = wartosc1;
    document.getElementById("<%=HiddenField2.ClientID%>").value = wartosc2;
    document.getElementById("<%=HiddenField3.ClientID%>").value = wartosc3;
    dane[0] = document.getElementById("<%=HiddenField1.ClientID%>").value;
    dane[1] = document.getElementById("<%=HiddenField2.ClientID%>").value;
    dane[2] = document.getElementById("<%=HiddenField3.ClientID%>").value;
    PageMethods.PrzeliczArray(dane, OnSuccess);
}
function OnSuccess(response, userContext, methodName) {
    document.getElementById("<%=lb_BusLatitude.ClientID %>").innerHTML = response[0];
    document.getElementById("<%=lb_BusLongitude.ClientID %>").innerHTML = response[1];
    document.getElementById("<%=lb_BusDistance.ClientID %>").innerHTML = response[2];
    document.getElementById("<%=lb_BusAccuracy.ClientID %>").innerHTML = wartosc3;
    wartosc1 = "";
    wartosc2 = "";
    wartosc3 = "";

    var txt = "";
    txt += "<p>Total width/height: " + screen.width + "*" + screen.height + "</p>";
    txt += "<p>Available width/height: " + screen.availWidth + "*" + screen.availHeight + "</p>";
    document.getElementById("<%=lblDim.ClientID %>").innerHTML = txt;
}