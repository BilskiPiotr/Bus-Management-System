var lat = document.getElementById("Latitude");
var lon = document.getElementById("Longitude");

function getLocation()
{
    if (navigator.geolocation)
    {
        navigator.geolocation.getCurrentPosition(showCoordinates, function () { }, { enableHighAccuracy: true });
    }
    else
    {
        lat.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
    }
}

function showCoordinates(coordinates)
{
    document.getElementById("<%=lb_Latitude.ClientID %>").innerHTML = coordinates.coords.latitude;
    document.getElementById("<%=lb_Longitude.ClientID %>").innerHTML = coordinates.coords.longitude;
    document.getElementById("<%=lb_Accuracy.ClientID %>").innerHTML = coordinates.coords.accuracy;
    document.getElementById("<%=Speed.ClientID %>").innerHTML = coordinates.coords.speed;
}

