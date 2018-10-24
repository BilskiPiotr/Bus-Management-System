
    var coorLat = "";
    var coorLon = "";
    var coorAcc = "";
    var coorSpe = "";
    var alert = "";
    var soundObject = null;
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
        coorLat = coordinates.coords.latitude;
        coorLon = coordinates.coords.longitude;
        coorAcc = coordinates.coords.accuracy;
        coorSpe = coordinates.coords.speed;
        document.getElementById("<%=HiddenField1.ClientID%>").value = coorLat;
        document.getElementById("<%=HiddenField2.ClientID%>").value = coorLon;
        document.getElementById("<%=HiddenField3.ClientID%>").value = coorAcc;
        document.getElementById("<%=HiddenField4.ClientID%>").Value = coorSpe;
        dane[0] = document.getElementById("<%=HiddenField1.ClientID%>").value;
        dane[1] = document.getElementById("<%=HiddenField2.ClientID%>").value;
        dane[2] = document.getElementById("<%=HiddenField3.ClientID%>").value;
        dane[3] = document.getElementById("<%=HiddenField4.ClientID%>").value;
        PageMethods.PrzeliczArray(dane, OnSuccess);
    }
    function OnSuccess(dane, userContext, PlaySound() {
<% --document.getElementById("<%=lb_BusLatitude.ClientID %>").innerHTML = response[0];
        document.getElementById("<%=lb_BusLongitude.ClientID %>").innerHTML = response[1];
        document.getElementById("<%=lb_BusDistance.ClientID %>").innerHTML = response[2];
        document.getElementById("<%=lb_BusAccuracy.ClientID %>").innerHTML = wartosc3; --%>
            coorLat = "";
        coorLon = "";
        coorAcc = "";
        coorSpe = "";

<% --            var txt = "";
        txt += "<p>Total width/height: " + screen.width + "*" + screen.height + "</p>";
        txt += "<p>Available width/height: " + screen.availWidth + "*" + screen.availHeight + "</p>";
        document.getElementById("<%=lblDim.ClientID %>").innerHTML = txt; --%>
            }



function PlaySound(nazwa) {
    _nazwa = nazwa;
    if (_nazwa != "") {
        if (soundObject != null) {
            document.body.removeChild(soundObject);
            soundObject.removed = true;
            soundObject = null;
        }
        soundObject = document.createElement("embed");
        soundObject.setAttribute("src", _nazwa);
        soundObject.setAttribute("hidden", true);
        soundObject.setAttribute("autostart", true);
        document.body.appendChild(soundObject);
    }
}
