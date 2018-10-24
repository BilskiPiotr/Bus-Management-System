var wartosc1 = "";
var wartosc2 = "";
var wartosc3 = "";
var speed = "";

var soundPath = "";

var error = "";
var dane = new Array();



function getLocation()
{
    if (navigator.geolocation)
    {
        navigator.geolocation.getCurrentPosition(KonstruujArray, function () { }, { enableHighAccuracy: true });
    }
    else {
        error.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
    }
}

function KonstruujArray(coordinates)
{
    wartosc1 = coordinates.coords.latitude;
    wartosc2 = coordinates.coords.longitude;
    wartosc3 = coordinates.coords.accuracy;
    speed = coordinates.coords.speed;

    document.getElementById("<%=HiddenField1.ClientID%>").value = wartosc1;
    document.getElementById("<%=HiddenField2.ClientID%>").value = wartosc2;
    document.getElementById("<%=HiddenField3.ClientID%>").value = wartosc3;
    document.getElementById("<%=HiddenField4.ClientID%>").value = speed;

    dane[0] = document.getElementById("<%=HiddenField1.ClientID%>").value;
    dane[1] = document.getElementById("<%=HiddenField2.ClientID%>").value;
    dane[2] = document.getElementById("<%=HiddenField3.ClientID%>").value;
    dane[3] = document.getElementById("<%=HiddenField4.ClientID%>").value;
    PageMethods.PrzeliczArray(dane, OnSuccess);
}

function OnSuccess(response)
{

    soundPath = response;
    PlaySound(soundPath);
}