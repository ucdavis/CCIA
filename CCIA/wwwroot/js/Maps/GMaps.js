let map;
async function initMap() {
    const { Map } = await google.maps.importLibrary("maps");


    map = new Map(document.getElementById("map"), {
        center: { lat: 37.6, lng:  - 118.33 },
        zoom: 6,
        streetViewControl: false,
        mapId: "CCIAMap",
        mapTypeId: "satellite",
    });
}
initMap();

function Panmap() {
    var x = $("#txtLat").val();
    var y = $("#txtLong").val();
    if (isNaN(x) || x < 32 || x > 42) {
        alert('Latitude must be a number between 32 and 42');
    }
    else
        if (isNaN(y) || y < -125 || y > -114) {
            alert('Longitude must be a number between -125 and -114');
        }
        else {
            map.setCenter(new google.maps.LatLng(x, y));
            map.setZoom(14);            
        }
};