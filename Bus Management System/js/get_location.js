//var lat = document.getElementById("Latitude");
//var lon = document.getElementById("Longitude");

setInterval(getLocation, 5000);

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showCoordinates, function () { }, { enableHighAccuracy: true });
    }
    else {
        lat.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
    }
}

function showCoordinates(coordinates) {

    document.getElementById("<%=lb_Latitude.ClientID %>").innerHTML = coordinates.coords.latitude;
    document.getElementById("<%=lb_Longitude.ClientID %>").innerHTML = coordinates.coords.longitude;
    document.getElementById("<%=lb_Accuracy.ClientID %>").innerHTML = coordinates.coords.accuracy;
}

function ShowCurrentTime() {
    PageMethods.GetCurrentTime(document.getElementById("<%=tb_JakiesDane.ClientID%>").value, OnSuccess);
}

function OnSuccess(response, userContext, methodName) {
    document.getElementById("<%=lb_ActualTime.ClientID %>").innerHTML = response;
}

        //function showError(error) {
        //    switch (error.code) {
        //        case error.PREMISSION_DENIED:
        //            lat.innerHTML = "Użytkownik odmówił pobrania lokalizacji"
        //            break;
        //        case error.POSITION_UNAVAILABLE:
        //            lat.innerHTML = "Informacje o lokalizacji są niedostępne"
        //            break;
        //        case error.TIMEOUT:
        //            lat.innerHTML = "Przekroczenie czasu oczekiwania na pobranie współrzędnych"
        //            break;
        //        case error.UNKNOWN_ERROR:
        //            lat.innerHTML = "Wystąpił nieznany błąd"
        //            break;
        //    }
        //}
